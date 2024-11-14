using AutoMapper;
using DU_CertificateApp.Database.Database;
using DU_CertificateApp.Manager;
using DU_CertificateApp.Manager.Contract;
using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace DU_CertificateApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly DU_CertificateDB _db;
        private readonly IMapper _iMapper;
        private readonly ICartManager _iCartManager;
        private readonly IOrderManager _iOrderManager;
        private readonly ICertificateManager _iCertificateManager;
        private readonly IOrderCertificateManager _iOrderCertificateManager;
        private readonly IStudentManager _iStudentManager;
        public OrderController(IMapper iMapper, ICartManager iCartManager,IOrderManager iOrderManager, ICertificateManager iCertificateManager, IOrderCertificateManager iOrderCertificateManager, IStudentManager iStudentManager)
        {
            _db = new DU_CertificateDB();
            _iMapper = iMapper;
            _iCartManager = iCartManager;
            _iOrderManager = iOrderManager;
            _iCertificateManager = iCertificateManager;
            _iOrderCertificateManager = iOrderCertificateManager;
            _iStudentManager = iStudentManager;
        }
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> Index()
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            //Placed orders only should be here
            if (HttpContext.Session.GetString("Membership") == "Admin" || HttpContext.Session.GetString("Membership") == "Student")
            {
                 ViewBag.Orders = _db.Orders.Where(w=>w.StudentID == Convert.ToInt32(HttpContext.Session.GetString("ID"))).ToList();
            }
            else if(HttpContext.Session.GetString("Membership") == "Admin" || HttpContext.Session.GetString("Membership") == "Check1")
            {
                ViewBag.Orders = _db.Orders.Where(w=>w.Status == "Accept").ToList();
            }
            else if(HttpContext.Session.GetString("Membership") == "Admin" || HttpContext.Session.GetString("Membership") == "Check2")
            {
                ViewBag.Orders = _db.Orders.Where(w=>w.Status == "Approve").ToList();
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            if (TempData["Error"] != null)
            {
                ViewBag.ErrorMessage = TempData["Error"];
            }
            ViewBag.Certificates = _iMapper.Map<IEnumerable<CertificateViewModel>>(await _iCertificateManager.GetAll());
            ViewBag.Cart = _db.Carts.Where(i=>i.StudentID == 1).ToList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PlaceOrder(int price)
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("ID"));           
            ICollection<Cart> carts = _db.Carts.Where(i=>i.StudentID == id).ToList();

            //Create Order
            Order order = new Order();
            order.Price = price;
            order.StudentID = id;
            order.Status = "Pending";
            order.OrderedDate = DateTime.Now;
            bool save = await _iOrderManager.Create(order);

            if (save)
            {
                int orderID = _db.Orders.Where(w=>w.Status == "Pending").Where(z=>z.StudentID == id).Where(y=>y.Price == price).Select(s=>s.ID).FirstOrDefault();

                if(orderID == 0)
                {
                    //Failed to find order number
                }

                List<OrderCertificate> orderCertificate = new List<OrderCertificate>();
                

                foreach (Cart i in carts)
                {
                    OrderCertificate addOrder = new OrderCertificate()
                    {
                        OrderID = orderID,
                        CertificateID = i.CertificateID
                };
                    orderCertificate.Add(addOrder);
                    price = price + _db.Certificates.Where(c=>c.ID == i.CertificateID).Select(s=>s.Price).FirstOrDefault();                   
                }

                _db.OrderCertificates.AddRange(orderCertificate);
                
                int save2 = _db.SaveChanges();
                if(save2<=0)
                {
                    //Failed to create Order Certificate
                }

                _db.Carts.RemoveRange(carts);
                int save3 = _db.SaveChanges();
                if(save3<=0)
                {
                    //Failed to clear cartlist
                }
            }
            else
            {
                //Failed to Place Order
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddCart(int id)
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            // Check if the request has the ID parameter
            if (id != 0)
            {
                Cart? IsAvailable = _db.Carts.Where(c=>c.CertificateID == id).Where(c=>c.StudentID == 1).FirstOrDefault();
                if (IsAvailable != null)
                {
                    TempData["Error"] = "Already added to the Cart";
                    return RedirectToAction("Create");
                }

                Cart addtoCart = new Cart();
                addtoCart.CertificateID = id;
                addtoCart.StudentID = 1;

                bool save = await _iCartManager.Create(addtoCart);
                if (save)
                {
                    // Refresh the cart list after adding the item
                    return RedirectToAction("Create");
                }
            }
            else
            {
                TempData["Error"] = "Failed to update Cart";
            }
            // Refresh the cart list even if there's an error
            return RedirectToAction("Create");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveCart(int id)
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            // Check if the request has the ID parameter
            if (id != 0)
            {
                Cart? IsAvailable = _db.Carts.Where(c => c.CertificateID == id).Where(c => c.StudentID == 1).FirstOrDefault();
                if (IsAvailable == null)
                {
                    TempData["Error"] = "Not Exists in the Cart";
                    return RedirectToAction("Create");
                }

                Cart? removefromCart = _db.Carts.Where(w=>w.CertificateID == id).FirstOrDefault();

                if(removefromCart!= null)
                {
                    bool save = await _iCartManager.Remove(removefromCart);
                    if (save)
                    {
                        // Refresh the cart list after removing the item
                        return RedirectToAction("Create");
                    }
                }      
            }
            else
            {
                TempData["Error"] = "Failed to update Cart";
            }
            // Refresh the cart list even if there's an error
            return RedirectToAction("Create");
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

            if(TempData["ErrorMessage"]!= null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }

            OrderViewModel existsOrder = _iMapper.Map<OrderViewModel>(await _iOrderManager.GetById(id));

            if (existsOrder == null || existsOrder.Status != "Pending") 
            {
                ViewBag.ErrorMessage = "Unable to Update Order!!!";
                return RedirectToAction("Index");
            }
            ViewBag.existsOrder = existsOrder;

            List<CertificateViewModel> existsCertificates = new List<CertificateViewModel>();

            List<int> CertificatesIDs = _db.OrderCertificates.Where(w=>w.OrderID == id).Select(s=>s.CertificateID).ToList();
            foreach(int i in CertificatesIDs)
            {
                CertificateViewModel certificate = _iMapper.Map<CertificateViewModel>(await _iCertificateManager.GetById(i));
                existsCertificates.Add(certificate);
            }
            if (existsCertificates == null)
               return NotFound();

            ViewBag.ExistCertificates = existsCertificates;

            ViewBag.Certificates = _iMapper.Map<IEnumerable<CertificateViewModel>>(await _iCertificateManager.GetAll());
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddOnUpdate(int id, int orderId)
        {
            try
            {               
                if (id == 0 || orderId == 0)
                {
                    ViewBag.ErrorMessage = "Failed to update Order!!!";
                    return RedirectToAction("Index");
                }
                int cer = _db.OrderCertificates.Where(a=>a.OrderID == orderId).Where(b=>b.CertificateID==id).Count();
                if(cer>0)
                {
                    TempData["ErrorMessage"] = "Order already exists!!!";
                    return RedirectToAction("Update", "Order", new { id = orderId });
                }
                OrderCertificate order = new OrderCertificate();
                order.OrderID = orderId;
                order.CertificateID = id;

                bool save = await _iOrderCertificateManager.Create(order);
                if (save)
                {
                    return RedirectToAction("Update", "Order", new { id = orderId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update Order!!!";
                    return RedirectToAction("Update", "Order", new { id = orderId });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromUpdate(int id, int orderId)
        {
            try
            {
                if (id == 0 || orderId == 0)
                {
                    ViewBag.ErrorMessage = "Failed to update Order!!!";
                    return RedirectToAction("Index");
                }

                OrderCertificate? order = _db.OrderCertificates.Where(a => a.OrderID == orderId).Where(b => b.CertificateID == id).FirstOrDefault();
                if (order != null)
                {
                    bool save = await _iOrderCertificateManager.Remove(order);
                    if (save)
                    {
                        return RedirectToAction("Update", "Order", new { id = orderId });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update Order!!!";
                        return RedirectToAction("Update", "Order", new { id = orderId });
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Order or Certificate not found!!!";
                    return RedirectToAction("Update", "Order", new { id = orderId });
                }
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message; 
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Proceed(int? id)
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            if (id == null)
                return NotFound();

            Order existOrder = await _iOrderManager.GetById(id);

            if (existOrder == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                //Student
                if (existOrder.Status == "Pending")
                {
                    existOrder.Status = "Accept";
                    existOrder.OrderedDate = DateTime.Now;
                }
                //Check1
                else if (existOrder.Status == "Accept")
                {
                    existOrder.Status = "Approve";
                    existOrder.AcceptedID = Convert.ToInt32(HttpContext.Session.GetString("ID"));
                    existOrder.AcceptedDate = DateTime.Now;
                }
                //Check2
                else if (existOrder.Status == "Approve")
                {
                    existOrder.Status = "Complete";
                    existOrder.ApprovedID = Convert.ToInt32(HttpContext.Session.GetString("ID"));
                    existOrder.ApprovedDate = DateTime.Now;
                }

                // Update the existing OrderViewModel object directly
                bool IsUpdated = await _iOrderManager.Update(existOrder);

                if (!IsUpdated)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewOrder(int? id)
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            if (id == null)
                return NotFound();

            OrderViewModel existOrder = _iMapper.Map<OrderViewModel>(await _iOrderManager.GetById(id));

            if (existOrder == null)
                return NotFound();

            List<CertificateViewModel> certificates = new List<CertificateViewModel>();
            CertificateViewModel cer = new CertificateViewModel();
           
            List<int> CertificateIDs = _db.OrderCertificates.Where(w=>w.OrderID == id).Select(s=>s.CertificateID).ToList();

            foreach(int i in CertificateIDs)
            {
                cer = _iMapper.Map<CertificateViewModel>(await _iCertificateManager.GetById(i));
                certificates.Add(cer);
            }
            ViewBag.Certificates = certificates;
            ViewBag.Student = _iMapper.Map<StudentViewModel>(await _iStudentManager.GetById(existOrder.StudentID));

            return View(existOrder);
        }
    }
}
