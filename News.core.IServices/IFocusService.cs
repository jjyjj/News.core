using News.core.Model;
using News.core.Model.ViewModel;
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
        Task<PageModel<FocusViewModel>> GetAllByUserId(int userId, QueryModel queryModel, bool type);


        #region 
        Task<int> Add(int userId, int focusId);
        Task<bool> Del(int focusId);
        #endregion
    }
}
