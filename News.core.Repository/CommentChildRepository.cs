using News.core.IRepository;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Repository
{

    public class CommentChildRepository : BaseRepository<Model.Entities.CommentsChild>, ICommentChildRepository
    {
        public CommentChildRepository(NewsDbContext db) : base(db)
        {
        }
    }
}
