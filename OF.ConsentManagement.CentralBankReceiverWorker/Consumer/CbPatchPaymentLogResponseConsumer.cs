using ConsentManagerBackendReceiverWorker.IServices;
using ConsentManagerCommon.NLog;
using ConsentMangerModel.Consent;

namespace OF.ServiceInitiation.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbPatchPaymentLogResponseConsumer : IConsumer<CbPatchPaymentRecordResponseDto>
{
    private readonly PaymentsApiLogger _logger;
    private readonly IPaymentsService _paymentsService;

    public CbPatchPaymentLogResponseConsumer(PaymentsApiLogger logger, IPaymentsService paymentsService)
    {
        _logger = logger;
        _paymentsService = paymentsService;
    }

    public async Task Consume(ConsumeContext<CbPatchPaymentRecordResponseDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbPatchPaymentLogResponseConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbPatchPaymentLogResponseConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");

            await UpdatePatchPaymentLogResponseAsync(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbPatchPaymentLogResponseConsumer.Consume()");
        }
    }

    private async Task UpdatePatchPaymentLogResponseAsync(CbPatchPaymentRecordResponseDto responseWrapper)
    {
        try
        {
            _logger.Info($"CbPatchPaymentLogResponseConsumer UpdatePatchPaymentLogResponseAsync started for CorrelationId: {responseWrapper?.CorrelationId}");

            if (responseWrapper is null)
                return;

            await _paymentsService.UpdatePatchPaymentLogRequestStatusAsync(responseWrapper.CorrelationId, responseWrapper.status!,responseWrapper.response!, _logger.Log);
            _logger.Info($"CbPatchPaymentLogResponseConsumer UpdatePatchPaymentLogResponseAsync completed for CorrelationId: {responseWrapper?.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbPatchPaymentLogResponseConsumer: Error occurred in UpdatePatchPaymentLogResponseAsync. CorrelationId: {responseWrapper?.CorrelationId}");
        }
    }
}
