using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;

[assembly: OwinStartup(typeof(projectlapang_api.Startup))]

namespace projectlapang_api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        RequireExpirationTime = true,
                        ValidIssuer = "projectlapang.com.my",
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(File.ReadAllText(@"C:\Users\RAZIQ-PC\Desktop\Flutter Project\token\token.txt")))
                    }
                });
        }
    }
}
