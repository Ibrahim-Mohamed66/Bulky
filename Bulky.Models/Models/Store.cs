

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models.Models;

public class Store
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? StreetAddress { get; set; }
    public string? PhoneNumber  { get; set; }

    [ValidateNever]
    public List<ProductStore> ProductStores { get; set; }
}
