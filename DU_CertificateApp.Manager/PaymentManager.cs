using DU_CertificateApp.Manager.Base;
using DU_CertificateApp.Manager.Contract;
using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Manager
{
    public class PaymentManager : BaseManager<Payment>, IPaymentManager
    {
        private readonly IPaymentRepository _PaymentRepository;
        public PaymentManager(IPaymentRepository PaymentRepository) : base(PaymentRepository)
        {
            _PaymentRepository = PaymentRepository;
        }
    }
}
