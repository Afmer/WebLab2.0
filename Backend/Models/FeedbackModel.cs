using System.ComponentModel.DataAnnotations;

namespace Weblab.Models;
public class FeedbackModel
{
    [Required]
    [MaxLength(50)]
    public string Label {get; set;} = null!;
    [Required]
    public string Text {get; set;} = null!;
}