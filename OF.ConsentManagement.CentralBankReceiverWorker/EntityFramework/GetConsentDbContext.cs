using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.EFModel;

public class GetConsentDbContext : DbContext
{
    public GetConsentDbContext(DbContextOptions<GetConsentDbContext> options) : base(options) { }

    public DbSet<ConsentStatusHistory> ConsentStatusHistory { get; set; }
    public DbSet<ConsentResponseHistory> ConsentResponseHistory { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table mapping
        modelBuilder.Entity<ConsentStatusHistory>().ToTable("ConsentStatusHistory");
        modelBuilder.Entity<ConsentResponseHistory>().ToTable("ConsentResponseHistory");

        // Primary keys
        modelBuilder.Entity<ConsentStatusHistory>()
            .HasKey(q => q.ConsentStatusHistoryId);

        //modelBuilder.Entity<ConsentResponseHistory>()
        //    .HasKey(q => q.ConsentUsageId);

        // Relationships
        modelBuilder.Entity<ConsentResponseHistory>()
            .HasOne(r => r.ConsentStatusHistory)     // Navigation property in ConsentResponseHistory
            .WithMany(s => s.ConsentResponseHistories) // Navigation property in ConsentStatusHistory
            .HasForeignKey(r => r.ConsentStatusHistoryId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete (optional)

    }
}
