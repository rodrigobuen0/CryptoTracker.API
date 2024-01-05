using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CryptoTracker.API.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Ativos>? Ativos { get; set; }
        public DbSet<Transacoes>? Transacoes { get; set; }
        public DbSet<Portfolio>? Portfolio { get; set; }
        public DbSet<Alertas>? Alertas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
