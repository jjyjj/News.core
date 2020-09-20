using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace News.core.Model
{
    //查询通用类
    public class QueryModel
    {
        /// <summary>
        /// 是否分页
        /// </summary>
        public bool isPage { get; set; } = false;

        /// <summary>
        /// 当前页面
        /// </summary>
        public int pageIndex { get; set; } = 1;
        /// <summary>
        /// 页面大小
        /// </summary>
        public int pageSize { get; set; } = 4;
        /// <summary>
        /// 排序字段
        /// </summary>
        public string orderBy { get; set; } = null;
        /// <summary>
        /// 查询字段
        /// </summary>
        public string strWhere { get; set; } = null;
        /// <summary>
        /// 是否排序
        /// </summary>
        public bool isDesc { get; set; } = true;//是否降序 true 降序 false 升序 


    }
}
