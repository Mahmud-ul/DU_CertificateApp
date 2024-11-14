using AutoMapper;
using DU_CertificateApp.Manager.Contract;
using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DU_CertificateApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly IMapper _iMapper;
        private readonly IRoleManager _iRoleManager;

        public RoleController(IMapper iMapper, IRoleManager iRoleManager)
        {
            _iMapper = iMapper;
            _iRoleManager = iRoleManager;
        }

        public async Task<ActionResult<IEnumerable<RoleViewModel>>> Index()
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            IEnumerable<RoleViewModel> roles = _iMapper.Map<IEnumerable<RoleViewModel>>(await _iRoleManager.GetAll());

            return View(roles);
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
        public async Task<IActionResult> Create(RoleViewModel role)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (ModelState.IsValid)
            {
                Role AddRole = _iMapper.Map<Role>(role);
                bool IsAdded = await _iRoleManager.Create(AddRole);

                if (IsAdded)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Create Role";
            }
            return View(role);
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

            RoleViewModel existRole = _iMapper.Map<RoleViewModel>(await _iRoleManager.GetById(id));

            if (existRole == null)
                return NotFound();

            return View(existRole);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleViewModel ExistRole)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (ModelState.IsValid)
            {
                Role role = _iMapper.Map<Role>(ExistRole);

                bool IsUpdated = await _iRoleManager.Update(role);

                if (IsUpdated)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Update Role";
            }
            return View(ExistRole);
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

            Role existRole = await _iRoleManager.GetById(id);

            if (existRole == null)
                return NotFound();

            bool remove = await _iRoleManager.Remove(existRole);

            if (remove)
                return RedirectToAction("Index");
            else
                ViewBag.ErrorMessage = "Failed to Delete Role";

            return BadRequest();
        }
    }
}
