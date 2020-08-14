using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using News.core.AuthHelp.Models;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Controllers
{
    public class LoginController : BaseController
    {
        private JwtSettings _jwtSettings;
        private readonly IUserService _userService;

        public LoginController(IOptions<JwtSettings> _jwtSettingsAccesser, IUserService userService)
        {
            _jwtSettings = _jwtSettingsAccesser.Value;
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        #region 生成token
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="user"></param>
        private object CreateToken(Users users)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var authTime = DateTime.Now;//授权时间
            var expiresAt = authTime.AddDays(30);//过期时间
            var tokenDescripor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(JwtClaimTypes.Audience,_jwtSettings.Audience),
                    new Claim(JwtClaimTypes.Issuer,_jwtSettings.Issuer),
                    new Claim(JwtClaimTypes.Name, users.Email.ToString()),
                    new Claim(JwtClaimTypes.Id, users.Id.ToString()),
                    new Claim(JwtClaimTypes.PhoneNumber, users.PassWord.ToString())
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
                    id = users.Id,
                    email = users.Email,
                    passWord = users.PassWord,
                    auth_time = authTime,
                    expires_at = expiresAt
                }
            };
            return result;
        }

        #endregion





        [HttpPost]
        public async Task<MessageModel> Login([FromBody] Users users)
        {
            MessageModel messageModel = new MessageModel();
            var userList = await _userService.GetAll();
            var user = userList.Find(m => m.Name == users.Name && m.PassWord == users.PassWord);
            if (user == null)
            {
                messageModel.Msg = "邮箱或者密码错误";

            }
            else
            {
                messageModel.Code = 200;
                messageModel.Data = CreateToken(user);
                messageModel.Msg = "登录成功";
            }

            return messageModel;
        }
    }
}
