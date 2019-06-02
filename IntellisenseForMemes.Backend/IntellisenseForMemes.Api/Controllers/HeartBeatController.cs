using System;
using IntellisenseForMemes.Api.AppHelpers;
using Microsoft.AspNetCore.Mvc;

namespace IntellisenseForMemes.Api.Controllers
{
    [ApiController]
    public class HeartBeatController : ControllerBase
    {
        //TODO: Move to settings
        private const string Version = "0.1.1";

        [HttpGet]
        [Route("api/echo")]
        public ActionResult<DateTime> Echo()
        {
            return new JsonResult(AjaxResponse.Success(DateTime.UtcNow));
        }

        [HttpGet]
        [Route("version")]
        public ActionResult<DateTime> GetVersion()
        {
            return new JsonResult(AjaxResponse.Success(Version));
        }
    }
}
