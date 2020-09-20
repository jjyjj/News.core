using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IServices
{

    public interface ICommentChildService : IBaseService<Model.Entities.CommentsChild>
    {
        Task<bool> Del(int commentChildId);
        Task<int> Add(CommentsChild commentsChild);
    }
}
