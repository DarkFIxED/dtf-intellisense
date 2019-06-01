using System.Threading.Tasks;
using IntellisenseForMemes.Api.AppHelpers;
using IntellisenseForMemes.BusinessLogic.Senders.DtfSender.Models;
using IntellisenseForMemes.Common.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IntellisenseForMemes.Api.Controllers
{
    [ApiController]
    public class DtfController : ControllerBase
    {
        private readonly ILogger _logger;

        public DtfController(ILogger logger)
        {
            _logger = logger;
        }

        [Route("webhooks/dtf/comments")]
        public async Task<IActionResult> NewComment(DtfRequestModel<DtfComment> comment)
        {
            if (string.IsNullOrWhiteSpace(comment?.Data?.Text))
            {
                return new JsonResult(AjaxResponse.Success());
            }

            var memeName = DtfHelper.MemeNameFromComment(comment.Data.Text);
            if (string.IsNullOrWhiteSpace(memeName))
            {
                return new JsonResult(AjaxResponse.Success());
            }

            _logger.LogDebug($"User ask a meme with name {memeName} on comment with id {comment.Data.ReplyTo}");

            return new JsonResult(AjaxResponse.Success());
        }
    }
}
