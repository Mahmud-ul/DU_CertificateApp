using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Models
{
    public class PaymentViewModel
    {
        public int ID { get; set; }
        public int Amount { get; set; }
        public int StudentID { get; set; }
        public int PaymentMethodID { get; set; }
        public int TransectionID { get; set; }

        [StringLength(50)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
