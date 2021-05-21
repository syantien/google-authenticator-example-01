using Google.Authenticator;
using GoogleAuthenticatorExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleAuthenticatorExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _secretKey = "SuperSecretKeyGoesHere";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("/")]
        public IActionResult Index()
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode("MyExampleApp", "user@example.com", _secretKey, false, 300);

            string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
            string manualEntrySetupCode = setupInfo.ManualEntryKey;

            ViewData["qrCodeImageUrl"] = qrCodeImageUrl;
            ViewData["manualEntrySetupCode"] = manualEntrySetupCode;

            return View();
        }

        [Route("/Authenticate")]
        public IActionResult Result(string code)
        {

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            bool isCorrectPIN = tfa.ValidateTwoFactorPIN(_secretKey, code);

            if (isCorrectPIN)
            {
                ViewData["Result"] = "OK";
            }
            else
            {
                ViewData["Result"] = "NOT OK";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
