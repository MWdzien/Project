using Project.Models;

namespace Project.Repositories.UserRepositories;

public interface IUserRepository
{
    Task<Result> AddUser(User user);
    Task<User?> GetUserByUsername(string username);
    Task<User?> GetUserByEmail(string email);
}