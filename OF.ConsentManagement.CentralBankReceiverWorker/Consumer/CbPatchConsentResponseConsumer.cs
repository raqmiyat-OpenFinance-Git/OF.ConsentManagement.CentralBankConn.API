using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.CentralBank.Consent.PostRequestDto;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbPatchConsentResponseConsumer : IConsumer<CbPatchConsentResponseDto>
{
    private readonly ConsentLogger _logger;
    private readonly IPatchConsentService _consentService;

    public CbPatchConsentResponseConsumer(ConsentLogger logger, IPatchConsentService consentService)
    {
        _logger = logger;
        _consentService = consentService;
    }

    public async Task Consume(ConsumeContext<CbPatchConsentResponseDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbPatchConsentsResponseConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbPatchConsentsResponseConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");

            await CreateAsync(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbPatchConsentsResponseConsumer.Consume()");
            throw; // Rethrow to enable MassTransit retry/error policies
        }
    }

    private async Task CreateAsync(CbPatchConsentResponseDto responseWrapper)
    {
        try
        {
            long ConsentRequestId = await _consentService.GetConsentRequestIdAsync(responseWrapper.ConsentId, _logger.Log);
            Console.WriteLine($"Consent Updated. Id = {responseWrapper.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbPatchConsentsResponseConsumer: Error occurred in CreateAsync. CorrelationId: {responseWrapper?.CorrelationId}");
        }
    }
}
