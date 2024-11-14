using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DU_CertificateApp.Models
{
    public class OrderCertificateViewModel
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int CertificateID { get; set; }
    }
}
