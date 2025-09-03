using ConsentManagerBackendReceiverWorker.IServices;
using ConsentManagerBackendReceiverWorker.Mappers;
using ConsentManagerCommon.NLog;
using ConsentMangerModel.Consent;

namespace OF.ServiceInitiation.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbPatchPaymentLogRequestConsumer : IConsumer<CbPatchPaymentRecordRequestDto>
{
    private readonly PaymentsApiLogger _logger;
    private readonly IPaymentsService _paymentsService;

    public CbPatchPaymentLogRequestConsumer(PaymentsApiLogger logger, IPaymentsService paymentsService)
    {
        _logger = logger;
        _paymentsService = paymentsService;
    }

    public async Task Consume(ConsumeContext<CbPatchPaymentRecordRequestDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbPatchPaymentLogRequestConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbPatchPaymentLogRequestConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");

            await SavePatchPaymentLogRequestAsync(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbPatchPaymentLogRequestConsumer.Consume()");
        }
    }

    private async Task SavePatchPaymentLogRequestAsync(CbPatchPaymentRecordRequestDto requestWrapper)
    {
        try
        {
            _logger.Info($"CbPatchPaymentLogRequestConsumer SavePatchPaymentLogRequestAsync started for CorrelationId: {requestWrapper?.CorrelationId}");
            if (requestWrapper is null)
                return;

            var patchPaymentLogConsent = CbPaymentLogMapper.MapCbPatchPaymentLogToEF(requestWrapper, _logger.Log);
            await _paymentsService.SavePatchPaymentLogRequestAsync(requestWrapper.CorrelationId, patchPaymentLogConsent, _logger.Log);
            _logger.Info($"CbPatchPaymentLogRequestConsumer SavePatchPaymentLogRequestAsync completed for CorrelationId: {requestWrapper?.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbPatchPaymentLogRequestConsumer: Error occurred in SavePatchPaymentLogRequestAsync. CorrelationId: {requestWrapper?.CorrelationId}");
        }
    }
}
