using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Model.Models
{
    public class Student
    {
        public Student()
        {
            this.Name = string.Empty;
            this.Email = string.Empty;
            this.Phone = string.Empty;
            this.Status = true;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "Must Insert Student Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Must Insert Registration Number")]
        public int Registration { get; set; }

        [Required(ErrorMessage = "Must Insert Exam Roll")]
        public int ExamRoll { get; set; }

        [Required(ErrorMessage = "Must Insert Batch Number")]
        public int Batch { get; set; }
        public bool Status { get; set; }

        [Required(ErrorMessage = "Must Insert Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Must Insert Phone Number")]
        public string Phone { get; set; }
        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }
    }
}