using System.ComponentModel.DataAnnotations;
using Weblab.Architecture.Constants;

namespace Weblab.Models;
public class ShowModel : ShortShowModel
{
    [Required]
    [MaxLength(ShowConstants.DescriptionMaxLength)]
    public string Description {get; set;} = null!;
    [Required]
    public Guid LabelImage {get; set;}
    [Required]
    public DateTime Date {get; set;}
    public Guid[]? Images {get; set;}
}