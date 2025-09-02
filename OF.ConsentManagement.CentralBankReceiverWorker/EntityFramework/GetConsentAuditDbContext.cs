using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.EFModel;

public class GetConsentAuditDbContext : DbContext
{
    public GetConsentAuditDbContext(DbContextOptions<GetConsentDbContext> options) : base(options) { }

    public DbSet<ConsentAudit> ConsentAudits { get; set; }
    public DbSet<ConsentAuditResponse> ConsentAuditResponses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table mapping
        modelBuilder.Entity<ConsentAudit>()
            .HasMany(c => c.ConsentAuditResponses)
            .WithOne(r => r.ConsentAudit)
            .HasForeignKey(r => r.ConsentAuditId);
    }
}
