using Newtonsoft.Json.Linq;
using OF.ConsentManagement.CentralBankConn.API.Services;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.CentralBank.ResourceLog;
using OF.ConsentManagement.Model.Common;
using System.Diagnostics;

namespace ConsentManagerService.Controllers
{
    public class ResourceLogApiController : Controller
    {
        private readonly IResourceLogService _service;
        private readonly ResourceLogApiLogger _logger;
        
        

        public ResourceLogApiController(IResourceLogService service, ResourceLogApiLogger logger)
        {
            _service = service;
            _logger = logger;
        }


        [HttpPatch]
        [Route("/fx-quote-log/{logId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> PatchFxQuoteLog([FromQuery] string logId, [FromBody] CbPatchAccountOpeningLogRequest cbPatchAccountOpeningLogRequest)
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
                   
                    responseJson = "FxQuote Status updated successfully.";
                    

                    _logger.Info("------------------------------------------------------------------------");
                    _logger.Info("PatchFxQuoteLog completed successfully.");

                    return Ok(204);
                }
                else
                {
                  
                    _logger.Info("------------------------------------------------------------------------");
                    _logger.Info("PatchFxQuoteLog completed with failure.");


                    return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Consent.Invalid" ?? "Payment data not found." });

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, new ErrorResponse { errorCode = "500", errorMessage = "An unexpected error occurred." });
            }
        }


        //[HttpPatch]
        //[Route("/insurance-quote-log/{logId}")]
        //[Consumes("application/json")]
        //public async Task<IActionResult> PatchInsuranceQuoteLog([FromBody] CbPatchFxQuoteLogRequest cbPatchFxQuoteLogRequest)
        //{
        //    var stopwatch = Stopwatch.StartNew();

        //    string requestJson = string.Empty;
        //    string responseJson = string.Empty;
        //    string endPointUrl = string.Empty;
        //    Guid guid = Guid.Empty;

        //    try
        //    {
        //        _logger.Info("PatchInsuranceQuoteLog invoked.");
        //        _logger.Info("------------------------------------------------------------------------");

        //        endPointUrl = UrlHelper.CombineUrl(_backEndApis.Value.BaseUrl!, _backEndApis.Value.ConsentManagerServiceUrl!.UpdateInsuranceQuoteLogByLogId!);

        //        if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
        //               !Guid.TryParse(corrIdObj?.ToString(), out guid))
        //        {
        //            _logger.Info($"Invalid CorrelationId: {corrIdObj}");
        //            return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
        //        }
        //        CbPatchResourceLogInsuranceQuoteRecordRequestDto cbRequestDto = new()
        //        {
        //            CorrelationId = guid,
        //            LogId = logId,
        //            patchResourceLogInsuranceQuoteRecordBody = cbPatchResourceLogInsuranceQuoteRecordBody

        //        };
        //        requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);

        //        _logger.Info($"Request received:\n{requestJson}");

        //        await _sendPointInitialize.UpdateInsuranceQuoteLogRequest!.Send(cbRequestDto);
        //        var apiResult = await _service.PatchInsuranceQuoteLogResponse(logId, cbPatchResourceLogInsuranceQuoteRecordBody, _logger.Log);

        //        if (apiResult == null)
        //        {
        //            return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Payment data not found." });
        //        }
        //        CbPatchResourceLogInsuranceQuoteRecordResponseDto cbResponseDto = new();
        //        cbResponseDto.CorrelationId = guid;

        //        if (apiResult != null)
        //        {
        //            cbResponseDto.status = "PROCESSED";
        //            await _sendPointInitialize.UpdateInsuranceQuoteLogResponse!.Send(cbResponseDto);

        //            _logger.Info("Sending response:\n" + "InsuranceQuote Status updated successfully.");

        //            responseJson = "InsuranceQuote Status updated successfully.";
        //            stopwatch.Stop();

        //            var log = AuditLogFactory.CreateAuditLog(
        //               correlationId: guid,
        //               sourceSystem: "ConsentManagerAPI",
        //               targetSystem: "CoreBankAPI",
        //               endpoint: endPointUrl,
        //               requestPayload: requestJson,
        //               responsePayload: responseJson,
        //               statusCode: "200",
        //               requestType: MessageTypeMappings.PatchResourceLogInsuranceQuote,
        //               executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

        //            await _sendPointInitialize.AuditLog!.Send(log);

        //            _logger.Info("------------------------------------------------------------------------");
        //            _logger.Info("PatchInsuranceQuoteLog completed successfully.");

        //            return Ok();
        //        }
        //        else
        //        {
        //            cbResponseDto.status = "FAILED";
        //             await _sendPointInitialize.UpdateInsuranceQuoteLogResponse!.Send(cbResponseDto);

        //            var log = AuditLogFactory.CreateAuditLog(
        //               correlationId: guid,
        //               sourceSystem: "ConsentManagerAPI",
        //               targetSystem: "CoreBankAPI",
        //               endpoint: endPointUrl,
        //               requestPayload: requestJson,
        //               responsePayload: string.Empty,
        //               statusCode: "200",
        //               requestType: MessageTypeMappings.PatchResourceLogInsuranceQuote,
        //               executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

        //            await _sendPointInitialize.AuditLog!.Send(log);
        //            _logger.Info("------------------------------------------------------------------------");
        //            _logger.Info("PatchInsuranceQuoteLog completed with failure.");


