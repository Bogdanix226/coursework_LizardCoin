using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CryptoTracker.Models;

namespace CryptoTracker.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{

    public DbSet<UserFavorite> UserFavorites { get; set; }
    public DbSet<UserAsset> UserAssets { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserFavorite>()
            .HasIndex(uf => new { uf.ApplicationUserId, uf.CoinId })
            .IsUnique();

        builder.Entity<UserAsset>()
            .HasIndex(ua => new { ua.ApplicationUserId, ua.CoinId })
            .IsUnique();
    }
}
