using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using P330Pronia.Models;
using P330Pronia.Models.Common;
using P330Pronia.Models.Identity;

namespace P330Pronia.Contexts;

public class AppDbContext : IdentityDbContext<AppUser>
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor) : base(options)
    {
        _contextAccessor = contextAccessor;
    }

    public DbSet<Slider> Sliders { get; set; } = null!;
    public DbSet<Feature> Features { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Setting> Settings { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<ProductTag> ProductTags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feature>().HasQueryFilter(f => !f.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        string? name = "Admin";
        var identity = _contextAccessor?.HttpContext?.User.Identity;
        if (identity is not null)
        {
            name = identity.IsAuthenticated ? identity.Name : "Admin";
        }

        var entries = ChangeTracker.Entries<BaseSectionEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.CreatedBy = name;
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = name;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = name;
                    break;
                default:
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}