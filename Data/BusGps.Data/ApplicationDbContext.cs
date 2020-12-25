namespace BusGps.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using BusGps.Data.Common.Models;
    using BusGps.Data.Models;
    using BusGps.Data.Models.AppModels;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Bus> Buses { get; set; }

        public DbSet<BusLine> BusLines { get; set; }

        public DbSet<Point> Points { get; set; }

        public DbSet<Stop> Stops { get; set; }

        public DbSet<LineStop> LineStops { get; set; }

        public DbSet<LinePoint> LinePoints { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);
            builder.Entity<Bus>((b) =>
            {
                b.HasOne(x => x.Line).WithMany(x => x.Buses).HasForeignKey(x => x.LineId);
                b.HasOne(x => x.Driver).WithOne(x => x.Bus).HasForeignKey<ApplicationUser>(x => x.BusId);
            });
            builder.Entity<Stop>((bs) =>
            {
                bs.HasKey(x => x.Id);
                bs.HasOne(x => x.Point).WithOne().HasForeignKey<Stop>(x => x.PointId);
            });
            builder.Entity<LineStop>((ls) =>
            {
                ls.HasKey(x => new { x.LineId, x.StopId });
                ls.HasOne(x => x.Line).WithMany(x => x.Stops).HasForeignKey(x => x.LineId);
                ls.HasOne(x => x.BusStop).WithMany(x => x.LineStops).HasForeignKey(x => x.StopId);
            });
            builder.Entity<LinePoint>((lp) =>
            {
                lp.HasKey(x => new { x.LineId, x.PointId, x.RowPosition });
                lp.HasOne(x => x.Point).WithMany(x => x.LinePoints).HasForeignKey(x => x.PointId);
                lp.HasOne(x => x.Line).WithMany(x => x.Route).HasForeignKey(x => x.LineId);
            });
            builder.Entity<BusLine>(bl =>
            {
                bl.Property(x => x.ColorHex).IsUnicode(false).HasMaxLength(7);
            });

            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
