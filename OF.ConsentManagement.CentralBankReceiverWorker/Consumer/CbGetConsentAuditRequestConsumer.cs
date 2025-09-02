using CentralBankReceiverWorker.Mappers;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.CentralBank.Consent.GetRequestDto;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbGetConsentAuditRequestConsumer : IConsumer<CbGetConsentAuditRequestDto>
{
    private readonly ConsentLogger _logger;
    private readonly IGetConsentAuditService _consentService;

    public CbGetConsentAuditRequestConsumer(ConsentLogger logger, IGetConsentAuditService consentService)
    {
        _logger = logger;
        _consentService = consentService;
    }

    public async Task Consume(ConsumeContext<CbGetConsentAuditRequestDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbGetConsentAuditRequestConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbGetConsentAuditRequestConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");

            await CreateAsync(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbGetConsentAuditRequestConsumer.Consume()");
            throw; // Rethrow to enable MassTransit retry/error policies
        }
    }

    private async Task CreateAsync(CbGetConsentAuditRequestDto requestWrapper)
    {
        try
        {
            var consentAudit = CbGetConsentAuditMapper.MapCbGetConsentAuditRequestToEF(requestWrapper);
            await _consentService.SaveConsentAuditAsync(consentAudit, _logger.Log);
            Console.WriteLine($"CreateAsync inserted. Id = {requestWrapper.CorrelationId}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"CbGetConsentRequestConsumer: Error occurred in CreateAsync. CorrelationId: {requestWrapper?.CorrelationId}");
        }
    }
}
