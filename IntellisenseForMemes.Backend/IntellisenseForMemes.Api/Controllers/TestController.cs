using System.Threading.Tasks;
using IntellisenseForMemes.Api.AppHelpers;
using IntellisenseForMemes.BusinessLogic.Senders.DtfSender;
using IntellisenseForMemes.BusinessLogic.Services.Meme;
using IntellisenseForMemes.DAL;
using IntellisenseForMemes.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IntellisenseForMemes.Api.Controllers
{

    /// <summary>
    /// You can use it controller for any experiments and test your features.
    /// </summary>
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IDtfSender _dtfSender;

        private readonly IRepository<Attachment> _attachmentRepository;

        private readonly IMemeService _memeService;

        private readonly ILogger _logger;

        public TestController(IDtfSender dtfSender, IRepository<Attachment> attachmentRepository, ILogger logger, IMemeService memeService)
        {
            _dtfSender = dtfSender;
            _attachmentRepository = attachmentRepository;
            this._logger = logger;
            _memeService = memeService;
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            await _memeService.NormalizedMemes();
            return new JsonResult(AjaxResponse.Success());
        }
    }
}