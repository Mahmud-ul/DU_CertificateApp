using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Models
{
    public class CertificateViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Must Insert Certificate Title")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Must Insert Certificate Price")]
        public int Price { get; set; }
        public bool Status { get; set; }
    }
}
