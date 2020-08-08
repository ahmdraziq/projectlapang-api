using projectlapang_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace projectlapang_api.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        projectlapangEntities db = new projectlapangEntities();

        [HttpGet]
        public IHttpActionResult getuserdetail(string t)
        {
            var email = AuthController.ValidateToken(t);

            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Invalid Token");
            }

            var user = db.Users.Select(x => new
            {
                x.email,
                x.fullname,
                x.phoneno,
                x.UserType.typename,
            }).FirstOrDefault(x => x.email == email);

            if (user == null)
                return NotFound();
            else
                return Ok(user);
        }

        [HttpGet]
        public IHttpActionResult getlistuser(string t)
        {
            var email = AuthController.ValidateToken(t);
            var users = db.Users.Select(x => new
            {
                x.email,
                x.usertypeid,
                x.phoneno,
                x.fullname
            }).Where(x => x.email != email && x.usertypeid != 1).ToList();

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }
    }
}
