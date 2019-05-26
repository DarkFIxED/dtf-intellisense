using System.Threading.Tasks;
using IntellisenseForMemes.Api.AppHelpers;
using IntellisenseForMemes.BusinessLogic.Senders.DtfSender.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntellisenseForMemes.Api.Controllers
{
    [ApiController]
    public class DtfController : ControllerBase
    {
        [Route("webhooks/dtf/comments")]
        public async Task<IActionResult> NewComment(DtfRequestModel<DtfComment> comment)
        {
            //51736

            return new JsonResult(AjaxResponse.Success(comment));
        }
    }
}
