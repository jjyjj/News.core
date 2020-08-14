using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IServices
{

    public interface INewsService : IBaseService<Model.Entities.News>
    {
        Task<Model.ViewModel.NewsDetailsModel> GetDetailsById(int newsId);
    }
}
