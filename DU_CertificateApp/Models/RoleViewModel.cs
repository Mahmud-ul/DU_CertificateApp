using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Models
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            this.Name = string.Empty;
            this.Status = true;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "Must Insert Role Title")]
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
