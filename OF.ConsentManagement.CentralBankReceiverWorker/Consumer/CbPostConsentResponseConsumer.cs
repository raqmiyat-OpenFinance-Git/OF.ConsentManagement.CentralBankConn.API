using CentralBankReceiverWorker.Mappers;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.CentralBank.Consent.PostResponseDto;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbPostConsentResponseConsumer : IConsumer<CbPostConsentResponseDto>
{
    private readonly ConsentLogger _logger;
    private readonly IPostConsentService _consentService;

    public CbPostConsentResponseConsumer(ConsentLogger logger, IPostConsentService consentService)
    {
        _logger = logger;
        _consentService = consentService;
    }

    public async Task Consume(ConsumeContext<CbPostConsentResponseDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbPostConsentResponseConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbPostConsentResponseConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");

            await CreateAsync(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbPostConsentResponseConsumer.Consume()");
            throw; // Rethrow to enable MassTransit retry/error policies
        }
    }

    private async Task CreateAsync(CbPostConsentResponseDto responseWrapper)
    {
        try
        {
            var consentResponse = CbPostConsentMapper.MapCbPostConsentResponseToEF(responseWrapper);
            long consentRequestId = await _consentService.GetConsentRequestIdAsync(responseWrapper.CorrelationId, _logger.Log);

            await _consentService.SaveConsentResponseAsync(consentRequestId, responseWrapper.CorrelationId, consentResponse, _logger.Log);
            await _consentService.UpdateConsentRequestStatusAsync(consentRequestId, responseWrapper.status, responseWrapper.CorrelationId, _logger.Log);
            _logger.Info($"CbPostConsentResponseConsumer: ConsentResponse inserted - CorrelationId: {responseWrapper.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbPostConsentResponseConsumer: Error in CreateAsync. CorrelationId: {responseWrapper.CorrelationId}");
        }
    }
}
