using CentralBankReceiverWorker.Mappers;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.CentralBank.Consent.GetRequestDto;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbGetConsentRequestConsumer : IConsumer<CbGetConsentRequestDto>
{
    private readonly ConsentLogger _logger;
    private readonly IGetConsentService _consentService;

    public CbGetConsentRequestConsumer(ConsentLogger logger, IGetConsentService consentService)
    {
        _logger = logger;
        _consentService = consentService;
    }

    public async Task Consume(ConsumeContext<CbGetConsentRequestDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbGetConsentRequestConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbGetConsentRequestConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");

            await CreateAsync(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbGetConsentRequestConsumer.Consume()");
            throw; // Rethrow to enable MassTransit retry/error policies
        }
    }

    private async Task CreateAsync(CbGetConsentRequestDto requestWrapper)
    {
        try
        {
            var consentStatusHistory = CbGetConsentMapper.MapCbGetConsentRequestToEF(requestWrapper);
            await _consentService.SaveConsentStatusHistoryAsync(consentStatusHistory, _logger.Log);
            Console.WriteLine($"CreateAsync inserted. Id = {requestWrapper.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbGetConsentRequestConsumer: Error occurred in CreateAsync. CorrelationId: {requestWrapper?.CorrelationId}");
        }
    }
}
