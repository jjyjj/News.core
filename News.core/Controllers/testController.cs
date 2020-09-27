using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using News.core.AuthHelp.Models;
using News.core.IServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Controllers
{


    public class testController : BaseController
    {
        //获取JwtSettings对象信息
        private JwtSettings _jwtSettings;
        private readonly IUserService _userService;
        private readonly INewsService _newsService;

        public testController(IOptions<JwtSettings> _jwtSettingsAccesser, IUserService userService, INewsService newsService)
        {
            if (_jwtSettingsAccesser is null)
            {
                throw new ArgumentNullException(nameof(_jwtSettingsAccesser));
            }

            _jwtSettings = _jwtSettingsAccesser.Value;
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
        }

      

        [Authorize]
        [HttpPost]
        public IActionResult GetUserInfo()
        {
            //获取当前请求用户的信息，包含token信息
            var user = HttpContext.User;
            return Ok("12");
        }

        [HttpGet]
        public async Task<object> Query()
        {
            return await _userService.Query();

        }
        [HttpGet]
        public async Task<object> QueryPage(int pageIndex, int pageSize, string strWhere, bool isOrder)
        {

            var data = await _newsService.Pagination<Model.Entities.News, object>(pageIndex, pageSize, m => m.CreateTime, u => u.UserId == 1, false);
            return data;

        }

    }
}
