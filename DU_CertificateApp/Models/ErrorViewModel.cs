namespace DU_CertificateApp.Models
{
    public class ErrorViewModel
    {
        public string? OrderId { get; set; }

        public bool ShowOrderId=>!string.IsNullOrEmpty(OrderId);
    }
}
