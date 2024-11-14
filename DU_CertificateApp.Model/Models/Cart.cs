using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Model.Models
{
    public class Cart
    {
        public int ID { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }

        [ForeignKey("Certificate")]
        public int CertificateID { get; set; }

        public virtual Student Student { get; set; }
        public virtual Certificate Certificate { get; set; }
    }
}
