using CentralBankReceiverWorker.Mappers;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.CentralBank.Consent.PostRequestDto;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbPostConsentRequestConsumer : IConsumer<CbPostConsentRequestDto>
{
    private readonly ConsentLogger _logger;
    private readonly IPostConsentService _consentService;

    public CbPostConsentRequestConsumer(ConsentLogger logger, IPostConsentService consentService)
    {
        _logger = logger;
        _consentService = consentService;
    }

    public async Task Consume(ConsumeContext<CbPostConsentRequestDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbPostConsentsRequestConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbPostConsentsRequestConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");

            await CreateAsync(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbPostConsentsRequestConsumer.Consume()");
            throw; // Rethrow to enable MassTransit retry/error policies
        }
    }

    private async Task CreateAsync(CbPostConsentRequestDto requestWrapper)
    {
        try
        {
            var consentRequest = CbPostConsentMapper.MapCbPostConsentRequestToEF(requestWrapper);
            await _consentService.SaveConsentRequestAsync(consentRequest, _logger.Log);
            Console.WriteLine($"ConsentRequest inserted. Id = {requestWrapper.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbPostConsentsRequestConsumer: Error occurred in CreatePaymentsAsync. CorrelationId: {requestWrapper?.CorrelationId}");
        }
    }
}
