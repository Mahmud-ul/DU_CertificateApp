using AutoMapper;
using DU_CertificateApp.Manager;
using DU_CertificateApp.Manager.Contract;
using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DU_CertificateApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IMapper _iMapper;
        private readonly IPaymentManager _iPaymentManager;
        private readonly IPaymentMethodManager _iPaymentMethodManager;

        public PaymentController(IMapper iMapper, IPaymentManager iPaymentManager, IPaymentMethodManager iPaymentMethodManager)
        {
            _iMapper = iMapper;
            _iPaymentManager = iPaymentManager;
            _iPaymentMethodManager = iPaymentMethodManager;
        }
        public async Task<ActionResult<IEnumerable<PaymentViewModel>>> Index()
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            IEnumerable<PaymentViewModel> payments = _iMapper.Map<IEnumerable<PaymentViewModel>>(await _iPaymentManager.GetAll());

            return View(payments);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            ViewBag.PaymentMethods = _iMapper.Map<IEnumerable<PaymentMethodViewModel>>(await _iPaymentMethodManager.GetAll());
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentViewModel payment)
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            if (ModelState.IsValid)
            {
                Payment AddPayment = _iMapper.Map<Payment>(payment);
                bool IsAdded = await _iPaymentManager.Create(AddPayment);

                if (IsAdded)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Create Payment";
            }
            ViewBag.PaymentMethods = _iMapper.Map<IEnumerable<PaymentMethodViewModel>>(await _iPaymentMethodManager.GetAll());
            return View(payment);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            if (id == null)
                return NotFound();

            PaymentViewModel existPayment = _iMapper.Map<PaymentViewModel>(await _iPaymentManager.GetById(id));

            if (existPayment == null)
                return NotFound();

            ViewBag.PaymentMethods = _iMapper.Map<IEnumerable<PaymentMethodViewModel>>(await _iPaymentMethodManager.GetAll());
            return View(existPayment);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaymentViewModel ExistPayment)
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            if (ModelState.IsValid)
            {
                Payment Payment = _iMapper.Map<Payment>(ExistPayment);

                bool IsUpdated = await _iPaymentManager.Update(Payment);

                if (IsUpdated)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Update Payment";
            }
            ViewBag.PaymentMethods = _iMapper.Map<IEnumerable<PaymentMethodViewModel>>(await _iPaymentMethodManager.GetAll());
            return View(ExistPayment);
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

            Payment existPayment = await _iPaymentManager.GetById(id);
            if (existPayment == null)
                return NotFound();

            bool remove = await _iPaymentManager.Remove(existPayment);

            if (remove)
                return RedirectToAction("Index");
            else
                ViewBag.ErrorMessage = "Failed to Delete Payment";

            return BadRequest();
        }
    }
}
