using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Common
{
    public class CommonJsonResult
    {   
        public bool Result { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}