using News.core.IRepository;
using News.core.IServices;
using News.core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.core.Services
{
    public class ImgsService : BaseService<Imgs>, IImgsService
    {
        private readonly IImgsRepository _imgsRepository;

        public ImgsService(IImgsRepository imgsRepository)
        {

            base.BaseDal = imgsRepository;
            _imgsRepository = imgsRepository ?? throw new ArgumentNullException(nameof(imgsRepository));
        }


    }
}
