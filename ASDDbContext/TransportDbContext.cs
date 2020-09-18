using System;
using ASPDbContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASPDbContext
{
    public class TransportDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public TransportDbContext() : base() { }
        public TransportDbContext(DbContextOptions<TransportDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=BusGps;Integrated Security=True;");
        }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusLine> BusLines { get; set; }
        public DbSet<BusStop> Stops { get; set; }
        public DbSet<LinePoint> LinePoints { get; set; }
        public DbSet<LineStop> LineStops { get; set; }
        public DbSet<Point> Points { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bus>((b) =>
            {
                b.HasKey(x => x.BusId);
                b.HasOne(x => x.Line).WithMany(x => x.Buses).HasForeignKey(x => x.LineId);
                b.HasOne(x => x.Driver).WithOne(x => x.Bus).HasForeignKey<ApplicationUser>(x => x.BusId);
            });
            modelBuilder.Entity<BusStop>((bs) =>
            {
                bs.HasKey(x => x.Id);
                bs.HasOne(x => x.Point).WithOne().HasForeignKey<BusStop>(x => x.PointId);
            });
            modelBuilder.Entity<LineStop>((ls) =>
            {
                ls.HasKey(x => new { x.LineId, x.StopId });
                ls.HasOne(x => x.Line).WithMany(x => x.Stops).HasForeignKey(x => x.LineId);
                ls.HasOne(x => x.BusStop).WithMany(x => x.LineStops).HasForeignKey(x => x.StopId);
            });
            modelBuilder.Entity<LinePoint>((lp) =>
            {
                lp.HasKey(x => new { x.LineId, x.PointId, x.RowPosition });
                lp.HasOne(x => x.Point).WithMany(x => x.LinePoints).HasForeignKey(x => x.PointId);
                lp.HasOne(x => x.Line).WithMany(x => x.Route).HasForeignKey(x => x.LineId);
            });
            modelBuilder.Entity<BusLine>(bl =>
            {
                bl.Property(x => x.ColorHex).IsUnicode(false).HasMaxLength(7);
            });
            modelBuilder.Entity<ApplicationUserRole>(aur =>
            {
                aur.HasKey(x => new { x.UserId, x.RoleId });
                aur.HasOne(x => x.Role).WithMany(x => x.RoleUsers).HasForeignKey(x => x.RoleId).IsRequired(true);
                aur.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId).IsRequired(true);
            });
        }
        
    }
}
