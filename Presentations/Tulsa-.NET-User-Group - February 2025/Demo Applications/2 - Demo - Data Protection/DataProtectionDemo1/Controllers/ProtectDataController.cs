using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Data_Protection_API___Start_project.Controllers
{
    public class ProtectDataController : Controller
    {
        private readonly ILogger<ProtectDataController> _logger;
        private readonly IDataProtectionProvider dataProtection;

        public ProtectDataController(ILogger<ProtectDataController> logger, IDataProtectionProvider dataProtection)
        {
            _logger = logger;
            this.dataProtection = dataProtection;
        }

        public IActionResult Index(CryptoModel model)
        {
            if (model == null)
                model = new CryptoModel();

            model.EncryptPurpose = "CustomerData.v1";
            model.DecryptPurpose = "CustomerData.v1";

            return View(model);
        }

        [HttpPost]
        public IActionResult Encrypt(CryptoModel model)
        {
            var _protector = dataProtection.CreateProtector(model.EncryptPurpose);

            model.EncryptedData = _protector.Protect(model.DataToEncrypt);
            model.DataToDecrypt = model.EncryptedData;

            return RedirectToAction("Index", model);
        }

        [HttpPost]
        public IActionResult Decrypt(CryptoModel model)
        {
            model.Exception = "";

            try
            {
                var _protector = dataProtection.CreateProtector(model.DecryptPurpose);

                model.DecryptedData = _protector.Unprotect(model.DataToDecrypt);
            }
            catch (Exception ex)
            {
                model.Exception = ex.ToString();
            }
            return RedirectToAction("Index", model);
        }

    }
}
