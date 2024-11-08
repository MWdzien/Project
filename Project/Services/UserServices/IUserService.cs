using Project.Models;

namespace Project.Services.UserServices;

public interface IUserService
{
    Task<Result> AddUser(RegisterUserDTO req);
    Task<string> LoginUser(LoginUserDTO req);
    Task<bool> DoesUsernameExist(string username);
    Task<bool> DoesEmailExist(string email);

    string CreateToken(User user);
}