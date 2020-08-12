using News.core.IRepository;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Repository
{
    public class UserRepository : BaseRepository<Model.Entities.Users>, IUserRepository
    {
        public UserRepository(NewsDbContext db) : base(db)
        {
        }
    }
}
