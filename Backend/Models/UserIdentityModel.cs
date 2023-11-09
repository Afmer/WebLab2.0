using System.ComponentModel.DataAnnotations;
using Weblab.Architecture.Constants;
using Weblab.Architecture.Enums;

namespace Weblab.Models;

public class UserIdentityModel : UserIdentityBaseModel
{
    [Required]
    [MaxLength(40)]
    public string PasswordHash {get; set;} = null!;
    [Required]
    public int Salt {get; set;}
    [Required]
    [EmailAddress]
    public string Email {get; set;} = null!;
}