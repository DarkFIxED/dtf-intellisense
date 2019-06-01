using System;
using IntellisenseForMemes.Api.AppHelpers;
using Microsoft.AspNetCore.Mvc;

namespace IntellisenseForMemes.Api.Controllers
{
    [ApiController]
    public class HeartBeatController : ControllerBase
    {
        [HttpGet]
        [Route("api/echo")]
        public ActionResult<DateTime> Echo()
        {
            return new JsonResult(AjaxResponse.Success(DateTime.UtcNow));
        }
    }
}
