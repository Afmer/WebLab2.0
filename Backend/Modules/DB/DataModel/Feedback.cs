using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Weblab.Architecture.Constants;

namespace Weblab.Modules.DB.DataModel;
[PrimaryKey(nameof(Id))]
public class Feedback
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id {get; set;}
    [Required]
    [MaxLength(50)]
    public string Label {get; set;} = null!;
    [Required]
    public string Text {get; set;} = null!;
    [StringLength(UserIdentityConstants.LoginMaxLength, MinimumLength = UserIdentityConstants.LoginMinLength)]
    public string UserId {get; set;} = null!;
    public UserIdentityInfo User {get; set;} = null!; 
}