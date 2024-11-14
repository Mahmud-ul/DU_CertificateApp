using DU_CertificateApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DU_CertificateApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion
            if(HttpContext.Session.GetString("Membership") == "Student")
                return RedirectToAction("Index", "Student");

            return RedirectToAction("Index", "User");
        }
    }
}
