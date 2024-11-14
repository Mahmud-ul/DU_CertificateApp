using DU_CertificateApp.Model.Models;
using DU_CertificateApp.Repository.Base;
using DU_CertificateApp.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU_CertificateApp.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
    }
}
