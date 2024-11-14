using AutoMapper;
using DU_CertificateApp.Manager.Contract;
using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DU_CertificateApp.Controllers
{
    public class PaymentMethodController : Controller
    {
        private readonly IMapper _iMapper;
        private readonly IPaymentMethodManager _iPaymentMethodManager;

        public PaymentMethodController(IMapper iMapper, IPaymentMethodManager iPaymentMethodManager)
        {
            _iMapper = iMapper;
            _iPaymentMethodManager = iPaymentMethodManager;
        }

        public async Task<ActionResult<IEnumerable<PaymentMethodViewModel>>> Index()
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            IEnumerable<PaymentMethodViewModel> paymentMethods = _iMapper.Map<IEnumerable<PaymentMethodViewModel>>(await _iPaymentMethodManager.GetAll());

            return View(paymentMethods);
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
        public async Task<IActionResult> Create(PaymentMethodViewModel paymentMethod)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (ModelState.IsValid)
            {
                PaymentMethod AddPaymentMethod = _iMapper.Map<PaymentMethod>(paymentMethod);
                bool IsAdded = await _iPaymentMethodManager.Create(AddPaymentMethod);

                if (IsAdded)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Create PaymentMethod";
            }
            return View(paymentMethod);
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

            PaymentMethodViewModel existPaymentMethod = _iMapper.Map<PaymentMethodViewModel>(await _iPaymentMethodManager.GetById(id));

            if (existPaymentMethod == null)
                return NotFound();

            return View(existPaymentMethod);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaymentMethodViewModel ExistPaymentMethod)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (ModelState.IsValid)
            {
                PaymentMethod PaymentMethod = _iMapper.Map<PaymentMethod>(ExistPaymentMethod);

                bool IsUpdated = await _iPaymentMethodManager.Update(PaymentMethod);

                if (IsUpdated)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Update PaymentMethod";
            }
            return View(ExistPaymentMethod);
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

            PaymentMethod existPaymentMethod = await _iPaymentMethodManager.GetById(id);

            if (existPaymentMethod == null)
                return NotFound();

            bool remove = await _iPaymentMethodManager.Remove(existPaymentMethod);

            if (remove)
                return RedirectToAction("Index");
            else
                ViewBag.ErrorMessage = "Failed to Delete PaymentMethod";

            return BadRequest();
        }
    }
}
