using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Model.Models
{
    public class OrderCertificate
    {
        [ForeignKey("Order")]
        public int OrderID { get; set; }

        [ForeignKey("Certificate")]
        public int CertificateID { get; set; }

        public virtual Order Order { get; set; }
        public virtual Certificate Certificate { get; set; }
    }
}
