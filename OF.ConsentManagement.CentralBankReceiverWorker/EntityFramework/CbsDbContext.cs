using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.EFModel;

public class CbsDbContext : DbContext
{
    public CbsDbContext(DbContextOptions<CbsDbContext> options) : base(options) { }

    public DbSet<CoreBankPosting> coreBankPostings { get; set; }

    public DbSet<CoreBankEnquiry> coreBankEnquiries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Fluent API configs (optional)
    }
}