using System.ComponentModel.DataAnnotations;
using Weblab.Architecture.Constants;

namespace Weblab.Models;

public class AddShowModel
{
    [Required]
    [MaxLength(ShowConstants.NameMaxLength)]
    public string Name {get; set;} = null!;
    [Required]
    [MaxLength(ShowConstants.DescriptionMaxLength)]
    public string Description {get; set;} = null!;
    [Required]
    public DateTime Date {get; set;}
    [Required]
    public IFormFile LabelImage {get; set;} = null!;
    public IFormFile[]? Images {get; set;}
}