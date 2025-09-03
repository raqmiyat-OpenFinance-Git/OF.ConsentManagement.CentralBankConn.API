using ConsentManagerCommon.Logging;
using ConsentMangerModel.CoreBank;

namespace ConsentManagerBackendReceiverWorker.Consumer;

public class CbConsentbyConsentGroupIdConsumer : IConsumer<CbsConsentRevokedDto>
{
    private readonly RevokeConsentApiLogger _logger;
    private readonly IRevokeConsent _Service;

    public CbConsentbyConsentGroupIdConsumer(RevokeConsentApiLogger logger, IRevokeConsent Service)
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
            await SaveRevokeConsentGroupId(context.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbConsentbyConsentGroupIdConsumer.Consume()");
        }
    }

    private async Task SaveRevokeConsentGroupId(CbsConsentRevokedDto request)
    {
        try
        {
            await _Service.RevokeConsentAsync(request,_logger.Log);
            _logger.Info($"ConsentRequest Updated. ConsentGroupID = {request.ConsentGroupId}");

        }
        catch (Exception ex)
        {
            _logger.Error(ex);
        }
    }
}
