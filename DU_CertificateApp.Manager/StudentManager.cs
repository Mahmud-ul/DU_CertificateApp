using DU_CertificateApp.Manager.Base;
using DU_CertificateApp.Manager.Contract;
using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Manager
{
    public class StudentManager : BaseManager<Student>, IStudentManager
    {
        private readonly IStudentRepository _StudentRepository;
        public StudentManager(IStudentRepository StudentRepository) : base(StudentRepository)
        {
            _StudentRepository = StudentRepository;
        }
    }
}
