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
        private readonly IUserService _userService;

        public FocusController(IFocusService focusService, IUserService userService)
        {
            _focusService = focusService ?? throw new ArgumentNullException(nameof(focusService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        #region 添加粉丝
        [HttpPost]
        public async Task<MessageModel> Add(int userId, int focusId)
        {
            MessageModel messageModel = new MessageModel();
            if (userId <= 0 || focusId <= 0) messageModel.Msg = "关注失败";
            else
            {
                var isExit = await _focusService.GetOneByStr(m => m.UserId == userId && m.FocusId == focusId);
                if (isExit != null)
                {
                    messageModel.Msg = "请勿重复关注";
                }
                else
                {
                    var isCreate = await _focusService.Create(
                    new Focus()
                    {
                        UserId = userId,
                        FocusId = focusId
                    }
                    ) > 0;
                    messageModel.Msg = isCreate == true ? "关注成功" : "关注失败";
                }

            }
            messageModel.Code = 200;

            return messageModel;
        }
        #endregion
        #region 查询粉丝/被关注 列表
        [HttpGet]
        public async Task<MessageModel> GetAllById(int userId, QueryModel queryModel, bool type)
        {
            MessageModel messageModel = new MessageModel();
            if (userId <= 0) messageModel.Msg = "用户不存在，无法进行查询";
            else
            {
                var focusList = await _focusService.GetAllById(userId, queryModel, type);
                messageModel.Data = focusList;
            }
            messageModel.Code = 200;
            return messageModel;
        }
        #endregion

        #region 删除关注粉丝
        [HttpDelete]
        public async Task<MessageModel> Del(int focusId)
        {
            MessageModel messageModel = new MessageModel();
            if (focusId > 0)
            {
                var focusData = await _focusService.GetOneById(focusId);
                if (focusData == null) messageModel.Msg = "不存在该记录";
                else messageModel.Msg = await _focusService.Delete(focusData) == true ? "删除成功" : "删除失败";

            }
            messageModel.Code = 200;
            return messageModel;
        }
        #endregion
    }
}
