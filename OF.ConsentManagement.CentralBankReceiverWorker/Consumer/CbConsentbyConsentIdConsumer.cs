using ConsentManagerCommon.Logging;
using ConsentMangerModel.CoreBank;

namespace ConsentManagerBackendReceiverWorker.Consumer;

public class CbConsentbyConsentIdConsumer : IConsumer<CbsConsentRevokedDto>
{
    private readonly RevokeConsentApiLogger _logger;
    private readonly IRevokeConsent _Service;

    public CbConsentbyConsentIdConsumer(RevokeConsentApiLogger logger, IRevokeConsent Service)
    {
        _logger = logger;
        _Service = Service;
    }

    public async Task Consume(ConsumeContext<CbsConsentRevokedDto> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbConsentbyConsentGroupIdConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbConsentbyConsentGroupIdConsumer: Received message - CorrelationId: {context.Message.CorrelationId}");
            await SaveRevokeConsentId(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbConsentbyConsentGroupIdConsumer.Consume()");
        }
    }

    private async Task SaveRevokeConsentId(CbsConsentRevokedDto request)
    {
        try
        {


        }
        catch (Exception ex)
        {
            _logger.Error(ex);
        }
    }
}
