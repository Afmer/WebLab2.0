using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Weblab.Modules.DB.DataModel;

[PrimaryKey(nameof(Id))]
public class ShowsImage 
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id {get; set;}
    [Required]
    public Guid ShowId {get; set;}
    [Required]
    public Show Show {get; set;} = null!;
}