﻿using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model
{
    public class MessageModel
    {
        public int Code { get; set; } = 0;
        public string Msg { get; set; } = "请求失败";
        public dynamic Data { get; set; }
    }
}
