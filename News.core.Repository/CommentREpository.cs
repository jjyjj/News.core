using News.core.IRepository;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Repository
{
    public class CommentRepository : BaseRepository<Model.Entities.Comments>, ICommentRepostory
    {
        public CommentRepository(NewsDbContext db) : base(db)
        {
        }
    }
}
