
using MessengerApp.Application.Interfaces.Context;
using MessengerApp.Domain.Common;
using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Persistence.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<MessageInfo> MessageInfos { get; set; }
    public DbSet<Customer> Customers { get; set; }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    { 
        foreach (var entry in this.ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property(nameof(BaseEntity.CreatedDate)).CurrentValue =
                            DateTime.Now;
                        break;
                    
                    case EntityState.Modified:
                        entry.Property(nameof(BaseEntity.UpdatedDate)).CurrentValue =
                            DateTime.Now;
                        break;
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}