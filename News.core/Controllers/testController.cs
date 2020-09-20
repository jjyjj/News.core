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

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="user"></param>
        private object Token(test model)
        {
            //测试自己创建的对象
            var user = new test
            {
                id = 1,
                phone = "138000000",
                password = "e10adc3949ba59abbe56e057f20f883e"
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var authTime = DateTime.Now;//授权时间
            var expiresAt = authTime.AddDays(30);//过期时间
            var tokenDescripor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(JwtClaimTypes.Audience,_jwtSettings.Audience),
                    new Claim(JwtClaimTypes.Issuer,_jwtSettings.Issuer),
                    new Claim(JwtClaimTypes.Name, user.phone.ToString()),
                    new Claim(JwtClaimTypes.Id, user.id.ToString()),
                    new Claim(JwtClaimTypes.PhoneNumber, user.phone.ToString())
                }),
                Expires = expiresAt,
                //对称秘钥SymmetricSecurityKey
                //签名证书(秘钥，加密算法)SecurityAlgorithms
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescripor);
            var tokenString = tokenHandler.WriteToken(token);
            var result = new
            {
                access_token = tokenString,
                token_type = "Bearer",
                profile = new
                {
                    id = user.id,
                    name = user.phone,
                    phone = user.phone,
                    auth_time = authTime,
                    expires_at = expiresAt
                }
            };
            return result;
        }


        [HttpPost]
        public IActionResult GetToken()
        {
            return Ok(Token(null));
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
