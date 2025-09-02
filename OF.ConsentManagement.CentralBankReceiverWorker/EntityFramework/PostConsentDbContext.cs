

using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.EFModel;

public class PostConsentDbContext : DbContext
{
    public PostConsentDbContext(DbContextOptions<PostConsentDbContext> options) : base(options) { }

    public DbSet<ConsentRequest> ConsentRequest { get; set; }

    public DbSet<ConsentResponse> ConsentResponse { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ConsentRequest
        modelBuilder.Entity<ConsentRequest>(entity =>
        {
            entity.ToTable("ConsentRequest");
            entity.HasKey(e => e.ConsentRequestId);

        });

       
    }
}
