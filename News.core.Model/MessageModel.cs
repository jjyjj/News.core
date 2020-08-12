using System;
using System.Collections.Generic;
using System.Text;

namespace News.core.Model
{
    public class MessageModel
    {
        public int Code { get; set; } = 0;
        public string Msg { get; set; }
        public dynamic Data { get; set; }
    }
}
