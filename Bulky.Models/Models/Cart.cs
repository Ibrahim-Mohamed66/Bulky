using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models.Models;

public class Cart
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Count must be at least 1")]
    public int Count { get; set; }

    [NotMapped]
    public double TotalPrice
    {
        get
        {
            if (Product == null) return 0;

            if (Count > 100)
                return Count * Product.Price100;
            if (Count > 50)
                return Count * Product.Price50;

            return Count * Product.Price;
        }
    }

    [Required]
    public string ApplicationUserId { get; set; } = string.Empty;

    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser? ApplicationUser { get; set; }

    [Required]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product? Product { get; set; }
}
