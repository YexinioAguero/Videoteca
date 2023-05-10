using Videoteca.Models.DTO;

namespace Videoteca.Models.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);

        Task<Status> RegistrationAsync(RegistrationModel model);

        Task LogoutAsync();

    }
}
