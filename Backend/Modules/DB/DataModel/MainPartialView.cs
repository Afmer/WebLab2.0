using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Weblab.Architecture.Enums;

[PrimaryKey(nameof(Id))]
public class MainPartialView
{
    [Required]
    public MainPartialViewCode Id;
    [Required]
    public string Html {get; set;} = null!;
    [Required]
    public DateTime DateUpdate {get; set;}
}