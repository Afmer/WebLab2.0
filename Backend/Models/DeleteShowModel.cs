using System.ComponentModel.DataAnnotations;

namespace Weblab.Models;

public class DeleteShowModel
{
    [Required]
    public Guid ShowId {get; set;}
}