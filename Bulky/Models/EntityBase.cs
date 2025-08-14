using System.ComponentModel.DataAnnotations;

namespace Bulky.Models;

public abstract class EntityBase
{
    public bool IsHidden { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    [Range(1, 100)]
    public int DisplayOrder { get; set; }
}
