using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace News.core.Controllers
{
    public class ImgsController : BaseController
    {
        private readonly IImgsService _imgsService;


        public ImgsController(IImgsService imgsService)
        {
            _imgsService = imgsService ?? throw new ArgumentNullException(nameof(imgsService));

        }



        [HttpGet]
        public async Task<MessageModel> GetAll(QueryModel queryModel)
        {
            var list = await _imgsService.GetAll(queryModel);
            return new MessageModel()
            {
                Code = 200,
                Data = list,
                Msg = list == null ? "暂无数据" : "获取成功"
            };

        }
        [HttpGet]
        public async Task<Model.MessageModel> GetOneById(int id)
        {

            return new MessageModel() { Code = 200, Data = await _imgsService.GetOneById(id), Msg = "获取成功" };
        }
        [HttpGet]
        public async Task<Model.MessageModel> GetAllByUserId(int userId, int? state, int? carouselLength, QueryModel queryModel)
        {

            List<Imgs> imgsList = await _imgsService.Query(m => m.UserId == userId);
            MessageModel messageModel = new MessageModel();
            PageModel<Model.Entities.Imgs> pageModel = new PageModel<Model.Entities.Imgs>();
            //我的相册
            if (state == null)
            {
                pageModel.data = imgsList;
                if (queryModel.isPage)
                {
                    pageModel = await _imgsService.Pagination<Imgs, object>(queryModel.pageIndex, queryModel.pageSize, m => m.CreateTime, n => n.UserId == userId, true);
                }
            }
            //我的头像
            else
            {
                //我的轮播
                if (carouselLength != null)
                {
                    pageModel.data = imgsList.FindAll(m => m.State == state).Take(carouselLength.Value).ToList();
                }
                else { pageModel = await _imgsService.Pagination<Imgs, object>(queryModel.pageIndex, queryModel.pageSize, m => m.CreateTime, m => m.State == state, true); }

            }
            messageModel.Code = 200;
            messageModel.Data = pageModel;
            return messageModel;



        }

        [HttpPost]

        public async Task<MessageModel> Add(int userId, int state)
        {
            //0相片 1轮播图片 2新闻图片(未实现) 3头像(未实现) 4类别图片(未实现)
            var files = Request.Form.Files;
            var fileModel = await _imgsService.Add(files, userId, state);
            return new MessageModel()
            {
                Code = 200,
                Data = fileModel,
                Msg = fileModel.id > 0 ? "创建成功" : "创建失败"
            };

        }


        [HttpDelete]

        public async Task<MessageModel> Del(int id)
        {
            var isDel = await _imgsService.Del(id);
            return new MessageModel()
            {
                Code = 200,
                Data = isDel,
                Msg = isDel ? "删除成功" : "删除失败"
            };

        }

        [HttpPut]
        public async Task<MessageModel> Update(int id, int state)
        {
            var img = await _imgsService.GetOneById(id);
            img.State = state;

            var isUpdate = await _imgsService.Update(img);
            return new MessageModel() { Code = 200, Data = null, Msg = "修改成功" };


        }
    }
}
