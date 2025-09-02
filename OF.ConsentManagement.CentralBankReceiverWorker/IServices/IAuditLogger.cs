using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.IServices;
public interface IAuditLogger
{
    Task LogAsync(AuditLog log, Logger logger);
}