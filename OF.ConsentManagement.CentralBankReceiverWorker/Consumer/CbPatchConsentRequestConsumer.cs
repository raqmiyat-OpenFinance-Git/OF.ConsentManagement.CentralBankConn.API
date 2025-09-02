using CentralBankReceiverWorker.Mappers;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.CentralBank.Consent.PostRequestDto;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbPatchConsentRequestConsumer : IConsumer<CbPatchConsentRequestDto>
{
    private readonly ConsentLogger _logger;
    private readonly IPatchConsentService _consentService;

    public CbPatchConsentRequestConsumer(ConsentLogger logger, IPatchConsentService consentService)
    {
        _logger = logger;
        _consentService = consentService;
    }

    public async Task Consume(ConsumeContext<CbPatchConsentRequestDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbPatchConsentRequestConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbPatchConsentRequestConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");

            await CreateAsync(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbPatchConsentRequestConsumer.Consume()");
            throw; // Rethrow to enable MassTransit retry/error policies
        }
    }

    private async Task CreateAsync(CbPatchConsentRequestDto requestWrapper)
    {
        try
        {
            var consentResponse = CbPatchConsentMapper.MapCbPatchConsentRequestToEF(requestWrapper);
            var header = requestWrapper.cbPatchConsentHeader;
            var request = requestWrapper.cbPatchConsentRequest;
            long consentRequestId = await _consentService.GetConsentRequestIdAsync(header!.ConsentId, _logger.Log);
            await _consentService.UpdateConsentRequestAsync(consentRequestId, request!.Status,  _logger.Log);
            await _consentService.UpdateConsentResponseAsync(consentRequestId, consentResponse, _logger.Log);
            Console.WriteLine($"PatchConsentRequest inserted. Id = {requestWrapper.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbPatchConsentRequestConsumer: Error occurred in PatchConsentRequest. CorrelationId: {requestWrapper?.CorrelationId}");
        }
    }
}
