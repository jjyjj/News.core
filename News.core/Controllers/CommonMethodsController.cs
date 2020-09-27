using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using News.core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace News.core.Controllers
{
    public class CommonMethodsController : BaseController
    {
        [Obsolete]
        private IHostingEnvironment hostingEnv;

        string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO" };

        [Obsolete]
        public CommonMethodsController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">users 用户头像 news文章封面</param>
        /// <returns></returns>
        #region 上传图片
        [HttpPost]
        [Obsolete]

        public MessageModel Post(int types)
        {
            string type = null;
            if (types == 1)
            {
                type = "users";
            }
            if (types == 2)
            {
                type = "news";
            }

            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            MessageModel messageModel = new MessageModel();
            //size > 100MB refuse upload !
            if (size > 104857600)
            {
                messageModel.Msg = "pictures total size > 100MB , server refused !";
                return messageModel;

            }

            List<string> filePathResultList = new List<string>();

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                string filePath = hostingEnv.WebRootPath + $@"\Files\Pictures\{type}\";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string suffix = fileName.Split('.')[1];

                if (!pictureFormatArray.Contains(suffix))
                {
                    messageModel.Msg = "the picture format not support ! you must upload files that suffix like 'png','jpg','jpeg','bmp','gif','ico'.";
                    return messageModel;
                }
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                fileName = date + "-" + Guid.NewGuid() + "." + suffix;


                string fileFullName = filePath + fileName;

                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                filePathResultList.Add($"/Files/Pictures/{type}/{fileName}");
            }

            string message = $"{files.Count} file(s) /{size} bytes uploaded successfully!";
            //Json(Return_Helper_DG.Success_Msg_Data_DCount_HttpCode(message, filePathResultList, filePathResultList.Count));
            messageModel.Data = filePathResultList;
            return messageModel;
        }

        #endregion

        #region 删除图片
        [HttpDelete]
        [Obsolete]
        public MessageModel Delete(string filePath)
        {

            string path = hostingEnv.WebRootPath + $@"\";
            if (System.IO.Directory.Exists(path))
            {
                System.IO.File.Delete(path + filePath);//删除某个指定的文件
            }
            return new MessageModel()
            {
                Code = 200,
                Msg = "success",
            };
        }

        #endregion
   
    
    }
}
