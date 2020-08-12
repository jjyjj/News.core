using News.core.IRepository;
using News.core.IServices;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Services
{
    public class UserService : BaseService<Users>, IUserService
    {
        IUserRepository _usersRepositorys;


        public UserService(IUserRepository usersRepositorys)
        {
            _usersRepositorys = usersRepositorys ?? throw new ArgumentNullException(nameof(usersRepositorys));
            base.BaseDal = usersRepositorys;
        }

    }
}
