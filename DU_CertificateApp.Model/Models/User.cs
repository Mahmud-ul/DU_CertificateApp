using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Model.Models
{
    public class User
    {
        public User()
        {
            this.UserName = string.Empty;
            this.Email = string.Empty;
            this.Phone = string.Empty;
            this.Status = true;

        }
        public int ID { get; set; }

        [Required(ErrorMessage = "Must Insert Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Must Insert User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Must Insert Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Must Insert Phone Number")]
        public string Phone { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }
        public bool Status { get; set; }

        public virtual Role Role { get; set; }
    }
}
