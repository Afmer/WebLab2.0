using System.ComponentModel.DataAnnotations;
using Weblab.Architecture.Constants;

namespace Weblab.Models;
public class ShowModel : ShortShowModel
{
    [Required]
    [MaxLength(ShowConstants.DescriptionMaxLength)]
    public string Description {get; set;} = null!;
    [Required]
    public DateTime Date {get; set;}
}