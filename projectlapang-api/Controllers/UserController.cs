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
        public IHttpActionResult getuserby(string q)
        {
            var users = db.Users.FirstOrDefault(x => x.phoneno == q || x.email == q);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }
    }
}
