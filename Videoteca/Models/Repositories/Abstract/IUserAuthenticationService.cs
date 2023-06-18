using Videoteca.Models.DTO;

namespace Videoteca.Models.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<ApplicationUser> GetUserByNameAsync(string userName);
        Task<int> GetAccessFailedCountAsync(ApplicationUser user);
        Task<int> GetFailedAccessCountAsync(ApplicationUser user);
        Task<bool> GetLockoutEnabledAsync(ApplicationUser user);
        Task LockUserAsync(ApplicationUser user);

        Task<Status> LoginAsync(LoginModel model);

        Task<Status> RegistrationAsync(RegistrationModel model);

        Task LogoutAsync();

    }
}
