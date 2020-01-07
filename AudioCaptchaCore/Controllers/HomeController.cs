using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AudioCaptchaCore.Models;
using Pigi.Captcha;
using Microsoft.Extensions.Logging;

namespace AudioCaptchaCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //var logger = (ILogger<HomeController>)ControllerContext.HttpContext.RequestServices.GetService(typeof (ILogger<HomeController>));
            //logger.LogInformation("Seeded the database.");
            return View();
        }

        [HttpPost]
        public ActionResult Index(string c1)
        {
            if (string.IsNullOrEmpty(c1))
            {
                ViewBag.c1 = false;
                return View();
            }
            var isCaptcha1Valid = CaptchaManager.ValidateCurrentCaptcha("c1", c1);

            ViewBag.c1 = isCaptcha1Valid;
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
