using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Model.Models
{
    public class Certificate
    {
        public Certificate() 
        {
            this.Name = string.Empty;
            this.Status = true;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "Must Insert Certificate Title")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Must Insert Certificate Price")]
        public int Price { get; set; }
        public bool Status { get; set; }
    }
}
