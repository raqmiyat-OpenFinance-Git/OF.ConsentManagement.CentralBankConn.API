using ConsentManagerBackendReceiverWorker.Mappers;
using ConsentMangerModel.Consent;

namespace OF.ServiceInitiation.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbGetPaymentLogRequestConsumer : IConsumer<CbGetAuditConsentByConsentIdRequestDto>
{
    private readonly PaymentsApiLogger _logger;
    private readonly IPaymentsService _paymentsService;

    public CbGetPaymentLogRequestConsumer(PaymentsApiLogger logger, IPaymentsService paymentsService)
    {
        _logger = logger;
        _paymentsService = paymentsService;
    }

    public async Task Consume(ConsumeContext<CbGetAuditConsentByConsentIdRequestDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbGetPaymentLogRequestConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbGetPaymentLogRequestConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");

            await SaveGetPaymentLogRequestAsync(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbGetPaymentLogRequestConsumer.Consume()");
        }
    }

    private async Task SaveGetPaymentLogRequestAsync(CbGetAuditConsentByConsentIdRequestDto requestWrapper)
    {
        try
        {
            _logger.Info($"CbGetPaymentLogRequestConsumer SaveGetPaymentLogRequestAsync started for CorrelationId: {requestWrapper?.CorrelationId}");
            if (requestWrapper is null)
                return;

            var getPaymentLog = CbPaymentLogMapper.MapCbGetPaymentLogToEF(requestWrapper,_logger.Log);
            await _paymentsService.SaveGetPaymentLogRequestAsync(requestWrapper.CorrelationId, getPaymentLog, _logger.Log);
            _logger.Info($"CbGetPaymentLogRequestConsumer SaveGetPaymentLogRequestAsync completed for CorrelationId: {requestWrapper?.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbGetPaymentLogRequestConsumer: Error occurred in SaveGetPaymentLogRequestAsync. CorrelationId: {requestWrapper?.CorrelationId}");
        }
    }
}
