using OF.ConsentManagement.CentralBankConn.API.Model;
using OF.ConsentManagement.CentralBankConn.API.Services;
using OF.ConsentManagement.Common.Helpers;
using OF.ConsentManagement.Model.CentralBank.ResourceLog;
using System.Diagnostics;

namespace ConsentManagerService.Controllers
{
    public class ResourceLogApiController : Controller
    {
        private readonly IResourceLogService _service;
        private readonly ResourceLogApiLogger _logger;
        private readonly SendPointInitialize _sendpoint;

        public ResourceLogApiController(IResourceLogService service, SendPointInitialize sendpoint, ResourceLogApiLogger logger)
        {
            _service = service;
            _sendpoint = sendpoint;
            _logger = logger;
        }

        [HttpPatch]
        [Route("/account-opening-log/{logId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> PatchAccountOpeningLog([FromRoute] string logId, [FromBody] CbPatchAccountOpeningLogRequest cbPatchAccountOpeningLogRequest)
        {
            var stopwatch = Stopwatch.StartNew();

            string requestJson = string.Empty;
            string responseJson = string.Empty;
            string endPointUrl = string.Empty;
            Guid guid = Guid.Empty;

            try
            {
                _logger.Info("PatchAccountOpeningLog invoked.");
                _logger.Info("------------------------------------------------------------------------");


                if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
                       !Guid.TryParse(corrIdObj?.ToString(), out guid))
                {
                    _logger.Info($"Invalid CorrelationId: {corrIdObj}");
                    return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
                }
                CbPatchAccountOpeningLogRequestDto cbRequestDto = new()
                {
                    CorrelationId = guid,
                    LogId = logId,
                    cbPatchAccountOpeningLogRequest = cbPatchAccountOpeningLogRequest

                };
                requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);

                _logger.Info($"Request received:\n{requestJson}");

                bool result = await _service.UpdateAccountOpeningLog(cbRequestDto, _logger.Log);


                if (result)
                {
                    responseJson = "AccountOpening Status updated successfully.";

                    stopwatch.Stop();

                    var log = AuditLogFactory.CreateAuditLog(
                        correlationId: guid,
                        sourceSystem: "CentralBankAPI",
                        targetSystem: "ResourceLogService",
                        endpoint: endPointUrl,
                        requestPayload: requestJson,
                        responsePayload: responseJson,
                        statusCode: "204",
                        requestType: "UpdateAccountOpeningLog",
                        executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                    await _sendpoint.AuditLog!.Send(log);


                    _logger.Info("------------------------------------------------------------------------");
                    _logger.Info("PatchAccountOpeningLog completed successfully.");

                    return Ok(204);
                }
                else
                {
                    _logger.Info("------------------------------------------------------------------------");
                    responseJson = JsonConvert.SerializeObject(new { errorCode = "404", errorMessage = "Consent.Invalid" });

                    var log = AuditLogFactory.CreateAuditLog(
                        correlationId: guid,
                        sourceSystem: "CentralBankAPI",
                        targetSystem: "ResourceLogService",
                        endpoint: endPointUrl,
                        requestPayload: requestJson,
                        responsePayload: responseJson,
                        statusCode: "404",
                        requestType: "UpdateAccountOpeningLog",
                        executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                    await _sendpoint.AuditLog!.Send(log);

                    _logger.Info("PatchAccountOpeningLog completed with failure.");

                    return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Consent.Invalid" ?? "Payment data not found." });
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.Error(ex);

                responseJson = JsonConvert.SerializeObject(new { errorCode = "500", errorMessage = "An unexpected error occurred." });

                var log = AuditLogFactory.CreateAuditLog(
                    correlationId: Guid.Empty, // could use guid if parsed
                    sourceSystem: "CentralBankAPI",
                    targetSystem: "ResourceLogService",
                    endpoint: endPointUrl,
                    requestPayload: requestJson,
                    responsePayload: responseJson,
                    statusCode: "500",
                    requestType: "UpdateAccountOpeningLog",
                    executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                await _sendpoint.AuditLog!.Send(log);

                return StatusCode(500, new ErrorResponse { errorCode = "500", errorMessage = "An unexpected error occurred." });
            }
        }

        [HttpPatch]
        [Route("/fx-quote-log/{logId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> PatchFxQuoteLog([FromRoute] string logId, [FromBody] CbPatchFxQuoteLogRequest cbPatchFxQuoteLogRequest)
        {
            var stopwatch = Stopwatch.StartNew();

            string requestJson = string.Empty;
            string responseJson = string.Empty;
            string endPointUrl = string.Empty;
            Guid guid = Guid.Empty;

            try
            {
                _logger.Info("PatchFxQuoteLog invoked.");
                _logger.Info("------------------------------------------------------------------------");

                if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
                       !Guid.TryParse(corrIdObj?.ToString(), out guid))
                {
                    _logger.Info($"Invalid CorrelationId: {corrIdObj}");
                    return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
                }
                CbPatchFxQuoteLogRequestDto cbRequestDto = new()
                {
                    CorrelationId = guid,
                    LogId = logId,
                    cbPatchFxQuoteLogRequest = cbPatchFxQuoteLogRequest

                };
                requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);

                _logger.Info($"Request received:\n{requestJson}");

                bool result = await _service.UpdateFxQuoteLog(cbRequestDto, _logger.Log);

                if (result)
                {
                    responseJson = "FxQuote Status updated successfully.";

                    var log = AuditLogFactory.CreateAuditLog(
                         correlationId: guid,
                         sourceSystem: "CentralBankAPI",
                         targetSystem: "ResourceLogService",
                         endpoint: endPointUrl,
                         requestPayload: requestJson,
                         responsePayload: responseJson,
                         statusCode: "204",
                         requestType: "UpdateFxQuoteLog",
                         executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                    await _sendpoint.AuditLog!.Send(log);


                    _logger.Info("------------------------------------------------------------------------");
                    _logger.Info("PatchFxQuoteLog completed successfully.");

                    return Ok(204);
                }
                else
                {
                    _logger.Info("------------------------------------------------------------------------");
                    responseJson = JsonConvert.SerializeObject(new { errorCode = "404", errorMessage = "Consent.Invalid" });

                    var log = AuditLogFactory.CreateAuditLog(
                        correlationId: guid,
                        sourceSystem: "CentralBankAPI",
                        targetSystem: "ResourceLogService",
                        endpoint: endPointUrl,
                        requestPayload: requestJson,
                        responsePayload: responseJson,
                        statusCode: "404",
                        requestType: "UpdateFxQuoteLog",
                        executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                    await _sendpoint.AuditLog!.Send(log);

                    _logger.Info("PatchFxQuoteLog completed with failure.");

                    return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Consent.Invalid" ?? "Payment data not found." });
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.Error(ex);
                responseJson = JsonConvert.SerializeObject(new { errorCode = "500", errorMessage = "An unexpected error occurred." });

                var log = AuditLogFactory.CreateAuditLog(
                    correlationId: Guid.Empty, // could use guid if parsed
                    sourceSystem: "CentralBankAPI",
                    targetSystem: "ResourceLogService",
                    endpoint: endPointUrl,
                    requestPayload: requestJson,
                    responsePayload: responseJson,
                    statusCode: "500",
                    requestType: "UpdateFxQuoteLog",
                    executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                await _sendpoint.AuditLog!.Send(log);

                return StatusCode(500, new ErrorResponse { errorCode = "500", errorMessage = "An unexpected error occurred." });
            }

        }

        [HttpPatch]
        [Route("/insurance-quote-log/{logId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> PatchInsuranceQuoteLog([FromRoute] string logId, [FromBody] CbPatchInsuranceQuoteLogRequest cbPatchInsuranceQuoteLogRequest)
        {
            var stopwatch = Stopwatch.StartNew();

            string requestJson = string.Empty;
            string responseJson = string.Empty;
            string endPointUrl = string.Empty;
            Guid guid = Guid.Empty;

            try
            {
                _logger.Info("PatchInsuranceQuoteLog invoked.");
                _logger.Info("------------------------------------------------------------------------");


                if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
                       !Guid.TryParse(corrIdObj?.ToString(), out guid))
                {
                    _logger.Info($"Invalid CorrelationId: {corrIdObj}");
                    return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
                }
                CbPatchInsuranceQuoteLogRequestDto cbRequestDto = new()
                {
                    CorrelationId = guid,
                    LogId = logId,
                    cbPatchInsuranceQuoteLogRequest = cbPatchInsuranceQuoteLogRequest

                };
                requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);

                _logger.Info($"Request received:\n{requestJson}");

                bool result = await _service.UpdateInsuranceQuoteLog(cbRequestDto, _logger.Log);

                stopwatch.Stop();

                if (result)
                {
                    responseJson = "InsuranceQuote Status updated successfully.";

                    var log = AuditLogFactory.CreateAuditLog(
                         correlationId: guid,
                         sourceSystem: "CentralBankAPI",
                         targetSystem: "ResourceLogService",
                         endpoint: endPointUrl,
                         requestPayload: requestJson,
                         responsePayload: responseJson,
                         statusCode: "204",
                         requestType: "UpdateInsuranceQuoteLog",
                         executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                    await _sendpoint.AuditLog!.Send(log);

                    _logger.Info("------------------------------------------------------------------------");
                    _logger.Info("PatchInsuranceQuoteLog completed successfully.");

                    return Ok(204);
                }
                else
                {
                    _logger.Info("------------------------------------------------------------------------");

                    responseJson = JsonConvert.SerializeObject(new { errorCode = "404", errorMessage = "Consent.Invalid" });

                    var log = AuditLogFactory.CreateAuditLog(
                        correlationId: guid,
                        sourceSystem: "CentralBankAPI",
                        targetSystem: "ResourceLogService",
                        endpoint: endPointUrl,
                        requestPayload: requestJson,
                        responsePayload: responseJson,
                        statusCode: "404",
                        requestType: "UpdateInsuranceQuoteLog",
                        executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                    await _sendpoint.AuditLog!.Send(log);

                    _logger.Info("PatchInsuranceQuoteLog completed with failure.");

                    return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Consent.Invalid" ?? "Payment data not found." });
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Error(ex);

                responseJson = JsonConvert.SerializeObject(new { errorCode = "500", errorMessage = "An unexpected error occurred." });

                var log = AuditLogFactory.CreateAuditLog(
                    correlationId: Guid.Empty, // could use guid if parsed
                    sourceSystem: "CentralBankAPI",
                    targetSystem: "ResourceLogService",
                    endpoint: endPointUrl,
                    requestPayload: requestJson,
                    responsePayload: responseJson,
                    statusCode: "500",
                    requestType: "UpdateInsuranceQuoteLog",
                    executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                await _sendpoint.AuditLog!.Send(log);

                return StatusCode(500, new ErrorResponse { errorCode = "500", errorMessage = "An unexpected error occurred." });
            }
        }

    }
}
