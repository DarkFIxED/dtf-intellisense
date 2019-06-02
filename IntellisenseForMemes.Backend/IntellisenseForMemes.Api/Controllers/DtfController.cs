using System.Threading.Tasks;
using IntellisenseForMemes.Api.AppHelpers;
using IntellisenseForMemes.BusinessLogic.Senders.DtfSender;
using IntellisenseForMemes.BusinessLogic.Senders.DtfSender.Models;
using IntellisenseForMemes.BusinessLogic.Services.Meme;
using IntellisenseForMemes.Common.Settings;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IntellisenseForMemes.Api.Controllers
{
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class DtfController : ControllerBase
    {
        private readonly ILogger _logger;

        private readonly IDtfSender _dtfSender;

        private readonly IMemeService _memeService;

        public DtfController(ILogger logger, IDtfSender dtfSender, IMemeService memeService)
        {
            _logger = logger;
            _dtfSender = dtfSender;
            _memeService = memeService;
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

            var attachmentObject = await _memeService.GetDtfAttachmentByMemeName(memeName);
            await _dtfSender.PostComment(comment.Data.Content.Id, comment.Data.ReplyTo, string.Empty, attachmentObject);

            return new JsonResult(AjaxResponse.Success());
        }
    }
}
