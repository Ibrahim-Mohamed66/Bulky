using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models.Models;

public class Product : EntityBase
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters")]
    [Display(Name = "Book Title")]
    public string Title { get; set; } 
    public string? Description { get; set; }

    [Required(ErrorMessage = "ISBN is required")]
    [Display(Name = "ISBN Number")]
    public string ISBN { get; set; } 

    [Required(ErrorMessage = "Author name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Author name must be between 2 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z\s\.'-]+$",
        ErrorMessage = "Author name can only contain letters, spaces, periods, apostrophes, and hyphens")]
    [Display(Name = "Author Name")]
    public string Author { get; set; } 

    [Required(ErrorMessage = "List price is required")]
    [Range(0.01, 999.99, ErrorMessage = "List price must be between $0.01 and $999.99")]
    [Display(Name = "List Price")]
    public double ListPrice { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 999.99, ErrorMessage = "Price must be between $0.01 and $999.99")]
    [Display(Name = "Price for 1-50")]
    public double Price { get; set; }

    [Required(ErrorMessage = "Price for 50+ is required")]
    [Range(0.01, 999.99, ErrorMessage = "Price for 50+ must be between $0.01 and $999.99")]
    [Display(Name = "Price for 50+")]
    public double Price50 { get; set; }

    [Required(ErrorMessage = "Price for 100+ is required")]
    [Range(0.01, 999.99, ErrorMessage = "Price for 100+ must be between $0.01 and $999.99")]
    [Display(Name = "Price for 100+")]
    public double Price100 { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category? Category { get; set; }
    [ValidateNever]
    public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    [ValidateNever]
    public List<ProductStore> ProductStores { get; set; } = new List<ProductStore>();

}
