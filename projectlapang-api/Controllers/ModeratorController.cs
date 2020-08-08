using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace projectlapang_api.Controllers
{
    [Authorize(Roles = "Moderator", Users = "projectlapang-system@gmail.com")]
    public class ModeratorController : ApiController
    {
    }
}
