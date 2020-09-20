using News.core.IRepository;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Repository
{
    public class FocusRepository : BaseRepository<Model.Entities.Focus>, IFocusRepository
    {


        public FocusRepository(NewsDbContext db) : base(db)
        {

        }
    }
}
