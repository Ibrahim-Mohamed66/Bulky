using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.Models;
public class Category:EntityBase
{

    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(30, MinimumLength = 3)]
    public string Name { get; set; }
}
