using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using News.core.IRepository;
using News.core.IServices;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace News.core.Services
{
    public class ImgsService : BaseService<Imgs>, IImgsService
    {
        [Obsolete]


        string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO" };
        private readonly IImgsRepository _imgsRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUserRepository _userRepository;

        public ImgsService(IImgsRepository imgsRepository, IHostingEnvironment hostingEnvironment, IUserRepository userRepository)
        {

            base.BaseDal = imgsRepository;
            _imgsRepository = imgsRepository ?? throw new ArgumentNullException(nameof(imgsRepository));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }


        [Obsolete]
        public async Task<FileModel> Add(IFormFileCollection files, int userId, int state)
        {
            FileModel fileModel = new FileModel();

            List<string> filePathResultList = new List<string>();
            try
            {
                #region 数据处理
                string type = "";
                //0相片 1轮播图片 2新闻图片(未实现) 3头像(未实现) 4类别图片(未实现)

                if (state == 2) { type = $@"article"; }
                else if (state == 3) { type = $@"avater"; }
                else if (state == 4) { type = $@"cate"; }
                else { type = $@"users"; }



                long size = files.Sum(f => f.Length);

                //size > 100MB refuse upload !
                if (size > 104857600)
                {
                    fileModel.msg = "pictures total size > 100MB , server refused !";

                }


                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    string filePath = _hostingEnvironment.WebRootPath;
                    filePath = filePath + $@"\Files\Pictures\{type}\";


                    string suffix = fileName.Split('.')[1];
                    if (!pictureFormatArray.Contains(suffix))
                    {
                        fileModel.msg = "the picture format not support ! you must upload files that suffix like 'png','jpg','jpeg','bmp','gif','ico'.";
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
                    imgs.State = state;
                    fileModel.id = await _imgsRepository.Create(imgs);
                    fileModel.url = filePathResultList[0];

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
                        fileModel.msg = "上传图片失败";
                    }


                }
                fileModel.msg = $"{files.Count} file(s) /{size} bytes uploaded successfully!";
            }
            catch (Exception)
            {

                throw;
            }

            //messageModel.Data = filePathResultList;
            return fileModel;
        }

        public async Task<dynamic> GetAll(QueryModel queryModel)
        {
            if (!queryModel.isPage) return await _imgsRepository.Query();
            else
            {
                var imgsList = await _imgsRepository.Pagination<Model.Entities.Imgs, object>(queryModel.pageIndex, queryModel.pageSize, n => n.CreateTime, null, queryModel.isDesc);
                foreach (var item in imgsList.data)
                {
                    item.User = await _userRepository.GetOneById(item.UserId);
                }
                return imgsList;
            }
        }

        [Obsolete]
        public async Task<bool> Del(int id)
        {
            var img = await _imgsRepository.GetOneById(id);
            string filePath = img.Url;
            await _imgsRepository.Delete(img);
            try
            {
                string path = _hostingEnvironment.WebRootPath + $@"\";
                if (System.IO.Directory.Exists(path))
                {
                    System.IO.File.Delete(path + filePath);//删除某个指定的文件
                }
                return true;
            }
            catch (Exception)
            {

                _imgsRepository.RollBack();
                return false;
            }

        }



    }
}
