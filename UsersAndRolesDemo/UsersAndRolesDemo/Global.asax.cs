using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;


namespace UsersAndRolesDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
            .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;


        }

        void Application_PostAuthenticateRequest() {
            if (User.Identity.IsAuthenticated) {
                var name = User.Identity.Name; // Get current user name.
 
                MyDbEntities context = new MyDbEntities();
                var user  = context.AspNetUsers.Where(u=>u.UserName == name).FirstOrDefault();
                IQueryable<string> roleQuery = from  u in context.AspNetUsers 
                            from  r in u.AspNetRoles 
                            where u.UserName == Context.User.Identity.Name
                            select r.Name;
                string[] roles = roleQuery.ToArray();

                HttpContext.Current.User = Thread.CurrentPrincipal =
                                           new GenericPrincipal(User.Identity, roles);
            }
        }
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

    }
}
