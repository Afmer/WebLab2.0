using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Weblab.Architecture.Constants;

namespace Weblab.Modules.DB.DataModel;

[PrimaryKey(nameof(Id))]
public class Show
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id {get; set;}
    [Required]
    [MaxLength(ShowConstants.NameMaxLength)]
    public string Name {get; set;} = null!;
    [Required]
    public Guid LabelImage {get; set;}
    [Required]
    [MaxLength(ShowConstants.DescriptionMaxLength)]
    public string Description {get; set;} = null!;
    [Required]
    public DateTime Date {get; set;}
    public ICollection<ShowsImage>? Images {get; set;}
}