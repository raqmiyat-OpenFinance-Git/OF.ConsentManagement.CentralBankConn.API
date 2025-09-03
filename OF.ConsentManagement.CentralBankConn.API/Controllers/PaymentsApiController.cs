using ConsentManagerCommon.NLog;
using ConsentManagerService.Services;
using ConsentMangerModel.Consent;
using Newtonsoft.Json.Linq;
using OF.ConsentManagement.CentralBankConn.API.Model;
using OF.ConsentManagement.Common.AES;
using OF.ConsentManagement.Common.Helpers;
using OF.ConsentManagement.Model.Common;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ConsentManagerService.Controllers
{
    public class PaymentsApiController : Controller
    {
        private readonly IPaymentsService _service;
        private readonly PaymentsApiLogger _logger;
        private readonly SendPointInitialize _sendPointInitialize;
        private readonly IOptions<ServiceParams> _serviceParams;
        private readonly IOptions<CoreBankApis> _backEndApis;
        private readonly IOptions<SecurityParameters> _securityParameters;

        public PaymentsApiController(IPaymentsService service, PaymentsApiLogger logger, SendPointInitialize sendPointInitialize, IOptions<ServiceParams> serviceParams, IOptions<CoreBankApis> backEndApis, IOptions<SecurityParameters> securityParameters)
        {
            _service = service;
            _logger = logger;
            _sendPointInitialize = sendPointInitialize;
            _serviceParams = serviceParams;
            _backEndApis = backEndApis;
            _securityParameters = securityParameters;
        }

        [HttpGet]
        [Route("/payment-log")]
        public async Task<IActionResult> GetAuditConsentsByConsentIdw([FromQuery(Name = "consentId")][Required()] string consentId, [FromQuery(Name = "accountId")][Required()] string accountId)
        {           
            var stopwatch = Stopwatch.StartNew(); 

            string requestJson = string.Empty;
            string responseJson = string.Empty;
            string endPointUrl = string.Empty;
            Guid guid = Guid.Empty;
            try
            {
                _logger.Info("GetAuditConsentsByConsentIdw invoked.");
                _logger.Info("------------------------------------------------------------------------");
                endPointUrl = UrlHelper.CombineUrl(_backEndApis.Value.BaseUrl!, _backEndApis.Value.ConsentManagementUrl!.GetPaymentLog!);

                if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
                        !Guid.TryParse(corrIdObj?.ToString(), out guid))
                {   
                    _logger.Info($"Invalid CorrelationId: {corrIdObj}");
                    return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
                }

                CbGetAuditConsentByConsentIdRequestDto cbRequestDto = new()
                {
                    CorrelationId = guid,
                    ConsentId = consentId,
                    AccountId = accountId
                };

                requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);

                _logger.Info($"Request received:\n{requestJson}");

                await _sendPointInitialize.GetPaymentLogRequest!.Send(cbRequestDto);

                 // call internal API

                var apiResult = await _service.GetAuditConsentResponse(consentId, accountId, _logger.Log);

                if (apiResult == null)
                {
                    return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Payment data not found." });
                }

                CbGetAuditConsentByConsentIdResponseDto cbResponseDto = new();
                GetAuditConsentsByConsentIdResponse cbResponse;
                cbResponseDto.CorrelationId = guid;

                if (apiResult.data != null)
                {
                    cbResponseDto.auditConsentsByConsentIdResponse = apiResult;
                    cbResponseDto.status = "PROCESSED";

                    await _sendPointInitialize.GetPaymentLogResponse!.Send(cbResponseDto);

                    cbResponse = apiResult;

                    var response = await GetResponseObject(cbResponse);

                    _logger.Info("Sending response:\n" + response.ToString());

                    responseJson = response.ToString();
                    stopwatch.Stop();

                    var log = AuditLogFactory.CreateAuditLog(
                        correlationId: guid,
                        sourceSystem: "ConsentManagerAPI",
                        targetSystem: "CoreBankAPI",
                        endpoint: endPointUrl,
                        requestPayload: requestJson,
                        responsePayload: responseJson,
                        statusCode: "200",
                        requestType: MessageTypeMappings.GetPaymentLog,
                        executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                    await _sendPointInitialize.AuditLog!.Send(log);

                    _logger.Info("------------------------------------------------------------------------");
                    _logger.Info("GetAuditConsentsByConsentIdw completed successfully.");

                    return Ok(cbResponse);
                }
                else
                {
                    cbResponseDto.auditConsentsByConsentIdResponse = new GetAuditConsentsByConsentIdResponse();
                    cbResponseDto.status = "FAILED";
                    await _sendPointInitialize.GetPaymentLogResponse!.Send(cbResponseDto);

                    var log = AuditLogFactory.CreateAuditLog(
                       correlationId: guid,
                       sourceSystem: "ConsentManagerAPI",
                       targetSystem: "CoreBankAPI",
                       endpoint: endPointUrl,
                       requestPayload: requestJson,
                       responsePayload: string.Empty,
                       statusCode: "200",
                       requestType: MessageTypeMappings.GetPaymentLog,
                       executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                    await _sendPointInitialize.AuditLog!.Send(log);
                    _logger.Info("------------------------------------------------------------------------");
                    _logger.Info("GetAuditConsentsByConsentIdw completed with failure.");


                    return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Consent.Invalid" ?? "Payment data not found." });

                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex);

                var log = AuditLogFactory.CreateAuditLog(correlationId: guid,
                    sourceSystem: "ConsentManagerAPI",
                    targetSystem: "CoreBankAPI",
                    endpoint: endPointUrl,
                    requestPayload: requestJson,
                    responsePayload: null,
                    statusCode: "500",
                    requestType: MessageTypeMappings.GetPaymentLog,
                executionTimeMs: (int)stopwatch.ElapsedMilliseconds,
                    errorMessage: ex.Message);

                await _sendPointInitialize.AuditLog!.Send(log);
                _logger.Info("------------------------------------------------------------------------");
                _logger.Info("GetAuditConsentsByConsentIdw completed with error.");

                return StatusCode(500, new ErrorResponse { errorCode = "500", errorMessage = "An unexpected error occurred." });

            }
        }

        [HttpPatch]
        [Route("/payment-log/{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> PatchPaymentLog([FromRoute(Name = "id")][Required] string id, [FromBody] PatchPaymentRecordBody cbPatchPaymentRecordBody)
        {
            var stopwatch = Stopwatch.StartNew();

            string requestJson = string.Empty;
            string responseJson = string.Empty;
            string endPointUrl = string.Empty;
            Guid guid = Guid.Empty;
            try
            {
                _logger.Info("PatchPaymentLog invoked.");
                _logger.Info("------------------------------------------------------------------------");
                endPointUrl = UrlHelper.CombineUrl(_backEndApis.Value.BaseUrl!, _backEndApis.Value.ConsentManagementUrl!.UpdatePaymentLogById!);

                if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
                        !Guid.TryParse(corrIdObj?.ToString(), out guid))
                {
                    _logger.Info($"Invalid CorrelationId: {corrIdObj}");
                    return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
                }

                CbPatchPaymentRecordRequestDto cbRequestDto = new()
                {
                    CorrelationId = guid,
                    Id = id,
                    paymentRecordBody = cbPatchPaymentRecordBody
                };

                requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);

                _logger.Info($"Request received:\n{requestJson}");

                await _sendPointInitialize.GetPaymentLogRequest!.Send(cbRequestDto);

                // call internal API

                var apiResult = await _service.PatchPaymentLogResponse(id, cbPatchPaymentRecordBody, _logger.Log);

                if (apiResult == null)
                {
                    return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Payment data not found." });
                }

                CbPatchPaymentRecordResponseDto cbResponseDto = new();
                cbResponseDto.CorrelationId = guid;

                if (apiResult != null)
                {
                    cbResponseDto.status = "PROCESSED";

                    await _sendPointInitialize.GetPaymentLogResponse!.Send(cbResponseDto);

                    _logger.Info("Sending response:\n" + "Payment Status updated successfully.");

                    responseJson = "Payment Status updated successfully.";
                    stopwatch.Stop();

                    var log = AuditLogFactory.CreateAuditLog(
                        correlationId: guid,
                        sourceSystem: "ConsentManagerAPI",
                        targetSystem: "CoreBankAPI",
                        endpoint: endPointUrl,
                        requestPayload: requestJson,
                        responsePayload: responseJson,
                        statusCode: "200",
                        requestType: MessageTypeMappings.GetPaymentLog,
                        executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                    await _sendPointInitialize.AuditLog!.Send(log);

                    _logger.Info("------------------------------------------------------------------------");
                    _logger.Info("PatchPaymentLog completed successfully.");

                    return Ok();
                }
                else
                {
                    cbResponseDto.status = "FAILED";
                    await _sendPointInitialize.GetPaymentLogResponse!.Send(cbResponseDto);

                    var log = AuditLogFactory.CreateAuditLog(
                       correlationId: guid,
                       sourceSystem: "ConsentManagerAPI",
                       targetSystem: "CoreBankAPI",
                       endpoint: endPointUrl,
                       requestPayload: requestJson,
                       responsePayload: string.Empty,
                       statusCode: "200",
                       requestType: MessageTypeMappings.GetPaymentLog,
                       executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                    await _sendPointInitialize.AuditLog!.Send(log);
                    _logger.Info("------------------------------------------------------------------------");
                    _logger.Info("PatchPaymentLog completed with failure.");


                    return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Consent.Invalid" ?? "Payment data not found." });

                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex);

                var log = AuditLogFactory.CreateAuditLog(correlationId: guid,
                    sourceSystem: "ConsentManagerAPI",
                    targetSystem: "CoreBankAPI",
                    endpoint: endPointUrl,
                    requestPayload: requestJson,
                    responsePayload: null,
                    statusCode: "500",
                    requestType: MessageTypeMappings.GetPaymentLog,
                executionTimeMs: (int)stopwatch.ElapsedMilliseconds,
                    errorMessage: ex.Message);

                await _sendPointInitialize.AuditLog!.Send(log);
                _logger.Info("------------------------------------------------------------------------");
                _logger.Info("PatchPaymentLog completed with error.");

                return StatusCode(500, new ErrorResponse { errorCode = "500", errorMessage = "An unexpected error occurred." });

            }
        }

        private Task<JObject> GetResponseObject<T>(T responseObject)
        {
            try
            {
                var response = JObject.FromObject(responseObject!, new Newtonsoft.Json.JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented
                });
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred in GetResponseObject()");
                return Task.FromResult(new JObject());
            }
        }

    }
}
