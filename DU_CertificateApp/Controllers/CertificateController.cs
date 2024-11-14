using AutoMapper;
using DU_CertificateApp.Manager.Contract;
using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DU_CertificateApp.Controllers
{
    public class CertificateController : Controller
    {
        private readonly IMapper _iMapper;
        private readonly ICertificateManager _iCertificateManager;

        public CertificateController(IMapper iMapper, ICertificateManager iCertificateManager)
        {
            _iMapper = iMapper;
            _iCertificateManager = iCertificateManager;
        }

        public async Task<ActionResult<IEnumerable<CertificateViewModel>>> Index()
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            IEnumerable<CertificateViewModel> certificates = _iMapper.Map<IEnumerable<CertificateViewModel>>(await _iCertificateManager.GetAll());

            return View(certificates);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CertificateViewModel certificate)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (ModelState.IsValid)
            {
                Certificate AddCertificate = _iMapper.Map<Certificate>(certificate);
                bool IsAdded = await _iCertificateManager.Create(AddCertificate);

                if (IsAdded)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Create Certificate";
            }
            return View(certificate);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (id == null)
                return NotFound();

            CertificateViewModel existCertificate = _iMapper.Map<CertificateViewModel>(await _iCertificateManager.GetById(id));

            if (existCertificate == null)
                return NotFound();

            return View(existCertificate);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CertificateViewModel ExistCertificate)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (ModelState.IsValid)
            {
                Certificate certificate = _iMapper.Map<Certificate>(ExistCertificate);

                bool IsUpdated = await _iCertificateManager.Update(certificate);

                if (IsUpdated)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Update Certificate";
            }
            return View(ExistCertificate);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int? id)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (id == null)
                return NotFound();

            Certificate existCertificate = await _iCertificateManager.GetById(id);

            if (existCertificate == null)
                return NotFound();

            bool remove = await _iCertificateManager.Remove(existCertificate);

            if (remove)
                return RedirectToAction("Index");
            else
                ViewBag.ErrorMessage = "Failed to Delete Certificate";

            return BadRequest();
        }
    }
}
