using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Model.Models
{
    public class Department
    {
        public Department()
        {
            this.Name = string.Empty;
            this.Status = true;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "Must Insert Department Name")]
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
