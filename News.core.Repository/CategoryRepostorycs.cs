using News.core.IRepository;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Repository
{

    public class CategoryRepostorycs : BaseRepository<Model.Entities.Category>, ICategoryRepostorycs
    {
        public CategoryRepostorycs(NewsDbContext db) : base(db)
        {
        }
    }
}
