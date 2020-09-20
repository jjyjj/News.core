using News.core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IServices
{

    public interface INewsService : IBaseService<Model.Entities.News>
    {
        Task<Model.ViewModel.NewsDetailsModel> GetDetailsById(int newsId);
        Task<PageModel<Model.Entities.News>> GetAllByCategory(string categoryName, QueryModel queryModel);

        Task<PageModel<Model.ViewModel.NewsDetailsModel>> GetAllByUserId(int userId, QueryModel queryModel,int state);
        Task<PageModel<Model.ViewModel.HotCommentNewsViewModel>> HotCommentNews(QueryModel queryModel);
        Task<bool> delNew(int newsId);
    }
}
