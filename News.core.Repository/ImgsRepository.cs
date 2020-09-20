using News.core.IRepository;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Repository
{

    public class ImgsRepository : BaseRepository<Model.Entities.Imgs>, IImgsRepository
    {
        public ImgsRepository(NewsDbContext db) : base(db)
        {
        }
    }
}
