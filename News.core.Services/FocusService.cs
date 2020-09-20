using News.core.IRepository;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using News.core.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Services
{
    public class FocusService : BaseService<Model.Entities.Focus>, IFocusService
    {
        private readonly IFocusRepository _focusRepository;
        private readonly IUserRepository _userRepository;

        public FocusService(IFocusRepository focusRepository, IUserRepository userRepository)
        {

            _focusRepository = focusRepository ?? throw new ArgumentNullException(nameof(focusRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            base.BaseDal = focusRepository;
        }

        public async Task<int> Add(int userId, int focusId)
        {
            if (userId <= 0 || focusId <= 0) return -1;
            else
            {
                var focu = await _focusRepository.GetOneByStr(m => m.UserId == userId && m.FocusId == focusId);
                if (focu != null) return -1;
                return await _focusRepository.Create(
                  new Focus()
                  {
                      UserId = userId,
                      FocusId = focusId
                  }
                  );
            }
        }

        public async Task<bool> Del(int focusId)
        {
            var model = await _focusRepository.GetOneByStr(m => m.Id == focusId);

            return await _focusRepository.Delete(model);
        }

        public async Task<PageModel<Model.ViewModel.FocusViewModel>> GetAllByUserId(int userId, QueryModel queryModel, bool type)
        {
            List<Model.Entities.Focus> focusIdList = new List<Focus>();
            List<Model.ViewModel.FocusViewModel> focusViewModels = new List<Model.ViewModel.FocusViewModel>();
            #region 查询我的粉丝
            if (type)
            {
                focusIdList = await _focusRepository.Query(m => m.UserId == userId);
                foreach (var item in focusIdList)
                {
                    FocusViewModel focusViewModel = new FocusViewModel();
                    var data = await _userRepository.GetOneById(item.FocusId);
                    focusViewModel.Focus = item;
                    focusViewModel.Users = data;
                    focusViewModels.Add(focusViewModel);
                }
            }
            #endregion

            #region 我被谁关注了
            else
            {
                focusIdList = await _focusRepository.Query(m => m.FocusId == userId);
                foreach (var item in focusIdList)
                {
                    FocusViewModel focusViewModel = new FocusViewModel();
                    var data = await _userRepository.GetOneById(item.UserId);
                    focusViewModel.Focus = item;
                    focusViewModel.Users = data;
                    focusViewModels.Add(focusViewModel);
                }
            }
            #endregion

            #region 分页
            PageModel<Model.ViewModel.FocusViewModel> pageModel = new PageModel<FocusViewModel>();
            pageModel.dataCount = focusIdList.Count;
            pageModel.pageCount = (int)Math.Ceiling(pageModel.dataCount / (double)queryModel.pageSize);
            pageModel.pageSize = queryModel.pageSize;
            pageModel.pageIndex = queryModel.pageIndex;
            var ss = focusViewModels.AsQueryable();
            pageModel.data = ss
                .Skip((pageModel.pageIndex - 1) * pageModel.pageSize)
                .Take(pageModel.pageSize)
                .ToList() ?? null;
            #endregion

            return pageModel;
        }
    }
}