        //            return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Consent.Invalid" ?? "Payment data not found." });

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex);

        //        var log = AuditLogFactory.CreateAuditLog(correlationId: guid,
        //            sourceSystem: "ConsentManagerAPI",
        //            targetSystem: "CoreBankAPI",
        //            endpoint: endPointUrl,
        //            requestPayload: requestJson,
        //            responsePayload: null,
        //            statusCode: "500",
        //            requestType: MessageTypeMappings.PatchResourceLogInsuranceQuote,
        //        executionTimeMs: (int)stopwatch.ElapsedMilliseconds,
        //            errorMessage: ex.Message);

        //        await _sendPointInitialize.AuditLog!.Send(log);
        //        _logger.Info("------------------------------------------------------------------------");
        //        _logger.Info("PatchInsuranceQuoteLog completed with error.");

        //        return StatusCode(500, new ErrorResponse { errorCode = "500", errorMessage = "An unexpected error occurred." });
        //    }
        //}


        //[HttpPatch]
        //[Route("/account-opening-log/{logId}")]
        //[Consumes("application/json")]
        //public async Task<IActionResult> PatchAccountOpeningLog([FromBody] CbPatchInsuranceQuoteLogRequest cbPatchInsuranceQuoteLogRequest)
        //{
        //    var stopwatch = Stopwatch.StartNew();

        //    string requestJson = string.Empty;
        //    string responseJson = string.Empty;
        //    string endPointUrl = string.Empty;
        //    Guid guid = Guid.Empty;

        //    try
        //    {
        //        _logger.Info("PatchAccountOpeningLog invoked.");
        //        _logger.Info("------------------------------------------------------------------------");

        //        endPointUrl = UrlHelper.CombineUrl(_backEndApis.Value.BaseUrl!, _backEndApis.Value.ConsentManagerServiceUrl!.UpdateAccountOpeningLogByLogId!);

        //        if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
        //               !Guid.TryParse(corrIdObj?.ToString(), out guid))
        //        {
        //            _logger.Info($"Invalid CorrelationId: {corrIdObj}");
        //            return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
        //        }

        //        CbPatchResourceLogAccountOpeningRecordRequestDto cbRequestDto = new()
        //        {
        //            CorrelationId = guid,
        //            LogId = logId,
        //            patchResourceLogAccountOpeningRecordBody = cbPatchResourceLogAccountOpeningRecordBody

        //        };
        //        requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);

        //        _logger.Info($"Request received:\n{requestJson}");

        //        // await _sendPointInitialize.GetFxQuoteLogRequest!.Send(cbRequestDto);
        //        var apiResult = await _service.PatchAccountOpeningLogResponse(logId, cbPatchResourceLogAccountOpeningRecordBody, _logger.Log);

        //        if (apiResult == null)
        //        {
        //            return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Payment data not found." });
        //        }
        //        CbPatchResourceLogAccountOpeningRecordResponseDto cbResponseDto = new();
        //        cbResponseDto.CorrelationId = guid;

        //        if (apiResult != null)
        //        {
        //            cbResponseDto.status = "PROCESSED";
        //            // await _sendPointInitialize.GetFxQuoteLogResponse!.Send(cbResponseDto);

        //            _logger.Info("Sending response:\n" + "AccountOpening Status updated successfully.");

        //            responseJson = "AccountOpening Status updated successfully.";
        //            stopwatch.Stop();

        //            var log = AuditLogFactory.CreateAuditLog(
        //               correlationId: guid,
        //               sourceSystem: "ConsentManagerAPI",
        //               targetSystem: "CoreBankAPI",
        //               endpoint: endPointUrl,
        //               requestPayload: requestJson,
        //               responsePayload: responseJson,
        //               statusCode: "200",
        //               requestType: MessageTypeMappings.PatchResourceLogAccountOpening,
        //               executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

        //            await _sendPointInitialize.AuditLog!.Send(log);

        //            _logger.Info("------------------------------------------------------------------------");
        //            _logger.Info("PatchAccountOpeningLog completed successfully.");

        //            return Ok();
        //        }
        //        else
        //        {
        //            cbResponseDto.status = "FAILED";
        //            // await _sendPointInitialize.GetFxQuoteLogResponse!.Send(cbResponseDto);

        //            var log = AuditLogFactory.CreateAuditLog(
        //               correlationId: guid,
        //               sourceSystem: "ConsentManagerAPI",
        //               targetSystem: "CoreBankAPI",
        //               endpoint: endPointUrl,
        //               requestPayload: requestJson,
        //               responsePayload: string.Empty,
        //               statusCode: "200",
        //               requestType: MessageTypeMappings.PatchResourceLogAccountOpening,
        //               executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

        //            await _sendPointInitialize.AuditLog!.Send(log);
        //            _logger.Info("------------------------------------------------------------------------");
        //            _logger.Info("PatchAccountOpeningLog completed with failure.");


        //            return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Consent.Invalid" ?? "Payment data not found." });

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex);

        //        var log = AuditLogFactory.CreateAuditLog(correlationId: guid,
        //            sourceSystem: "ConsentManagerAPI",
        //            targetSystem: "CoreBankAPI",
        //            endpoint: endPointUrl,
        //            requestPayload: requestJson,
        //            responsePayload: null,
        //            statusCode: "500",
        //            requestType: MessageTypeMappings.PatchResourceLogAccountOpening,
        //        executionTimeMs: (int)stopwatch.ElapsedMilliseconds,
        //            errorMessage: ex.Message);

        //        await _sendPointInitialize.AuditLog!.Send(log);
        //        _logger.Info("------------------------------------------------------------------------");
        //        _logger.Info("PatchAccountOpeningLog completed with error.");

        //        return StatusCode(500, new ErrorResponse { errorCode = "500", errorMessage = "An unexpected error occurred." });
        //    }

        //}


    }
}
