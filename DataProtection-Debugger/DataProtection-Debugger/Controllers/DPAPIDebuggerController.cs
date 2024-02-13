using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Data_Protection_API___Start_project.Controllers
{
    /// <summary>
    /// Data Protection API Debugger
    /// 
    /// Written by Tore Nestenius
    /// Blog: https://nestenius.se
    /// Business: https://www.tn-data.se
    /// </summary>
    public class DPAPIDebuggerController : Controller
    {
        private readonly ILogger<DPAPIDebuggerController> _logger;
        private readonly IDataProtectionProvider dataProtection;
        private readonly XmlKeyManager? keyManager;

        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_KeyEncryptor")]
        extern static IXmlEncryptor GetKeyEncryptorField(XmlKeyManager @this);
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_KeyRepository")]
        extern static IXmlRepository GetKeyRepositoryField(XmlKeyManager @this);

        public DPAPIDebuggerController(ILogger<DPAPIDebuggerController> logger,
                                       IDataProtectionProvider dataProtection,
                                       IKeyManager keyManager)
        {
            _logger = logger;
            this.dataProtection = dataProtection;
            this.keyManager = keyManager as XmlKeyManager;
        }

        public IActionResult Index()
        {
            if (keyManager != null)
            {
                //Get the name of the key manager type
                ViewData["KeyManagerName"] = keyManager.GetType().Name;

                //Get these two intances from the private field inside the XmlKeyManager
                IXmlEncryptor encryptor = GetKeyEncryptorField(keyManager);
                IXmlRepository repository = GetKeyRepositoryField(keyManager);

                //Get the name of the current encryptor and repository
                ViewData["IXmlEncryptor"] = encryptor?.GetType().Name ?? "NULL";
                ViewData["IXmlRepository"] = repository?.GetType().Name ?? "NULL";

                //Get all the raw keys from the repository 
                if (repository != null)
                {
                    List<System.Xml.Linq.XElement> repositoryKeys = repository.GetAllElements().ToList();

                    var xml = PrettyPrint(repositoryKeys);
                    ViewData["RepositoryElements"] = xml;
                }
                else
                {
                    ViewData["RepositoryElements"] = "Not found";
                }

                IReadOnlyCollection<IKey> allKeys = keyManager.GetAllKeys();
                ViewData["AllKeys"] = allKeys.ToList();
            }
            else
            {
                ViewData["AllKeys"] = new List<IKey>();
            }

            return View();
        }


        [HttpPost]
        public IActionResult CreateNewKey()
        {
            //Add a new key to the key ring that have a life-time of 60 seconds
            keyManager?.CreateNewKey(
                activationDate: DateTimeOffset.Now,
                expirationDate: DateTimeOffset.Now.AddSeconds(60));

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult RevokeAllKeys()
        {
            //All keys with a creation date before this date will be revoked.

            keyManager?.RevokeAllKeys(DateTimeOffset.Now, "We got hacked!!!");

            return RedirectToAction("Index");
        }


        public string PrettyPrint(List<XElement> elements)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ", // or "\t" for tab
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            var stringBuilder = new StringBuilder();

            foreach (var element in elements)
            {
                using (var writer = XmlWriter.Create(stringBuilder, settings))
                {
                    element.WriteTo(writer);
                }
                stringBuilder.AppendLine(); // Add an extra line between elements
            }

            return stringBuilder.ToString();
        }
    }
}
