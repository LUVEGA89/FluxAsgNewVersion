using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resporting.Service.Core.Api
{
    public class Response
    {
        public Response() { }
        public int ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public List<ResponseItem> SearchItems { get; set; }
    }
}
