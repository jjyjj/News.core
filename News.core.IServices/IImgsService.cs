using Microsoft.AspNetCore.Http;
using News.core.Model;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.core.IServices
{
    public interface IImgsService : IBaseService<Imgs>
    {

        Task<FileModel> Add(IFormFileCollection files, int userId, int state);
        Task<dynamic> GetAll(QueryModel queryModel);

        Task<bool> Del(int id);
    }
}
