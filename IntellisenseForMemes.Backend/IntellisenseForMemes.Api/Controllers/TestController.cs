using System.Threading.Tasks;
using IntellisenseForMemes.Api.AppHelpers;
using IntellisenseForMemes.BusinessLogic.Senders;
using IntellisenseForMemes.BusinessLogic.Senders.DtfSender;
using IntellisenseForMemes.DAL;
using IntellisenseForMemes.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public TestController(IDtfSender dtfSender, IRepository<Attachment> attachmentRepository)
        {
            _dtfSender = dtfSender;
            _attachmentRepository = attachmentRepository;
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            var a = await _attachmentRepository.AsQueryable().LastOrDefaultAsync();

            await _dtfSender.PostComment(51736, 0, "Best link ever with attachment", a.ObjectFromDtfInJson);

            return new JsonResult(AjaxResponse.Success());
        }
    }
}