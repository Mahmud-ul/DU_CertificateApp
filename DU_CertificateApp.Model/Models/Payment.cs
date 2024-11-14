using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Model.Models
{
    public class Payment
    {
        public Payment()
        {
            this.Description = string.Empty;
            this.Status = string.Empty;
        }
        public int ID { get; set; }
        public int Amount { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }

        [ForeignKey("PaymentMethod")]
        public int PaymentMethodID { get; set; }
        public int TransectionID { get; set; }

        [StringLength(50)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual Student Student { get; set; }
    }
}
