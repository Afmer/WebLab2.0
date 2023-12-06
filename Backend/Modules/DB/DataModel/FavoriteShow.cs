using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Weblab.Modules.DB.DataModel;

[PrimaryKey(nameof(Id))]
public class FavoriteShow
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id {get; set;}
    [Required]
    public string UserLogin {get; set;} = null!;
    [Required]
    public Guid ShowId {get; set;}
    [Required]
    public Show Show {get; set;} = null!;
}