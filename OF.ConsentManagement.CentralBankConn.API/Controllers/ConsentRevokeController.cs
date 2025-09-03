using ConsentManagerCommon.Logging;
using ConsentManagerService.Services;
using ConsentMangerModel.Consent;
using ConsentMangerModel.CoreBank;
using OF.ConsentManagement.CentralBankConn.API.Model;
using OF.ConsentManagement.Common.AES;
using OF.ConsentManagement.Common.Helpers;
using OF.ConsentManagement.Model.Common;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ConsentManagerService.Controllers
{
    public class ConsentRevokeController : ControllerBase
    {
        private readonly IRevokeConsentService _service;
        private readonly RevokeConsentApiLogger _logger;
        private readonly SendPointInitialize _sendPointInitialize;
        private readonly IOptions<ServiceParams> _serviceParams;
        private readonly IOptions<CoreBankApis> _backEndApis;
        private readonly IOptions<SecurityParameters> _securityParameters;

        public ConsentRevokeController(IRevokeConsentService service, RevokeConsentApiLogger logger, SendPointInitialize sendPointInitialize, IOptions<ServiceParams> serviceParams, IOptions<CoreBankApis> backEndApis, IOptions<SecurityParameters> securityParameters)
        {
            _service = service;
            _logger = logger;
            _sendPointInitialize = sendPointInitialize;
            _serviceParams = serviceParams;
            _backEndApis = backEndApis;
            _securityParameters = securityParameters;
        }

        [HttpPost]
        [Route("/consent-groups/{consentGroupId}/consents/action/revoke")]
        public async Task<IActionResult> RevokeConsentbyConsentGroupId([FromQuery(Name = "consentGroupId")][Required()] string consentGroupId, [FromBody] CbRevokeConsent Request)
        {

            var stopwatch = Stopwatch.StartNew();

            string requestJson = string.Empty;
            string responseJson = string.Empty;
            string endPointUrl = string.Empty;
            Guid guid = Guid.Empty;
            try
            {
                _logger.Info("RevokeConsentbyConsentGroupId invoked.");
                _logger.Info("------------------------------------------------------------------------");
                endPointUrl = UrlHelper.CombineUrl(_backEndApis.Value.BaseUrl!, _backEndApis.Value.ConsentManagementUrl!.RevokeConsentbyConsentGroupId!);

                if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
                        !Guid.TryParse(corrIdObj?.ToString(), out guid))
                {
                    _logger.Info($"Invalid CorrelationId: {corrIdObj}");
                    return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
                }

                CbsConsentRevokedDto cbRequestDto = new()
                {
                    CorrelationId = guid,
                    PsuUserId = Request.RevokedByPsu.UserId,
                    Revokedby = Request.RevokedBy,
                    ConsentGroupId = consentGroupId,
                    ConsentId = null

                };

                requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);

                _logger.Info($"Request received:\n{requestJson}");

                var apiResult = await _service.RevokeConsentbyConsentGroupid(cbRequestDto, _logger.Log);

                if (apiResult == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        errorCode = "404",
                        errorMessage = "Payment data not found."
                    });
                }


                if (apiResult is NoContentResult)
                {
                    cbRequestDto.Status = "Processed";

                    var log = AuditLogFactory.CreateAuditLog(
                  correlationId: guid,
                  sourceSystem: "ConsentManagerAPI",
                  targetSystem: "CoreBankAPI",
                  endpoint: endPointUrl,
                  requestPayload: requestJson,
                  responsePayload: responseJson,
                  statusCode: "200",
                  requestType: MessageTypeMappings.GetPaymentLog,
                  executionTimeMs: (int)stopwatch.ElapsedMilliseconds
              );

                    await _sendPointInitialize.AuditLog!.Send(log);
                }
                else
                {

                    cbRequestDto.Status = "Failure";

                    stopwatch.Stop();

                    var log = AuditLogFactory.CreateAuditLog(
                        correlationId: guid,
                        sourceSystem: "ConsentManagerAPI",
                        targetSystem: "CoreBankAPI",
                        endpoint: endPointUrl,
                        requestPayload: requestJson,
                        responsePayload: string.Empty,
                        statusCode: "400",
                        requestType: MessageTypeMappings.GetPaymentLog,
                        executionTimeMs: (int)stopwatch.ElapsedMilliseconds
                    );

                    await _sendPointInitialize.AuditLog!.Send(log);

                    _logger.Info("------------------------------------------------------------------------");
                    _logger.Info("RevokeConsentbyConsentGroupId completed with failure.");

                    return NotFound(new ErrorResponse
                    {
                        errorCode = "404",
                        errorMessage = "Consent.Invalid"
                    });
                }

                await _sendPointInitialize.RevokeConsentGroupIdRequest!.Send(cbRequestDto);
                stopwatch.Stop();

                _logger.Info("------------------------------------------------------------------------");
                _logger.Info("RevokeConsentbyConsentGroupId completed successfully.");

                return NoContent(); // ✅ clean return
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                var message = "Internal Server Error";
                return BadRequest(message);
            }
        }


        [HttpPost]
        [Route("/consents/{consentId}/action/revoke")]
        public async Task<IActionResult> RevokeConsentbyConsentId([FromQuery(Name = "consentId")][Required()] string consentId, [FromBody] CbRevokeConsent Request)
        {

            var stopwatch = Stopwatch.StartNew();

            string requestJson = string.Empty;
            string responseJson = string.Empty;
            string endPointUrl = string.Empty;
            Guid guid = Guid.Empty;
            try
            {
                _logger.Info("RevokeConsentbyConsentId invoked.");
                _logger.Info("------------------------------------------------------------------------");
                endPointUrl = UrlHelper.CombineUrl(_backEndApis.Value.BaseUrl!, _backEndApis.Value.ConsentManagementUrl!.RevokeConsentbyConsentId!);

                if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
                        !Guid.TryParse(corrIdObj?.ToString(), out guid))
                {
                    _logger.Info($"Invalid CorrelationId: {corrIdObj}");
                    return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
                }

                CbsConsentRevokedDto cbRequestDto = new()
                {
                    CorrelationId = guid,
                    PsuUserId = Request.RevokedByPsu.UserId,
                    Revokedby = Request.RevokedBy,
                    ConsentGroupId = null,
                    ConsentId = consentId

                };

                requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);

                _logger.Info($"Request received:\n{requestJson}");

                var apiResult = await _service.RevokeConsentbyConsentGroupid(cbRequestDto, _logger.Log);

                if (apiResult == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        errorCode = "404",
                        errorMessage = "Payment data not found."
                    });
                }


                if (apiResult is NoContentResult)
                {
                    cbRequestDto.Status = "Processed";
                    var log = AuditLogFactory.CreateAuditLog(
                  correlationId: guid,
                  sourceSystem: "ConsentManagerAPI",
                  targetSystem: "CoreBankAPI",
                  endpoint: endPointUrl,
                  requestPayload: requestJson,
                  responsePayload: responseJson,
                  statusCode: "200",
                  requestType: MessageTypeMappings.GetPaymentLog,
                  executionTimeMs: (int)stopwatch.ElapsedMilliseconds
              );

                    await _sendPointInitialize.AuditLog!.Send(log);
                }
                else
                {

                    cbRequestDto.Status = "Failure";
                    var log = AuditLogFactory.CreateAuditLog(
                        correlationId: guid,
                        sourceSystem: "ConsentManagerAPI",
                        targetSystem: "CoreBankAPI",
                        endpoint: endPointUrl,
                        requestPayload: requestJson,
                        responsePayload: string.Empty,
                        statusCode: "400",
                        requestType: MessageTypeMappings.GetPaymentLog,
                        executionTimeMs: (int)stopwatch.ElapsedMilliseconds
                    );

                    await _sendPointInitialize.AuditLog!.Send(log);

                    _logger.Info("------------------------------------------------------------------------");
                    _logger.Info("RevokeConsentbyConsentId completed with failure.");

                    return NotFound(new ErrorResponse
                    {
                        errorCode = "404",
                        errorMessage = "Consent.Invalid"
                    });
                }

                await _sendPointInitialize.RevokeConsentIdRequest!.Send(cbRequestDto);
                stopwatch.Stop();

                _logger.Info("------------------------------------------------------------------------");
                _logger.Info("RevokeConsentbyConsentId completed successfully.");

                return NoContent(); // ✅ clean return
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                var message = "Internal Server Error";
                return BadRequest(message);
            }
        }
    }
}
