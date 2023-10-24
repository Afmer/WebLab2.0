using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Weblab.Architecture.Constants;
using Weblab.Architecture.Enums;

namespace Weblab.Modules.DB.DataModel;

[PrimaryKey(nameof(Login))]
public class UserIdentityInfo
{
    [StringLength(UserIdentityConstants.LoginMaxLength, MinimumLength = UserIdentityConstants.LoginMinLength)]
    public string Login {get; set;} = null!;
    [Required]
    [MaxLength(40)]
    public string PasswordHash {get; set;} = null!;
    [Required]
    public int Salt {get; set;}
    [Required]
    public Role Role {get; set;}
    [Required]
    [EmailAddress]
    public string Email {get; set;} = null!;
}