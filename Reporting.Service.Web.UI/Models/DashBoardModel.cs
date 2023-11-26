using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Reporting.Service.Core;

namespace Reporting.Service.Web.UI.Models
{
    public class DashBoardModel
    {
        public int Aprobados { get; set; }
        public int EnEspera { get; set; }
        public int Destacados { get; set; }
        public int Eliminados { get; set; }

        public bool IsAdmin { get; set; }
        public List<Core.News.News> News { get; set; }
    }
}