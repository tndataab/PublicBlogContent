using Azure.Core;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace TnData.AzureKeyVaultExtensions;

public static class DataProtectionExtensions
{
    /// <summary>
    /// Configures the data protection system to persist the key-ring as a Azure Key Vault secret
    /// 
    /// Written by Tore Nestenius
    /// Blog: https://nestenius.se
    /// Business: https://www.tn-data.se
    /// </summary>
    /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
    /// <param name="credentials">the Azure credentials</param>
    /// <param name="KeyVaultUrl">The base URL to your Azure Key Vault</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="credentials"/> is null or empty
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="vaultUri"/> is null or empty
    /// </exception>  
    /// <exception cref="ArgumentException">
    /// <paramref name="keyRingName"/> is null or empty
    /// </exception>
    /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
    public static IDataProtectionBuilder PersistKeysToAzureKeyVault(this IDataProtectionBuilder builder,
                                                                    TokenCredential credentials,
                                                                    Uri vaultUri,
                                                                    string keyRingName)
    {
        ArgumentNullException.ThrowIfNull(credentials);
        ArgumentNullException.ThrowIfNull(vaultUri);
        ArgumentNullException.ThrowIfNull(keyRingName);

        builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services =>
        {
            return new ConfigureOptions<KeyManagementOptions>(options =>
            {
                var loggerFactory = services.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;

                options.XmlRepository = new AzureKeyVaultKeyRingRepository(credentials, vaultUri, keyRingName, loggerFactory);
            });
        });

        return builder;
    }

    /// <summary>
    /// Protect the key ring with no protection at all
    ///
    /// This extra extension method can be used if you for some reason don't want to encrypt the data at rest
    /// </summary>
    /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
    /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
    public static IDataProtectionBuilder ProtectKeysWithNoEncryption(this IDataProtectionBuilder builder)
    {
        builder.Services.Configure<KeyManagementOptions>(options =>
        {
            options.XmlEncryptor = new NullXmlEncryptor();
        });

        return builder;
    }
}
