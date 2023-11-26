using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class AppController : Controller
    {
        //private AppManager _manager;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        ApplicationDbContext context;
        public AppController()
        {
            //_manager = new AppManager();
            context = new ApplicationDbContext();
        }
        // GET: App
        public bool Prueba()
        {
            if (!Request.IsAuthenticated)
                return false;

            return true;
        }
    }
}