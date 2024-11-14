using AutoMapper;
using DU_CertificateApp.Database.Database;
using DU_CertificateApp.Manager.Contract;
using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DU_CertificateApp.Controllers
{
    public class UserController : Controller
    {
        private readonly DU_CertificateDB _db;
        private readonly IMapper _iMapper;
        private readonly IStudentManager _iStudentManager;
        private readonly IUserManager _iUserManager;
        private readonly IRoleManager _iRoleManager;
        public UserController(IMapper iMapper, IStudentManager iStudentManager, IUserManager iUserManager, IRoleManager iRoleManager)
        {
            _db = new DU_CertificateDB();
            _iMapper = iMapper;
            _iStudentManager = iStudentManager;
            _iUserManager = iUserManager;
            _iRoleManager = iRoleManager;
        }

        public async Task<ActionResult<IEnumerable<UserViewModel>>> Index()
        {
            return View();
        }

        public async Task<ActionResult<IEnumerable<UserViewModel>>> UserList()
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            IEnumerable<UserViewModel> users = _iMapper.Map<IEnumerable<UserViewModel>>(await _iUserManager.GetAll());

            ViewBag.Roles = _iMapper.Map<IEnumerable<RoleViewModel>>(await _iRoleManager.GetAll());
            return View(users);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel user)
        {
            try
            {
                User? loginUser = _db.Users.Where(e=>e.UserName == user.UserName).FirstOrDefault();

                if (loginUser == null)
                {
                    //Check Student ID
                    Student? loginStudent = _db.Students.Where(s=>s.Email == user.UserName).FirstOrDefault();

                    if(loginStudent == null)
                    {
                        ViewBag.ErrorMessage = "UserName or Passeword is Incorrect!";
                    }
                    else
                    {
                        if(loginStudent.Password == user.Password)
                        {
                            // Student Login Success
                            HttpContext.Session.SetString("ID", loginStudent.ID.ToString());
                            HttpContext.Session.SetString("Name", loginStudent.Name.ToString());
                            HttpContext.Session.SetString("Membership", "Student");

                            return RedirectToAction("Index", "Student");
                        }
                        else
                            ViewBag.ErrorMessage = "UserName or Passeword is Incorrect!";
                    }                   
                }
                else
                {
                    if(loginUser.Password == user.Password)
                    {
                        //User Login Success
                        HttpContext.Session.SetString("ID", loginUser.ID.ToString());
                        HttpContext.Session.SetString("Name", loginUser.Name.ToString());

                        RoleViewModel role = _iMapper.Map<RoleViewModel>(await _iRoleManager.GetById(loginUser.RoleID));
                        HttpContext.Session.SetString("Membership", role.Name.ToString());

                        return RedirectToAction("Index");
                    }
                    else
                        ViewBag.ErrorMessage = "UserName or Passeword is Incorrect!";
                }
                return View();
            }
            catch (Exception ex)
            {
                return ViewBag.ErrorMessage = "Failed to Login. Error: " + ex.Message;
            }
        }

        #region SignUp
        [HttpGet]
        public IActionResult Signup()
        {
            #region Login Check
            if (HttpContext.Session.GetString("UserID") == "" || HttpContext.Session.GetString("UserID") == null)
            {
                IEnumerable<Department> dept = _db.Departments.Where(d=>d.Status == true).ToList();
                ViewBag.Departments = _iMapper.Map<IEnumerable<DepartmentViewModel>>(dept);

                return View();
            }              
            #endregion
            return RedirectToAction("Index", "Student");
        }

        [HttpPost]
        public async Task<IActionResult> SignupAsync(StudentViewModel student)
        {
            try
            {
                IEnumerable<Department> dept = _db.Departments.Where(d=>d.Status == true).ToList();
                ViewBag.Departments = _iMapper.Map<IEnumerable<DepartmentViewModel>>(dept);

                #region Validation Check
                bool notValid = false;

                if(student.Batch == 0)
                {
                    ViewBag.Batch = "Please insert a valid batch number!";
                    notValid = true;
                }

                if (student.DepartmentID == 0)
                {
                    ViewBag.Department = "Please Select your Department!";
                    notValid = true;
                }

                if (student.ExamRoll == 0)
                {
                    ViewBag.ExamRoll = "Please insert a valid Exam-Roll!";
                    notValid = true;
                }

                if (student.Registration == 0)
                {
                    ViewBag.Registration = "Please insert a valid Registration number!";
                    notValid = true;
                }
                if (student.Phone.Any(char.IsLetter))
                {
                    ViewBag.Phone = "Please insert a valid Phone number!";
                    notValid = true;
                }

                bool registrationExists = _db.Students.Any(u=>u.Registration == student.Registration);
                if (registrationExists)
                {
                    ViewBag.Registration = "Registration Number already exists!";
                    notValid = true;
                }
                bool examRollExists = _db.Students.Any(e=>e.ExamRoll == student.ExamRoll);
                if (examRollExists)
                {
                    ViewBag.ExamRoll = "Exam-Roll already exists!";
                    notValid = true;
                }
                bool emailExists = _db.Students.Any(e=>e.Email == student.Email);
                if (emailExists)
                {
                    ViewBag.Email = "Email already exists!";
                    notValid = true;
                }
                bool phoneExists = _db.Students.Any(e=>e.Phone == student.Phone);
                if (phoneExists)
                {
                    ViewBag.Phone = "Phone Number already exists!";
                    notValid = true;
                }
                if (student.Password != student.ConfirmPassword)
                {
                    ViewBag.PasswordMismatched = "Password is not matching!";
                    notValid = true;
                }
                if (notValid)
                {
                    return View(student);
                }
                #endregion
                if(ModelState.IsValid)
                {
                    student.Status = true;

                    student.Password = EDPassword(student.Password, true);
                    Student newStudent = _iMapper.Map<Student>(student);
                    bool isAdded = await _iStudentManager.Create(newStudent);
                    if (isAdded)
                    {

                        HttpContext.Session.SetString("ID", student.ID.ToString());
                        HttpContext.Session.SetString("Name", student.Name.ToString());
                        HttpContext.Session.SetString("Membership", "Student");

                        return RedirectToAction("Index", "Student");
                    }
                    else
                        ViewBag.ErrorMessage = "Failed to Sign Up";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Operation Failed. \nReason: " + ex.Message;
            }          
            return View();
        }
        #endregion

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("ID", "");
            HttpContext.Session.SetString("Name", "");
            HttpContext.Session.SetString("Membership", "");

            return RedirectToAction("Login", "User");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            ViewBag.Roles = _iMapper.Map<IEnumerable<RoleViewModel>>(await _iRoleManager.GetAll());
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel user)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (ModelState.IsValid)
            {
                User AddUser = _iMapper.Map<User>(user);
                bool IsAdded = await _iUserManager.Create(AddUser);

                if (IsAdded)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Create User";
            }
            ViewBag.Roles = _iMapper.Map<IEnumerable<RoleViewModel>>(await _iRoleManager.GetAll());
            return View(user);
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

            UserViewModel existUser = _iMapper.Map<UserViewModel>(await _iUserManager.GetById(id));

            if (existUser == null)
                return NotFound();

            ViewBag.Roles = _iMapper.Map<IEnumerable<RoleViewModel>>(await _iRoleManager.GetAll());
            return View(existUser);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserViewModel ExistUser)
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You don not have the permission to access this resource");
            #endregion

            if (ModelState.IsValid)
            {
                User user = _iMapper.Map<User>(ExistUser);

                bool IsUpdated = await _iUserManager.Update(user);

                if (IsUpdated)
                    return RedirectToAction("Index");
                else
                    ViewBag.ErrorMessage = "Failed to Update User";
            }
            ViewBag.Roles = _iMapper.Map<IEnumerable<RoleViewModel>>(await _iRoleManager.GetAll());
            return View(ExistUser);
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

            User existUser = await _iUserManager.GetById(id);

            if (existUser == null)
                return NotFound();

            bool remove = await _iUserManager.Remove(existUser);

            if (remove)
                return RedirectToAction("Index");
            else
                ViewBag.ErrorMessage = "Failed to Delete User";

            return BadRequest();
        }

        #region Encryption and Decryption
        private string EDPassword(string pass, bool en)
        {
            string password = "";
            try
            {

                if (pass != null)
                {
                    if (en)
                    {
                        #region Encrypt the password
                        password = pass;

                        #endregion
                    }
                    else
                    {
                        #region Decrypt the password
                        password = pass;

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return password;
        }
        #endregion

        #region Session
        private IActionResult GetSetSession()
        {
            #region Admin Check
            if (HttpContext.Session.GetString("Membership") != "Admin")
                return StatusCode(403, "Access Denied: You do not have the permission to access this resource");
            #endregion

            #region User
            HttpContext.Session.SetString("ID", "");
            HttpContext.Session.SetString("Name", "");
            HttpContext.Session.SetString("Membership", "");
            #endregion

            return RedirectToAction("Index");
        }
        #endregion
    }
}
