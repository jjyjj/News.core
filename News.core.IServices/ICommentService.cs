using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IServices
{

    public interface ICommentService : IBaseService<Model.Entities.Comments>
    {
        Task<PageModel<Model.ViewModel.CommentViewModel>> GetAll(int newsId, QueryModel queryModel);
        Task<bool> Del(int commentId);
        Task<PageModel<Model.ViewModel.CommentViewModel>> GetAllByUserId(int userId, QueryModel queryModel);



        #region 
        Task<int> Add(Comments comments);
        #endregion

    }
}
