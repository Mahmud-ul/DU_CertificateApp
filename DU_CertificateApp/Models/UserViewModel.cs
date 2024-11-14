using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Models
{
    public class UserViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Must Insert Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Must Insert User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Must Insert Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Must Insert Phone Number")]
        public string Phone { get; set; }

        public int RoleID { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
        public bool Status { get; set; }
    }
}
