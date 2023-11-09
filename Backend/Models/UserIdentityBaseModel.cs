using System.ComponentModel.DataAnnotations;
using Weblab.Architecture.Constants;
using Weblab.Architecture.Enums;

namespace Weblab.Models;
public class UserIdentityBaseModel
{
    [StringLength(UserIdentityConstants.LoginMaxLength, MinimumLength = UserIdentityConstants.LoginMinLength)]
    public string Login {get; set;} = null!;
    [Required]
    public Role Role {get; set;}
}