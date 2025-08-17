using Bulky.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data;

public class BulkyDbContext : DbContext
{
    public BulkyDbContext(DbContextOptions<BulkyDbContext> options) : base(options)
    {

    }
    public DbSet<Category> Categories { get; set; }
    
    
}
