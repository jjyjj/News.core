using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using News.core.Common;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using News.core.Model.ViewModel;
using SimpleCaptcha;

namespace News.core.Controllers
{
    public class FocusController : BaseController
    {
        private readonly IFocusService _focusService;

        public FocusController(IFocusService focusService)
        {
            _focusService = focusService ?? throw new ArgumentNullException(nameof(focusService));
        }
        #region 添加粉丝
        [HttpPost]
        public async Task<MessageModel> Add(int userId, int focusId)
        {
            var id = await _focusService.Add(userId, focusId);
            return new MessageModel()
            {
                Code = 200,
                Data = id,
                Msg = id > 0 ? "创建成功" : "创建失败"
            };
        }
        #endregion

        #region 查询当前用户的粉丝列表或者关注人列表
        [HttpGet]
        public async Task<MessageModel> GetAllById(int userId, QueryModel queryModel, bool type)
        {
            var list = await _focusService.GetAllByUserId(userId, queryModel, type);
            return new MessageModel()
            {
                Code = 200,
                Data = list,
                Msg = list == null ? "获取失败" : "获取成功"
            };
        }
        #endregion

        #region 删除关注粉丝
        [HttpDelete]
        public async Task<MessageModel> Del(int focusId)
        {
            var isDel = await _focusService.Del(focusId);
            return new MessageModel()
            {
                Code = 200,
                Data = isDel,
                Msg = isDel ? "删除成功" : "删除失败"
            };
        }
        #endregion
    }
}
