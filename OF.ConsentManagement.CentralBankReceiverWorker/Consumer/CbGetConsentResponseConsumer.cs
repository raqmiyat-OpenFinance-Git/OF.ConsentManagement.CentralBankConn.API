using CentralBankReceiverWorker.Mappers;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.CentralBank.Consent.GetResponseDto;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbGetConsentResponseConsumer : IConsumer<CbGetConsentResponseDto>
{
    private readonly ConsentLogger _logger;
    private readonly IGetConsentService _consentService;

    public CbGetConsentResponseConsumer(ConsentLogger logger, IGetConsentService consentService)
    {
        _logger = logger;
        _consentService = consentService;
    }

    public async Task Consume(ConsumeContext<CbGetConsentResponseDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbGetConsentResponseConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbGetConsentResponseConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");

            await CreateAsync(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbGetConsentResponseConsumer.Consume()");
            throw; // Rethrow to enable MassTransit retry/error policies
        }
    }

    private async Task CreateAsync(CbGetConsentResponseDto responseWrapper)
    {
        try
        {
            long consentStatusHistoryId = await _consentService.GetConsentStatusHistoryIdAsync(responseWrapper.CorrelationId, _logger.Log);
            var consentIds = CbGetConsentMapper.MapCbGetConsentIds(responseWrapper);
            var consentIdentifiers = await _consentService.GetConsentRequestIdsAsync(responseWrapper.CorrelationId, consentIds, _logger.Log);
            var consentResponseHistories = CbGetConsentMapper.MapCbGetConsentResponseToEF(responseWrapper, consentIdentifiers, consentStatusHistoryId);
            
            await _consentService.SaveConsentResponseHistoryAsync(responseWrapper.CorrelationId, consentResponseHistories, _logger.Log);
            Console.WriteLine($"CreateAsync inserted. Id = {responseWrapper.CorrelationId}");
            var consentStatusHistory = CbGetConsentMapper.MapCbGetConsentStatusHistory(responseWrapper, consentIdentifiers, consentStatusHistoryId);
            await _consentService.UpdateConsentStatusHistoryAsync(consentStatusHistoryId, responseWrapper.CorrelationId, consentStatusHistory, _logger.Log);
            
            Console.WriteLine($"CreateAsync inserted. Id = {responseWrapper.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbGetConsentResponseConsumer: Error occurred in CreateAsync. CorrelationId: {responseWrapper?.CorrelationId}");
        }
    }
}
