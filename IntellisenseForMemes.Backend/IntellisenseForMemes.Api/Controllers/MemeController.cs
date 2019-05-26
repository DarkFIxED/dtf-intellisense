using System.Collections.Generic;
using System.Threading.Tasks;
using IntellisenseForMemes.Api.AppHelpers;
using IntellisenseForMemes.BusinessLogic.Services.Meme;
using IntellisenseForMemes.BusinessLogic.Services.Meme.Models;
using IntellisenseForMemes.BusinessLogic.Services.User;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace IntellisenseForMemes.Api.Controllers
{
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class MemeController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IMemeService _memeService;

        public MemeController(IMemeService memeService, IUserService userService)
        {
            _memeService = memeService;
            _userService = userService;
        }

        [HttpGet]
        [Route("memes/search")]
        public async Task<IActionResult> GetMemes(string option)
        {
            var memes = await _memeService.SearchMemes(option);

            return new JsonResult(AjaxResponse.Success(memes));
        }

        [HttpGet]
        [Route("memes")]
        public async Task<IActionResult> GetMemes()
        {
            var memes = await _memeService.GetMemes();

            return new JsonResult(AjaxResponse.Success(memes));
        }

        [HttpGet]
        [Route("memes/{memeId:int}")]
        public async Task<IActionResult> GetMeme(int memeId)
        {
            var meme = await _memeService.GetMeme(memeId);

            return new JsonResult(AjaxResponse.Success(meme));
        }

        [HttpPut]
        [Route("memes")]
        public async Task<IActionResult> UpdateMeme(MemeModel meme)
        {
            var currentUser = await _userService.GetCurrentUser(HttpContext.User);
            await _memeService.UpdateMeme(currentUser, meme);

            return new JsonResult(AjaxResponse.Success());
        }

        [HttpPost]
        [Route("memes")]
        public async Task<IActionResult> CreateMemes(MemeModel meme)
        {
            var currentUser = await _userService.GetCurrentUser(HttpContext.User);
            var memeId = await _memeService.CreateMeme(currentUser, meme);

            return new JsonResult(AjaxResponse.Success(memeId));
        }
    }
}
