using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.News
{
    public class LastNews
    {
        public int Sequence { get; set; }
        public string Summary { get; set; }
        public string UrlShowMore { get; set; }
        public bool Enabled { get; set; } 
    }
}
