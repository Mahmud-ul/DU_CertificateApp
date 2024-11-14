using AutoMapper;
using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Models;

namespace DU_CertificateApp.Utility
{
    public class AutoMapper : Profile
    {
        public AutoMapper() 
        {
            CreateMap<Certificate, CertificateViewModel>();
            CreateMap<CertificateViewModel, Certificate>();

            CreateMap<OrderCertificate, OrderCertificateViewModel>();
            CreateMap<OrderCertificateViewModel, OrderCertificate>();

            CreateMap<Cart, CartViewModel>();
            CreateMap<CartViewModel, Cart>();

            CreateMap<Department, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, Department>();

            CreateMap<Payment, PaymentViewModel>();
            CreateMap<PaymentViewModel,Payment>();

            CreateMap<PaymentMethod, PaymentMethodViewModel>();
            CreateMap<PaymentMethodViewModel,PaymentMethod>();

            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderViewModel, Order>();

            CreateMap<Role, RoleViewModel>();
            CreateMap<RoleViewModel, Role>();

            CreateMap<Student, StudentViewModel>();
            CreateMap<StudentViewModel, Student>();

            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
        }
    }
}
