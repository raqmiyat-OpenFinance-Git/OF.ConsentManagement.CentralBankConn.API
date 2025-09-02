using CentralBankReceiverWorker.Mappers;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.CentralBank.Consent.GetResponseDto;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbGetConsentAuditResponseConsumer : IConsumer<CbGetConsentAuditResponseDto>
{
    private readonly ConsentLogger _logger;
    private readonly IGetConsentAuditService _consentService;

    public CbGetConsentAuditResponseConsumer(ConsentLogger logger, IGetConsentAuditService consentService)
    {
        _logger = logger;
        _consentService = consentService;
    }

    public async Task Consume(ConsumeContext<CbGetConsentAuditResponseDto> context)
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

    private async Task CreateAsync(CbGetConsentAuditResponseDto responseWrapper)
    {
        try
        {
            long consentAuditId = await _consentService.GetConsentAuditIdAsync(responseWrapper.CorrelationId, _logger.Log);
            
            
            var consentAuditResponses = CbGetConsentAuditMapper.MapCbGetConsentAuditResponseToEF(responseWrapper, consentAuditId);
            
            await _consentService.SaveConsentAuditResponseAsync(consentAuditResponses, responseWrapper.CorrelationId, consentAuditId, _logger.Log);
            Console.WriteLine($"CreateAsync inserted. Id = {responseWrapper.CorrelationId}");
          
            await _consentService.UpdateConsentAuditAsync(consentAuditId, "PROCESSED", responseWrapper.CorrelationId, _logger.Log);
            
            Console.WriteLine($"CreateAsync inserted. Id = {responseWrapper.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbGetConsentResponseConsumer: Error occurred in CreateAsync. CorrelationId: {responseWrapper?.CorrelationId}");
        }
    }
}
