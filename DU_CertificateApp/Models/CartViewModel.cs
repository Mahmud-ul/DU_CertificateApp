using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DU_CertificateApp.Models
{
    public class CartViewModel
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int CertificateID { get; set; }
    }
}
