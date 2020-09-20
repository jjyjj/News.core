using News.core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IServices
{

    public interface ICommentService : IBaseService<Model.Entities.Comments>
    {
        Task<PageModel<Model.ViewModel.CommentViewModel>> GetAll(int newsId, QueryModel queryModel);
        Task<bool> DelAllComment(int commentId);
        Task<PageModel<Model.ViewModel.CommentViewModel>> GetAllByUserId(int userId, QueryModel queryModel);
    }
}
