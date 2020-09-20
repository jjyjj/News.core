using News.core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IServices
{
    public interface IFocusService : IBaseService<Model.Entities.Focus>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">查询人Id</param>
        /// <param name="queryModel">查询参数</param>
        /// <param name="type">true是查询我的粉丝,false查询谁关注了我</param>
        /// <returns></returns>
        Task<PageModel<Model.ViewModel.FocusViewModel>> GetAllById(int userId, QueryModel queryModel, bool type);
    }
}
