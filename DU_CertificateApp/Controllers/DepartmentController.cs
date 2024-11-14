using AutoMapper;
using DU_CertificateApp.Manager.Contract;
using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DU_CertificateApp.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IMapper _iMapper;
        private readonly IDepartmentManager _iDepartmentManager;

        public DepartmentController(IMapper iMapper, IDepartmentManager iDepartmentManager)
        {
            _iMapper = iMapper;
            _iDepartmentManager = iDepartmentManager;
        }

        public async Task<ActionResult<IEnumerable<DepartmentViewModel>>> Index()
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            IEnumerable<DepartmentViewModel> departments = _iMapper.Map<IEnumerable<DepartmentViewModel>>( await _iDepartmentManager.GetAll());

            return View(departments);
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
        public async Task<IActionResult> Create(DepartmentViewModel department)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (ModelState.IsValid)
            {
                Department AddDepartment = _iMapper.Map<Department>(department);
                bool IsAdded = await _iDepartmentManager.Create(AddDepartment);

                if (IsAdded)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Create Department";
            }
            return View(department);
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

            DepartmentViewModel existDepartment = _iMapper.Map<DepartmentViewModel>( await _iDepartmentManager.GetById(id));

            if (existDepartment == null)
                return NotFound();

            return View(existDepartment);
        }

        [HttpPost]
        public async Task<IActionResult> Update(DepartmentViewModel ExistDepartment)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (ModelState.IsValid)
            {
                Department department = _iMapper.Map<Department>(ExistDepartment);

                bool IsUpdated = await _iDepartmentManager.Update(department);

                if (IsUpdated)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Update Department";
            }
            return View(ExistDepartment);
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

            Department existDepartment = await _iDepartmentManager.GetById(id);

            if (existDepartment == null)
                return NotFound();

            bool remove = await _iDepartmentManager.Remove(existDepartment);

            if (remove)
                return RedirectToAction("Index");
            else
                ViewBag.ErrorMessage = "Failed to Delete Department";

            return BadRequest();
        }
    }
}
