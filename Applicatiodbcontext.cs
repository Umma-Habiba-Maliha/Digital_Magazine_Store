using DigitalMagazineStore.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace DigitalMagazineStore.Data
{
public class ApplicationDbContext : IdentityDbContext
{
public ApplicationDbContext(DbContextOptions&lt;ApplicationDbContext&gt; options)
: base(options)
{
}
public DbSet&lt;Category&gt; Categories { get; set; }
public DbSet&lt;Magazine&gt; Magazines { get; set; }
public DbSet&lt;Cart&gt; Carts { get; set; }
public DbSet&lt;CartDetail&gt; CartDetails { get; set; }
public DbSet&lt;Order&gt; Orders { get; set; }
public DbSet&lt;OrderDetail&gt; OrderDetails { get; set; }
public DbSet&lt;OrderStatus&gt; OrderStatuses{ get; set; }
}
}