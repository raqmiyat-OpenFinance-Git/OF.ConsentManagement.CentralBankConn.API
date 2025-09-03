using ConsentMangerModel.Consent;

namespace OF.ServiceInitiation.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbGetPaymentLogResponseConsumer : IConsumer<CbGetAuditConsentByConsentIdResponseDto>
{
    private readonly PaymentsApiLogger _logger;
    private readonly IPaymentsService _paymentsService;

    public CbGetPaymentLogResponseConsumer(PaymentsApiLogger logger, IPaymentsService paymentsService)
    {
        _logger = logger;
        _paymentsService = paymentsService;
    }

    public async Task Consume(ConsumeContext<CbGetAuditConsentByConsentIdResponseDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbGetPaymentLogResponseConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbGetPaymentLogResponseConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");

            await UpdateGetPaymentLogResponseAsync(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbGetPaymentLogResponseConsumer.Consume()");
        }
    }

    private async Task UpdateGetPaymentLogResponseAsync(CbGetAuditConsentByConsentIdResponseDto responseWrapper)
    {
        try
        {
            _logger.Info($"CbGetPaymentLogResponseConsumer UpdateGetPaymentLogResponseAsync started for CorrelationId: {responseWrapper?.CorrelationId}");

            if (responseWrapper is null)
                return;

            await _paymentsService.UpdateGetPaymentLogRequestStatusAsync(responseWrapper.CorrelationId, responseWrapper.status!, responseWrapper.auditConsentsByConsentIdResponse, _logger.Log);
            _logger.Info($"CbGetPaymentLogResponseConsumer UpdateGetPaymentLogResponseAsync completed for CorrelationId: {responseWrapper?.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbGetPaymentLogResponseConsumer: Error occurred in UpdateGetPaymentLogResponseAsync. CorrelationId: {responseWrapper?.CorrelationId}");
        }
    }
}
