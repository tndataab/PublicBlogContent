using Azure;
using Azure.Core;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Text;
using System.Xml.Linq;

namespace TnData.AzureKeyVaultExtensions;

/// <summary>
/// AzureKeyVaultKeyRingRepository
/// ==============================
/// Implementation of an IXmlRepository that will store the key ring as a secret in Azure Key Vault.
/// In Azure Key Vault we can store about 12 keys (without key ring encryption)
///
/// Created by Tore Nestenius
/// https://www.tn-data.se
/// /// </summary>
public class AzureKeyVaultKeyRingRepository : IXmlRepository
{
    private readonly ILogger<AzureKeyVaultKeyRingRepository> logger;
    private readonly string keyRingName;
    private readonly TokenCredential credentials;
    private readonly Uri vaultUri;
    private readonly SecretClient client;

    /// <summary>
    /// According to https://social.technet.microsoft.com/wiki/contents/articles/52480.azure-key-vault-overview.aspx the max
    /// secret size in AKV is 25K, that represents about 12-13 keys and that should be more than enough.
    /// It could be increased if we applied some compression.
    /// 
    /// Futher improvements could include the introduction of caching and retry if we fail to connect to AKV.
    /// </summary>
    private const int AzureKeyVaultSecretMaxLength = 25000;

    //To avoid creating a too large secret, we limit the number of keys to 10.
    private const int MaxKeysInKeyRing = 10;


    /// <summary>
    /// Creates a new instance of <see cref="AzureKeyVaultKeyRingRepository"/>.
    /// </summary>
    /// <param name="credentials">The azure credentials</param>
    /// <param name="vaultUri">The URL to your Azure Key Vault</param>
    /// <param name="keyRingName">The name of the secret in Azure Key Vault that will hold the key ring</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
    public AzureKeyVaultKeyRingRepository(TokenCredential credentials,
                                          Uri vaultUri,
                                          string keyRingName,
                                          ILoggerFactory loggerFactory)
    {
        client = new SecretClient(vaultUri, credentials);

        this.credentials = credentials;
        this.keyRingName = keyRingName;
        this.vaultUri = vaultUri;

        this.logger = loggerFactory.CreateLogger<AzureKeyVaultKeyRingRepository>();
    }

    /// <summary>
    /// Gets all top-level XML elements from Azure Key Vault
    /// </summary>
    /// <returns></returns>
    public IReadOnlyCollection<XElement> GetAllElements()
    {
        logger.LogInformation("Loading Data Protection key ring from Azure Key Vault");

        try
        {
            KeyVaultSecret kvSecret = client.GetSecret(keyRingName);

            var encoded = kvSecret.Value;

            //The data stored in key vault is base64 encoded
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));

            XDocument doc = XDocument.Parse(decoded);

            var keyring = doc.Root.Elements().ToList();

            logger.LogInformation("Loaded {keycount} Keys from Azure Key Vault", keyring.Count);
            logger.LogInformation("Key Ring size in Key Vault is {size}", encoded.Length);

            return keyring;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            logger.LogWarning("Failed to load secret '{secretname}' from Azure Key Vault {ExceptionMessage}, Http.StatusCode={StatusCode}", keyRingName, ex.Message, ex.Status);
            return new List<XElement>();
        }
    }

    /// <summary>
    /// Add a new key to the Key Ring in Azure Key Vault
    /// </summary>
    /// <param name="newElement">The element to add</param>
    /// <param name="friendlyName">An optional name to be associated with the XML element.</param>
    public void StoreElement(XElement newElement, string friendlyName)
    {
        ArgumentNullException.ThrowIfNull(newElement);

        logger.LogInformation("Adding key {key} to Data Protection Key Ring", friendlyName);

        //First get a copy of existing keys in Azure Key Vault
        var existingKeys = GetAllElements();

        SaveKeyRingToAzureKeyVault(newElement, existingKeys);
    }

    /// <summary>
    /// Save all the keys in the key ring to Azure Key Vault
    /// </summary>
    private void SaveKeyRingToAzureKeyVault(XElement newElement, IReadOnlyCollection<XElement> existingKeys)
    {
        var keysToStore = new List<XElement>();

        keysToStore.Add(newElement);
        keysToStore.AddRange(existingKeys);

        //Limit the # of keys we store in the key ring
        keysToStore = keysToStore.Take(MaxKeysInKeyRing).ToList();

        //Convert the list of XElement to an XDocument
        var doc = new XDocument();
        var root = new XElement("root");
        doc.Add(root);
        foreach (var key in keysToStore)
        {
            root.Add(key);
        }

        //Base-64 the XDocument for easier storage
        string encodedStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(doc.ToString()));

        StoreSecretInAzureKeyVault(encodedStr);
    }

    /// <summary>
    /// Store the secret in Azure Key Vault
    /// </summary>
    /// <param name="encodedStr"></param>
    private void StoreSecretInAzureKeyVault(string encodedStr)
    {
        //Secrets properties documentation https://docs.microsoft.com/en-us/dotnet/api/azure.security.keyvault.secrets.secretproperties?view=azure-dotnet-preview
        var newSecret = new KeyVaultSecret(name: keyRingName, value: encodedStr)
        {
            Properties =
            {
                ContentType = "text/plain",
            }
        };

        try
        {
            client.SetSecret(newSecret);
        }
        catch (RequestFailedException ex) when (ex.Status == 409)
        {
            //Conflict!
            logger.LogWarning("Failed to set secret '{secretname}' from Azure Key Vault {ExceptionMessage}, Http.StatusCode={StatusCode}", keyRingName, ex.Message, ex.Status);

            //Try to pruge it and then try to set it again
            try
            {
                logger.LogWarning("Trying to purge the secret from Azure Key Vault", keyRingName, ex.Message, ex.Status);
                client.PurgeDeletedSecret(keyRingName);
            }
            catch (Exception)
            {
                //We don't care if this fails
            }

            //Let's try to set it again
            client.SetSecret(newSecret);
        }


        logger.LogInformation("Key Ring Size in Azure KeyVault is {size}", encodedStr.Length);

        //Notify in the log, if the keyring size grows over 20Kb in size
        if (encodedStr.Length > (AzureKeyVaultSecretMaxLength - 5000))
            logger.LogCritical("Key Ring Size is getting to big, current size is {size}, max size is 25Kb", encodedStr.Length);

    }
}
