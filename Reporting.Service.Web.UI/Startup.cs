using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Reporting.Service.Web.UI.Models;
using Owin;
using System.Security.Claims;

[assembly: OwinStartupAttribute(typeof(Reporting.Service.Web.UI.Startup))]
namespace Reporting.Service.Web.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
			createRolesandUsers();
            
		}


		// In this method we will create default User roles and Admin user for login
		private void createRolesandUsers()
		{
			ApplicationDbContext context = new ApplicationDbContext();

			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

           
            // In Startup iam creating first Admin Role and creating a default Admin User 
            if (!roleManager.RoleExists("Administrador"))
			{

				// first we create Admin rool
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Administrador";
				roleManager.Create(role);

				//Here we create a Admin super user who will maintain the website				

				var user = new ApplicationUser();
				user.UserName = "francisco_martinez@fussionweb.com";
				user.Email = "francisco_martinez@fussionweb.com";

				string userPWD = "M@$$r1v2013";

				var chkUser = UserManager.Create(user, userPWD);

				//Add default User to Role Admin
				if (chkUser.Succeeded)
				{
					var result1 = UserManager.AddToRole(user.Id, "Administrador");

				}
			}
            
		}
	}
}
