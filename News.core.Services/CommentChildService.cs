using News.core.IRepository;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<int> Add(CommentsChild commentsChild)
        {

            if (commentsChild.UserId == null || commentsChild.NewsId == null || commentsChild.Content == null) return -1;
            else
            {
                return await _commentChildRepository.Create(commentsChild);
            }
        }

        public async Task<bool> Del(int commentChildId)
        {
            var commentChild = await _commentChildRepository.GetOneByStr(m => m.Id == commentChildId);
            return await _commentChildRepository.Delete(commentChild);
        }
    }
}
