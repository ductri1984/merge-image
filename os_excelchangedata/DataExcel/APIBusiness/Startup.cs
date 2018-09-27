using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using System.Configuration;

[assembly: OwinStartup(typeof(APIBusiness.Startup))]
namespace APIBusiness
{    
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Allow all origins
            app.UseCors(CorsOptions.AllowAll);
            
            ExtendConfig(app);
        }
        
        private static void ExtendConfig(IAppBuilder app)
        {
            //string HostIS4 = ConfigurationManager.AppSettings["IdentityServer"];
            //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            //{
            //    Authority = HostIS4,
            //    RoleClaimType = System.Security.Claims.ClaimTypes.Role,
            //    RequiredScopes = new[] { "api1" }
            //});
        }
    }
}