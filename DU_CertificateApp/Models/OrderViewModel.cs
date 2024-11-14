using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Models
{
    public class OrderViewModel
    {
        public int ID { get; set; }
        public int Price { get; set; }
        public int StudentID { get; set; }
        public string Status { get; set; }
        public int? AcceptedID { get; set; }
        public int? ApprovedID { get; set; }
        public DateTime OrderedDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }

    }
}
