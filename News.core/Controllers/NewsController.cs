using Microsoft.AspNetCore.Mvc;
using News.core.IServices;
using News.core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.core.Controllers
{
    public class NewsController : BaseController
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
        }


        [HttpGet]
        public MessageModel GetAll()
        {

            var data = _newsService.GetAll().ToList();
            return new MessageModel()
            {
                Code = 200,
                Msg = "获取成功",
                Data = data
            };

        }
    }
}
