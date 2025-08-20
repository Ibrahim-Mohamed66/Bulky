using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bulky.Models.Models;
public class Category:EntityBase
{

    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(30, MinimumLength = 3)]
    public string Name { get; set; }
    [JsonIgnore] 
    public List<Product> Products { get; set; } = new List<Product>();
}
