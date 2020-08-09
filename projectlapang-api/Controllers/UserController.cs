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
        public IHttpActionResult getuserby(string t, string q)
        {
            var email = AuthController.ValidateToken(t);
            var user = db.Users.FirstOrDefault(x => x.phoneno == q || x.email == q);

            if (user == null)
            {
                return NotFound();
            }
            else if (email == user.email)
            {
                return BadRequest("You cannot create chatroom with yourself");
            }

            return Ok(user);
        }
    }
}
