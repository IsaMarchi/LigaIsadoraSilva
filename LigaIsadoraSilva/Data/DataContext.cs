using LigaIsadoraSilva.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LigaIsadoraSilva.Data
{
    public class DataContext : IdentityDbContext<User>
    {   
        public DbSet<FootballMatch> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<FootballTeam> Clubs { get; set; }
  
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Se eu apagar o jogo, não vai apagar o time. Desligando a exclusão em cascata. 
            builder.Entity<FootballMatch>()
                .HasOne(g => g.HomeTeam)
                .WithMany(c => c.HomeGames)
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FootballMatch>()
               .HasOne(g => g.VisitTeam)
               .WithMany(c => c.VisitGames)
               .HasForeignKey(g => g.VisitTeamId)
               .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
