using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Weblab.Modules.DB.DataModel;

[PrimaryKey(nameof(Id))]
public class PartialView
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id {get; set;}
    [Required]
    [MaxLength(100)]
    [MinLength(1)]
    public string Title {get; set;} = null!;
    [Required]
    public string Html {get; set;} = null!;
    [Required]
    public DateTime DateCreation {get; set;}
    [Required]
    public DateTime DateUpdate {get; set;}
}