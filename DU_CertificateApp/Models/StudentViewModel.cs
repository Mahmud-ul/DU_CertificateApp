using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Models
{
    public class StudentViewModel
    {
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

        public string ConfirmPassword { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
    }
}