using Focus.API.Entities;
using Focus.API.Entities.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Focus.API.Data
{
    public class ApplicationDBContext: IdentityDbContext<UserEntity>
    {
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<TaskListEntity> TaskLists { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var tracker = ChangeTracker;

            foreach (var entry in tracker.Entries())
            {
                if (entry.Entity is BaseEntity)
                {
                    var referenceEntity = entry.Entity as BaseEntity;

                    if (referenceEntity is null)
                        continue;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            referenceEntity.CreatedAt = DateTimeOffset.UtcNow;
                            break;
                        case EntityState.Deleted:
                        case EntityState.Modified:
                            referenceEntity.UpdatedAt = DateTimeOffset.UtcNow;
                            break;
                        default:
                            break;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}