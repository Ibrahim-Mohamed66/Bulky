
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.Models;

public class Company:EntityBase
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Company Name is required.")]
    public string Name { get; set; } 

    public string? StreetAddress { get; set; }
    public string? City { get; set; }

    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }


}
