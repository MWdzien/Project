using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class LoginUserDTO
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}