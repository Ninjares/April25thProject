using System;
using ASDDbContext.Models;
using ASPDbContext.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPDbContext
{
    public class TransportDbContext : DbContext
    { 
        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusLine> BusLines { get; set; }
        public DbSet<BusStop> Stops { get; set; }
        public DbSet<LinePoint> LinePoints { get; set; }
        public DbSet<LineStop> LineStops { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=TransportASP;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bus>((b) =>
            {
                b.HasKey(x => x.BusId);
                b.HasOne(x => x.Line).WithMany(x => x.Buses).HasForeignKey(x => x.LineId);
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
            modelBuilder.Entity<User>((u) =>
            {
                u.HasKey(x => x.Id);
                u.HasOne(x => x.Role).WithMany(x => x.Users).HasForeignKey(x => x.RoleId);
            });
            modelBuilder.Entity<Role>((r) =>
            {
                r.HasKey(x => x.Id);
            });
        }
        
    }
}
