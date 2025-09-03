using ConsentMangerModel.EFModel.Payments;

namespace ConsentManagerBackendReceiverWorker.EntityFramework;

public class PaymentLogDbContext : DbContext
{
    public PaymentLogDbContext(DbContextOptions<PaymentLogDbContext> options) : base(options) { }
    public DbSet<GetPaymentLog> GetPaymentLog { get; set; }
    public DbSet<PatchPaymentLog> PatchPaymentLog { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GetPaymentLog>().ToTable("GetPaymentLog");

        modelBuilder.Entity<PatchPaymentLog>().ToTable("PatchPaymentLog");
    }
}
