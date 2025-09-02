using OF.ConsentManagement.CentralBankConn.API.Model;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.Common;

namespace OF.ConsentManagement.CentralBankConn.API.API;

public class SendPointInitializerHostedService : IHostedService
{
    private readonly IBus _bus;
    private readonly RabbitMqSettings _settings;
    private readonly SendPointInitialize _sendPoint;
    private readonly ConsentLogger _logger;

    public SendPointInitializerHostedService(
        IBus bus,
        IOptions<RabbitMqSettings> options,
        SendPointInitialize sendPoint,
        ConsentLogger logger)
    {
        _bus = bus;
        _settings = options.Value;
        _sendPoint = sendPoint;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _sendPoint.AugmentConsentRequest = await _bus.GetSendEndpoint(
            new Uri("queue:" + _settings.AugmentConsentRequest));

        _logger.Info("Queue AugmentConsentRequest has been created");

        _sendPoint.AugmentConsentResponse = await _bus.GetSendEndpoint(
            new Uri("queue:" + _settings.AugmentConsentResponse));

        _logger.Info("Queue AugmentConsentResponse has been created");

        _sendPoint.ValidateConsentRequest = await _bus.GetSendEndpoint(
            new Uri("queue:" + _settings.ValidateConsentRequest));

        _logger.Info("Queue ValidateConsentRequest has been created");

        _sendPoint.ValidateConsentResponse = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.ValidateConsentResponse));

        _logger.Info("Queue ValidateConsentResponse has been created");

        _sendPoint.ConsentOperationRequest = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.ConsentOperationRequest));

        _logger.Info("Queue ConsentOperationRequest has been created");

        _sendPoint.AuditLog = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.AuditLog));

        _logger.Info("Queue AuditLog has been created");

        _sendPoint.CbsPostingRequest = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.CbsPostingRequest));

        _logger.Info("Queue CbsPostingRequest has been created");

        _sendPoint.CbsPostingResponse = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.CbsPostingResponse));

        _logger.Info("Queue CbsPostingResponse has been created");

        _sendPoint.CbsEnquiryRequest = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.CbsEnquiryRequest));

        _logger.Info("Queue CbsEnquiryRequest has been created");

        _sendPoint.CbsEnquiryResponse = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.CbsEnquiryResponse));

        _logger.Info("Queue CbsEnquiryResponse has been created");

        _sendPoint.PostConsentRequest = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.PostConsentRequest));

        _logger.Info("Queue PostConsentRequest has been created");

        _sendPoint.PostConsentResponse = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.PostConsentResponse));

        _logger.Info("Queue PostConsentResponse has been created");

        _sendPoint.GetConsentRequest = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.GetConsentRequest));

        _logger.Info("Queue GetConsentRequest has been created");

        _sendPoint.GetConsentResponse = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.GetConsentResponse));

        _logger.Info("Queue GetConsentResponse has been created");


        _sendPoint.PatchConsentRequest = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.PatchConsentRequest));

        _logger.Info("Queue PatchConsentRequest has been created");

        _sendPoint.PatchConsentResponse = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.PatchConsentResponse));

        _logger.Info("Queue PatchConsentResponse has been created");

        _sendPoint.GetConsentAuditRequest = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.GetConsentAuditRequest));

        _logger.Info("Queue GetConsentAuditRequest has been created");

        _sendPoint.GetConsentAuditResponse = await _bus.GetSendEndpoint(
           new Uri("queue:" + _settings.GetConsentAuditResponse));

        _logger.Info("Queue GetConsentAuditResponse has been created");

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
