
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Models.ViewModels;

public class ProductVM
{   
    public Product Product { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();
    [ValidateNever]
    public IEnumerable<SelectListItem> StoreList { get; set; } = new List<SelectListItem>();
    [ValidateNever]
    public List<int> SelectedStoreIds { get; set; } = new List<int>();

}
