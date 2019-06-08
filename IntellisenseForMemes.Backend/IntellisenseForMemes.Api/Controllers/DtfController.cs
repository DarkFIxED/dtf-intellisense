using System;
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
            try
            {
                if (comment == null || string.IsNullOrWhiteSpace(comment.Data?.Text) || comment.Data?.Content == null || comment.Data?.Content.Id.HasValue == false || comment.Data?.Id.HasValue == false)
                {
                    return new JsonResult(AjaxResponse.Success());
                }

                if (comment.Data.Text.Length > 3000)
                {
                    return new JsonResult(AjaxResponse.Success());
                }

                var attachmentObject = await _memeService.GetDtfAttachmentByMemeName(comment.Data.Text);
                await _dtfSender.PostComment(comment.Data.Content.Id.Value, comment.Data.Id.Value, string.Empty, attachmentObject);
                return new JsonResult(AjaxResponse.Success());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new JsonResult(AjaxResponse.Success());
            }
        }
    }
}
