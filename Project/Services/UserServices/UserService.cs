using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Project.Models;
using Project.Repositories.UserRepositories;

namespace Project.Services.UserServices;

public class UserService: IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public UserService(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<Result> AddUser(RegisterUserDTO req)
    {
        if (await DoesUsernameExist(req.Username))
            return new Result() { Message = $"User with username: {req.Username} already exists" };
        if (await DoesEmailExist(req.Email))
            return new Result() { Message = $"User with email: {req.Email} already exists" };
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);

        var user = new User()
        {
            Username = req.Username,
            PasswordHash = passwordHash,
            Email = req.Email
        };

        var result = await _userRepository.AddUser(user);

        return result;
    }

    public async Task<string> LoginUser(LoginUserDTO req)
    {
        var user = await _userRepository.GetUserByUsername(req.Username);
        if (user == null) return $"User with username: {req.Username} not found";

        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
        {
            return "Wrong password";
        }

        var token = CreateToken(user);

        return token;
    }

    public async Task<bool> DoesUsernameExist(string username)
    {
        return await _userRepository.GetUserByUsername(username) != null;
    }

    public async Task<bool> DoesEmailExist(string email)
    {
        return await _userRepository.GetUserByEmail(email) != null;
    }

    public string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Security:Token").Value!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}