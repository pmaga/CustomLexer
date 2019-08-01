using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CustomTokenizer.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenizerController : ControllerBase
    {
        // GET api/sayhi
        [HttpGet]
        public ActionResult<string> SayHi()
        {
            return "Hello world";
        }
    }
}
