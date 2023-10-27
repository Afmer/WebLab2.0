using System.ComponentModel.DataAnnotations;
using Weblab.Architecture.Constants;

namespace Weblab.Models;

public class RegisterModel
{
    [Required]
    [StringLength(UserIdentityConstants.LoginMaxLength, MinimumLength = UserIdentityConstants.LoginMinLength)]
    public string Login {get; set;} = null!;
    [Required]
    [MinLength(UserIdentityConstants.PasswordMinLength)]
    public string Password {get; set;} = null!;
    [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
    public string ConfirmPassword {get; set;} = null!;
    [Required]
    [EmailAddress]
    public string Email {get; set;} = null!;
}