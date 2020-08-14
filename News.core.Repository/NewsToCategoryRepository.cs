using Microsoft.EntityFrameworkCore;
using News.core.IRepository;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Repository
{

    public class NewsToCategoryRepository : BaseRepository<Model.Entities.NewsToCategory>, INewsToCategoryRepository
    {
        NewsDbContext _db;
        DbSet<NewsToCategory> _dbSet;
        public NewsToCategoryRepository(NewsDbContext db) : base(db)
        {
           
       
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _dbSet = db.Set<NewsToCategory>();
        }

     
    }
}
