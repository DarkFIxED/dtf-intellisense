using System.Security.Claims;
using System.Threading.Tasks;
using IntellisenseForMemes.BusinessLogic.Services.User.Models;

namespace IntellisenseForMemes.BusinessLogic.Services.User
{
    public interface IUserService
    {
        Task<string> SignUp(SignUpModel model);
        Task<string> SignIn(SignInModel model);
        Task<Domain.User> GetCurrentUser(ClaimsPrincipal currentUser);
    }
}