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
        private readonly IUserService _userService;

        [Obsolete]
        private readonly IHostingEnvironment hostingEnv;

        string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO" };

        [Obsolete]
        public ImgsController(IImgsService imgsService, IUserService userService, IHostingEnvironment env)
        {
            _imgsService = imgsService ?? throw new ArgumentNullException(nameof(imgsService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService)); this.hostingEnv = env;
        }



        [HttpGet]
        public async Task<MessageModel> GetAll(QueryModel queryModel, int state)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                if (!queryModel.isPage) messageModel.Data = await _imgsService.Query(m => m.State == 0);
                else
                {
                    var imgsList = await _imgsService.Pagination<Model.Entities.Imgs, object>(queryModel.pageIndex, queryModel.pageSize, n => n.CreateTime, x => x.State == state, queryModel.isDesc);
                    foreach (var item in imgsList.data)
                    {
                        item.User = await _userService.GetOneById(item.UserId);
                    }
                    messageModel.Data = imgsList;
                }
                messageModel.Code = 200;
                messageModel.Msg = "获取数据成功";
            }
            catch (Exception)
            {

                throw;
            }
            return messageModel;


        }
        [HttpGet]

        public async Task<Model.MessageModel> GetOneById(int id)
        {

            return new MessageModel() { Code = 200, Data = await _imgsService.GetOneById(id), Msg = "获取成功" };
        }
        [HttpGet]

        public async Task<Model.MessageModel> GetAllByUserId(int userId, int lunBoListLength, QueryModel queryModel)
        {
            //分页可选，状态可选
            #region news ways
            MessageModel messageModel = new MessageModel();
            var list = await _imgsService.Query(m => m.UserId == userId);
            if (!queryModel.isPage)
            {
                messageModel.Data = list;
            }
            else
            {
                messageModel.Data = await _imgsService.Pagination<Imgs, object>(
                       queryModel.pageIndex,
                       queryModel.pageSize,
                       n => n.CreateTime,
                       n => n.UserId == userId,
                       queryModel.isDesc
                       );
            }
            //如果是查轮播图片，则取出5条记录
            if (lunBoListLength > 0)
            {
                messageModel.Data = list.FindAll(s => s.State == 1).Take(lunBoListLength);
            }


            #endregion


            messageModel.Code = 200;
            return messageModel;



            //if (!queryModel.isPage)
            //{
            //    return new MessageModel() { Code = 200, Data = await _imgsService.Query(m => m.UserId == userId), Msg = "获取成功" };
            //}
            //else
            //{
            //    return new MessageModel()
            //    {
            //        Code = 200,
            //        Data = await _imgsService.Pagination<Imgs, object>(
            //            queryModel.pageIndex,
            //            queryModel.pageSize,
            //            n => n.CreateTime,
            //            n => n.UserId == userId,
            //            queryModel.isDesc
            //            ),
            //        Msg = "获取成功"
            //    };

            //}


        }

        [HttpPost]
        [Obsolete]
        public async Task<MessageModel> Add(int userId)
        {
            MessageModel messageModel = new MessageModel();
            List<string> filePathResultList = new List<string>();
            try
            {
                #region 数据处理
                string type = $@"LunBo/{userId}";
                var files = Request.Form.Files;
                long size = files.Sum(f => f.Length);

                //size > 100MB refuse upload !
                if (size > 104857600)
                {
                    messageModel.Msg = "pictures total size > 100MB , server refused !";
                    return messageModel;

                }


                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    string filePath = hostingEnv.WebRootPath;
                    filePath = filePath + $@"\Files\Pictures\{type}\";


                    string suffix = fileName.Split('.')[1];
                    if (!pictureFormatArray.Contains(suffix))
                    {
                        messageModel.Msg = "the picture format not support ! you must upload files that suffix like 'png','jpg','jpeg','bmp','gif','ico'.";
                        return messageModel;
                    }
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    fileName = date + "-" + Guid.NewGuid() + "." + suffix;
                    string fileFullName = filePath + fileName;
                    filePathResultList.Add($"/Files/Pictures/{type}/{fileName}");
                    #endregion

                    //写入数据库filePathResultList
                    Imgs imgs = new Imgs();
                    imgs.Url = filePathResultList[0];
                    imgs.UserId = userId;
                    var createId = await _imgsService.Create(imgs);

                    try
                    {
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        using (FileStream fs = System.IO.File.Create(fileFullName))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    catch (Exception)
                    {
                        _imgsService.RollBack();
                    }


                }
                string message = $"{files.Count} file(s) /{size} bytes uploaded successfully!";
            }
            catch (Exception)
            {

                throw;
            }

            messageModel.Data = filePathResultList;
            return messageModel;
        }


        [HttpDelete]
        [Obsolete]
        public async Task<MessageModel> Del(int id)
        {
            var img = await _imgsService.GetOneById(id);
            string filePath = img.Url;
            await _imgsService.Delete(img);
            try
            {
                string path = hostingEnv.WebRootPath + $@"\";
                if (System.IO.Directory.Exists(path))
                {
                    System.IO.File.Delete(path + filePath);//删除某个指定的文件
                }
            }
            catch (Exception)
            {

                _imgsService.RollBack();
            }
            return new MessageModel() { Code = 200, Data = null, Msg = "删除成功" };

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
