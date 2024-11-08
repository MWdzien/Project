using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Models;

namespace Project.Repositories.UserRepositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _databaseContext;

    public UserRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result> AddUser(User user)
    {
        await _databaseContext.Users.AddAsync(user);
        await _databaseContext.SaveChangesAsync();
        return new Result() { Success = true };
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _databaseContext.Users.FirstOrDefaultAsync(user => user.Username == username);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _databaseContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    }
}