using System.ComponentModel.DataAnnotations;
using Weblab.Architecture.Constants;

namespace Weblab.Models;

public class ShortShowModel
{
    [Required]
    public Guid Id {get; set;}
    [Required]
    [MaxLength(ShowConstants.NameMaxLength)]
    public string Name {get; set;} = null!;
}