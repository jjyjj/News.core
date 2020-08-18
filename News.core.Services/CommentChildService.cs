using News.core.IRepository;
using News.core.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Services
{

    public class CommentChildService : BaseService<Model.Entities.CommentsChild>, ICommentChildService
    {

        private readonly ICommentChildRepository _commentChildRepository;

        public CommentChildService(ICommentChildRepository commentChildRepository)
        {

            _commentChildRepository = commentChildRepository ?? throw new ArgumentNullException(nameof(commentChildRepository));

            base.BaseDal = commentChildRepository;

        }

    }
}
