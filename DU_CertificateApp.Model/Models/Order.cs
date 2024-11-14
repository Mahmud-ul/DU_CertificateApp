using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Model.Models
{
    public class Order
    {
        public int ID { get; set; }
        public int Price { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }
        public string Status { get; set; }

        [ForeignKey("AcceptedUser")]
        public int? AcceptedID { get; set; }

        [ForeignKey("ApprovedUser")]
        public int? ApprovedID { get; set; }

        public DateTime OrderedDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public virtual Student Student { get; set; }
        public virtual User AcceptedUser { get; set; }
        public virtual User ApprovedUser { get; set; }
    }
}
