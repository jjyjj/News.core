using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ICaptcha _captcha;

        public UserController(IUserService userService, ICaptcha captcha)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _captcha = captcha ?? throw new ArgumentNullException(nameof(captcha));
        }

        #region 获取所有用户信息
        [HttpGet]
        public async Task<MessageModel> GetAll()
        {

            var data = await _userService.GetAll();
            return new MessageModel()
            {
                Code = 200,
                Msg = "获取成功",
                Data = data
            };

        }

        #endregion


        #region 根据Id获取用户信息

        [HttpGet]
        public async Task<MessageModel> GetOneById(int userId)
        {
            var data = new MessageModel();
            try
            {

                var users = await _userService.GetOneById(userId);
                if (users == null) data.Msg = "查无此人";

                else
                {
                    data.Code = 200;
                    data.Msg = "获取成功";
                    data.Data = users;
                }

            }
            catch (Exception)
            {

                data.Msg = "查无此人";
            }

            return data;

        }
        #endregion


        #region 增加用户
        [HttpPost]
        public async Task<MessageModel> Add([FromBody] UserViewModel userViewModel)
        {
            MessageModel data = new MessageModel();
            if (Validate(userViewModel.Code))
            {
                Users users = new Users
                {
                    Email = userViewModel.Email,
                    PassWord = userViewModel.PassWord,
                    State = userViewModel.State,
                    CreateTime = userViewModel.CreateTime,
                    IsRemove = userViewModel.IsRemove
                };


                var result = await _userService.Create(users);
                if (result > 0)
                {
                    data.Code = 200;
                    data.Msg = "添加成功";
                }
                else data.Msg = "添加失败";


            }
            else
            {
                data.Msg = "验证码不正确，请重新输入";
            }
            return data;

        }
        #endregion


        #region 删除用户
        [HttpDelete]
        public async Task<MessageModel> Del(int id)
        {
            MessageModel data = new MessageModel();
            if (id < 0) data.Msg = "id不可为空";
            var result = await _userService.Delete(new Users()
            {
                Id = id
            });
            if (result)
            {
                data.Code = 200;
                data.Msg = "删除成功";
            }
            else data.Msg = "删除失败";




            return data;
        }

        #endregion

        #region 修改用户信息
        [Authorize]
        [HttpPut]
        public async Task<MessageModel> Update([FromBody]  Users model)
        {
            MessageModel data = new MessageModel();

            if (model != null && model.Id > 0)
            {
                var oldUser = await _userService.GetOneById(model.Id);
                if (oldUser != null)
                {
                    oldUser.Email = model.Email;
                    oldUser.PassWord = model.PassWord;
                    oldUser.ImgUrl = model.ImgUrl;
                    oldUser.Phone = model.Phone;
                    oldUser.Sex = model.Sex;
                    oldUser.Adress = model.Adress;
                    oldUser.Birthday = model.Birthday;
                    oldUser.Name = model.Name;
                    oldUser.Level = model.Level;
                    var result = await _userService.Update(oldUser);
                    if (result)
                    {
                        data.Code = 200;
                        data.Msg = "修改成功";
                    }
                    else data.Msg = "修改失败";
                }
            }
            return data;
        }
        #endregion

        #region 生成验证码图片
        [HttpGet]
        public IActionResult ValidateCode()
        {

            var info = _captcha.Generate("12");
            var stream = new MemoryStream(info.CaptchaByteData);
            return File(stream, "image/png");
        }
        #endregion

        #region 验证
        [HttpGet]
        public bool Validate(string code)
        {
            var result = _captcha.Validate("12", code);

            return result;
        }
        #endregion
    }
}