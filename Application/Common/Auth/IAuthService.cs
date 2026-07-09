using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;

namespace DeanInfoSystem.Application.Common.Auth;



public interface IAuthService
{
    public Task<Dictionary<string, string>> RegisterAsync(NewUserDTO nuDTO, Position? position = null);
    public Task<Dictionary<string, string>> LoginAsync(LoginFormDTO lfDTO);
    public Task<Dictionary<string, string>> AttemptRefreshAsync(string refreshToken);
    public Task ClearTokensAsync(string accessToken, string refreshToken);
    public Task<string[]?> GetAdministratorAsync();
    public Task<User> AuthenticateUserAsync(string token);

}