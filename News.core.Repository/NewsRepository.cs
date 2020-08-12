using News.core.IRepository;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Repository
{

    public class NewsRepository : BaseRepository<Model.Entities.News>, INewsRepository
    {
        public NewsRepository(NewsDbContext db) : base(db)
        {
        }
    }
}
