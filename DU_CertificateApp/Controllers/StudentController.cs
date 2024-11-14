using AutoMapper;
using DU_CertificateApp.Database.Database;
using DU_CertificateApp.Manager;
using DU_CertificateApp.Manager.Contract;
using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DU_CertificateApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly IMapper _iMapper;
        private readonly IStudentManager _iStudentManager;
        private readonly IDepartmentManager _iDepartmentManager;
        public StudentController(IMapper iMapper, IStudentManager iStudentManager, IDepartmentManager iDepartmentManager)
        {
            _iMapper = iMapper;
            _iStudentManager = iStudentManager;
            _iDepartmentManager = iDepartmentManager;
        }

        public async Task<ActionResult<IEnumerable<StudentViewModel>>> Index()
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            return View();
        }
        public async Task<ActionResult<IEnumerable<StudentViewModel>>> StudentList()
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            IEnumerable<StudentViewModel> students = _iMapper.Map<IEnumerable<StudentViewModel>>(await _iStudentManager.GetAll());

            ViewBag.Departments = _iMapper.Map<IEnumerable<DepartmentViewModel>>(await _iDepartmentManager.GetAll());
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
            {
                ViewBag.Departments = _iMapper.Map<IEnumerable<DepartmentViewModel>>(await _iDepartmentManager.GetAll());
                return View();

            }
            return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                Student AddStudent = _iMapper.Map<Student>(student);
                bool IsAdded = await _iStudentManager.Create(AddStudent);

                if (IsAdded)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Create Student";
            }
            ViewBag.Departments = _iMapper.Map<IEnumerable<DepartmentViewModel>>(await _iDepartmentManager.GetAll());
            return View(student);
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

            StudentViewModel existStudent = _iMapper.Map<StudentViewModel>(await _iStudentManager.GetById(id));

            if (existStudent == null)
                return NotFound();

            ViewBag.Departments = _iMapper.Map<IEnumerable<DepartmentViewModel>>(await _iDepartmentManager.GetAll());
            return View(existStudent);
        }

        [HttpPost]
        public async Task<IActionResult> Update(StudentViewModel ExistStudent)
        {
            #region Login Check
            if (HttpContext.Session.GetString("ID") == "" || HttpContext.Session.GetString("ID") == null)
                return RedirectToAction("Login", "User");
            #endregion

            if (ModelState.IsValid)
            {
                Student student = _iMapper.Map<Student>(ExistStudent);

                bool IsUpdated = await _iStudentManager.Update(student);

                if (IsUpdated)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Update Student";
            }
            ViewBag.Departments = _iMapper.Map<IEnumerable<DepartmentViewModel>>(await _iDepartmentManager.GetAll());
            return View(ExistStudent);
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

            Student existStudent = await _iStudentManager.GetById(id);

            if (existStudent == null)
                return NotFound();

            bool remove = await _iStudentManager.Remove(existStudent);

            if (remove)
                return RedirectToAction("Index");
            else
                ViewBag.ErrorMessage = "Failed to Delete Student";

            return BadRequest();
        }
    }
}
