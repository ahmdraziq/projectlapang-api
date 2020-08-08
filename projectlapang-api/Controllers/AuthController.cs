using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.IO;
using projectlapang_api.Models;
using System.Security.Cryptography;
using System.Text;

namespace projectlapang_api.Controllers
{
    public class AuthController : ApiController
    {
        projectlapangEntities db = new projectlapangEntities();

        [HttpPost]
        public IHttpActionResult register(User user)
        {
            try
            {
                user.password = md5(user.password);
                user.usertypeid = 3;
                db.Users.Add(user);
                db.SaveChanges();
                return Created("", user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult authenticate(LoginModel loginModel)
        {
            var newpass = md5(loginModel.password);
            var user = db.Users.FirstOrDefault(x => x.email == loginModel.email && x.password == newpass);
            if (user == null)
                return NotFound();
            else
                return Ok(GenerateToken(user.email, user.UserType.typename));
        }

        private static string GenerateToken(string email, string role)
        {
            byte[] key = Convert.FromBase64String(File.ReadAllText(@"C:\Users\RAZIQ-PC\Desktop\Flutter Project\token\token.txt"));
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            }),
                Expires = DateTime.UtcNow.AddMinutes(10080),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
                Issuer = "projectlapang.com.my"
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(File.ReadAllText(@"C:\Users\RAZIQ-PC\Desktop\Flutter Project\token\token.txt"));
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidIssuer = "projectlapang.com.my",
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public static string ValidateToken(string token)
        {
            string email = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim emailClaim = identity.FindFirst(ClaimTypes.Email);
            email = emailClaim.Value;
            return email;
        }

        private string md5(string password)
        {
            MD5 mD5 = MD5.Create();
            var passbyte = Encoding.ASCII.GetBytes(password);
            var hashpass = mD5.ComputeHash(passbyte);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashpass.Length; i++)
            {
                sb.Append(hashpass[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
