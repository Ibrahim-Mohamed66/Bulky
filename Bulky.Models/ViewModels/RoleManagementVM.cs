
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Models.ViewModels;

public class RoleManagementVM
{
    public ApplicationUser ApplicationUser { get; set; }
    public IEnumerable<SelectListItem> Roles { get; set; }
    public IEnumerable<SelectListItem> Companies { get; set; } 
}
