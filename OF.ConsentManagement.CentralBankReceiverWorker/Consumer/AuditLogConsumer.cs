using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Consumer;

#region MassTransitConsumer
[ExcludeFromConfigureEndpoints]
public class AuditLogConsumer : IConsumer<AuditLog>
{
    private readonly ConsentLogger _logger;
    
    public AuditLogConsumer(ConsentLogger logger)
    {
        _logger = logger;
        
    }
    public async Task Consume(ConsumeContext<AuditLog> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("AuditLog message is null.");
                return;
            }
            await ProcessAsync(context.Message);

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in AuditLog.Consume()");
        }
    }
    private async Task ProcessAsync(AuditLog auditLog)
    {
        try
        {
            //await _logger.Info(auditLog, _logger.Log);
            _logger.Info($"Received AuditLog - CorrelationId: {auditLog.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error occurred in AuditLog-ProcessAsync(). Request ID: {auditLog?.CorrelationId}");
        }
    }
}


#endregion
