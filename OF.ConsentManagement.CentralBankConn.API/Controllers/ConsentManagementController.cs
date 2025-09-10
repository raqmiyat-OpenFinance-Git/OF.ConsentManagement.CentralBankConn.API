using Newtonsoft.Json.Linq;
using OF.ConsentManagement.CentralBankConn.API.Model;
using OF.ConsentManagement.CentralBankConn.API.Services;
using OF.ConsentManagement.CentralBankConn.API.Stub;
using OF.ConsentManagement.Common.Helpers;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.CentralBank.Consent.GetQuery;
using OF.ConsentManagement.Model.CentralBank.Consent.GetRequestDto;
using OF.ConsentManagement.Model.CentralBank.Consent.GetResponse;
using OF.ConsentManagement.Model.CentralBank.Consent.GetResponseDto;
using OF.ConsentManagement.Model.CentralBank.Consent.PatchRequest;
using OF.ConsentManagement.Model.CentralBank.Consent.PostRequest;
using OF.ConsentManagement.Model.CentralBank.Consent.PostRequestDto;
using OF.ConsentManagement.Model.CentralBank.Consent.PostResponse;
using OF.ConsentManagement.Model.CentralBank.Consent.PostResponseDto;
using OF.ConsentManagement.Model.Common;
using OF.ConsentManagement.Model.CoreBank;
using System.Diagnostics;


namespace OF.ConsentManagement.CentralBankConn.API.Controllers;
[ApiController]

public class ConsentManagementController : ControllerBase
{
    private readonly IOptions<ServiceParams> _serviceParams;
    private readonly IConsentManagementService _consentservice;
    private readonly ConsentLogger _logger;
    private readonly IOptions<CoreBankApis> _coreBankApis;
    private readonly SendPointInitialize _sendpoint;
    private readonly HttpClient _client;

    public ConsentManagementController(IConsentManagementService service, IOptions<ServiceParams> serviceParams, ConsentLogger logger, SendPointInitialize sendpoint, IOptions<CoreBankApis> coreBankApis, HttpClient client)
    {
        _serviceParams = serviceParams;
        _consentservice = service;
        _logger = logger;
        _coreBankApis = coreBankApis;
        _sendpoint = sendpoint;
        _client = client;
    }

    [HttpPost]
    [Route("/consents")]
    [ProducesResponseType(typeof(CbPostConsentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateConsent(
        [FromBody] CbPostConsentRequest cbRequest)
    {
        var stopwatch = Stopwatch.StartNew();
        string requestJson;
        string responseJson = string.Empty;
        var endPointUrl = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.PostConsent!);
        Guid guid = Guid.Empty;

        if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
                    !Guid.TryParse(corrIdObj?.ToString(), out guid))
        {
            _logger.Info($"Invalid CorrelationId: {corrIdObj}");
            return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
        }

        CbPostConsentRequestDto cbRequestDto = new CbPostConsentRequestDto
        {
            CorrelationId = guid,

            cbPostConsentRequest = cbRequest

        };

        requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);
        _logger.Info($"Request received:\n{requestJson}");

        await _sendpoint.PostConsentRequest!.Send(cbRequestDto);

        var cbsRequest = new CbsPostConsentRequest
        {
            PaymentId = cbRequest.Consent.ConsentId,
            ConsentId = cbRequest.Consent.ConsentId,
            CorrelationId = guid,
        };

        var apiResult = await _consentservice.SendConsentToCoreBankAsync(cbsRequest, _logger.Log);

        if (apiResult == null)
            return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Payment data not found." });

        var cbResponseDto = new CbPostConsentResponseDto { CorrelationId = guid };

        if (apiResult.Success)
        {
            var cbResponse = ConsentPostResponseSample.GetSampleResponse();
            cbResponseDto.cbPostConsentResponse = cbResponse;
            cbResponseDto.status = "PROCESSED";
            await _sendpoint.PostConsentResponse!.Send(cbResponseDto);

            var response = await GetResponseObject(cbResponse);
            responseJson = response.ToString();
            stopwatch.Stop();

            var log = AuditLogFactory.CreateAuditLog(
                correlationId: guid,
                sourceSystem: "CentralBankAPI",
                targetSystem: "CoreBankAPI",
                endpoint: endPointUrl,
                requestPayload: requestJson,
                responsePayload: responseJson,
                statusCode: "200",
                requestType: MessageTypeMappings.CreateConsent,
                executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

            await _sendpoint.AuditLog!.Send(log);

            return Ok(cbResponse);
        }
        else
        {
            cbResponseDto.cbPostConsentResponse = new CbPostConsentResponse();
            cbResponseDto.status = "FAILED";
            await _sendpoint.PostConsentResponse!.Send(cbResponseDto);

            return NotFound(new ErrorResponse { errorCode = "404", errorMessage = apiResult.Message ?? "FxQuote data not found." });
        }
    }

    [HttpPatch("/consents/{consentId}")]
    public async Task<IActionResult> PatchConsent(
        string consentId,
        [FromBody] CbPatchConsentRequest cbRequest)
    {
        var stopwatch = Stopwatch.StartNew();
        string requestJson;
        string responseJson = string.Empty;
        var endPointUrl = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.UpdateConsent!);

        if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
            !Guid.TryParse(corrIdObj?.ToString(), out var guid))
        {
            _logger.Info($"Invalid CorrelationId: {corrIdObj}");
            return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
        }

        var cbRequestDto = new CbPatchConsentRequestDto
        {
            CorrelationId = guid,
            cbPatchConsentHeader = new OF.ConsentManagement.Model.CentralBank.Consent.PatchHeader.CbPatchConsentHeader() { CorrelationId = guid, ConsentId = consentId, RequestJson = "" },
            cbPatchConsentRequest = cbRequest,
        };

        requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);
        _logger.Info($"Request received:\n{requestJson}");

        await _sendpoint.PatchConsentRequest!.Send(cbRequestDto);

        var cbsRequest = new CbsPatchConsentRequest
        {
            PaymentId = consentId,
            ConsentId = consentId,
            CorrelationId = guid,
        };

        var apiResult = await _consentservice.SendConsentUpdateToCoreBankAsync(cbsRequest, _logger.Log);

        if (apiResult == null)
            return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Payment data not found." });

        var cbResponseDto = new CbPatchConsentResponseDto { CorrelationId = guid };

        if (apiResult.Success)
        {
            cbResponseDto.ConsentId = consentId;
            cbResponseDto.Status = "PROCESSED";
            await _sendpoint.PatchConsentResponse!.Send(cbResponseDto);

            stopwatch.Stop();

            var log = AuditLogFactory.CreateAuditLog(
                correlationId: guid,
                sourceSystem: "CentralBankAPI",
                targetSystem: "CoreBankAPI",
                endpoint: endPointUrl,
                requestPayload: requestJson,
                responsePayload: responseJson,
                statusCode: "200",
                requestType: MessageTypeMappings.UpdateConsent,
                executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

            await _sendpoint.AuditLog!.Send(log);
            return NoContent();

        }
        else
        {
            cbResponseDto.Status = "FAILED";
            await _sendpoint.PatchConsentResponse!.Send(cbResponseDto);

            return NotFound(new ErrorResponse { errorCode = "404", errorMessage = apiResult.Message ?? "FxQuote data not found." });
        }
    }

    [HttpGet("/consents")]
    public async Task<IActionResult> GetConsents(
    [FromQuery] long? updatedAt,       // optional (nullable long)
    [FromQuery] string consentType,    // optional
    [FromQuery] string status,         // optional
    [FromQuery] int page = 1,          // default to 1
    [FromQuery] int pageSize = 25      // default to 25
    )
    {
        var stopwatch = Stopwatch.StartNew();
        string requestJson;
        string responseJson = string.Empty;
        var endPointUrl = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.GetConsents!);

        if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
            !Guid.TryParse(corrIdObj?.ToString(), out var guid))
        {
            _logger.Info($"Invalid CorrelationId: {corrIdObj}");
            return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
        }

        var cbRequestDto = new CbGetConsentRequestDto
        {
            CorrelationId = guid,
            cbGetConsentQueryParameters = MapQueryParameterToCbGetConsentQueryParameter(updatedAt, consentType, status, page, pageSize, guid),
        };

        requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);
        _logger.Info($"Request received:\n{requestJson}");

        await _sendpoint.GetConsentRequest!.Send(cbRequestDto);

        var cbsRequest = new CbsGetConsentRequest
        {
            PaymentId = "123",
            ConsentId = "123",
            CorrelationId = guid,
        };

        var apiResult = await _consentservice.GetConsentsFromCoreBankAsync(cbsRequest, _logger.Log);

        if (apiResult == null)
            return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Payment data not found." });

        var cbResponseDto = new CbGetConsentResponseDto { CorrelationId = guid };

        if (apiResult.Success)
        {
            var cbResponse = ConsentGetResponseSample.GetSampleResponse(cbsRequest.ConsentId);
            cbResponseDto.cbGetConsentResponse = cbResponse;
            cbResponseDto.Status = "Accepted";

            await _sendpoint.GetConsentResponse!.Send(cbResponseDto);

            var response = await GetResponseObject(cbResponse);
            responseJson = response.ToString();
            stopwatch.Stop();

            var log = AuditLogFactory.CreateAuditLog(
                correlationId: guid,
                sourceSystem: "CentralBankAPI",
                targetSystem: "CoreBankAPI",
                endpoint: endPointUrl,
                requestPayload: requestJson,
                responsePayload: responseJson,
                statusCode: "200",
                requestType: MessageTypeMappings.RetrieveConsent,
                executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

            await _sendpoint.AuditLog!.Send(log);

            return Ok(cbResponse);
        }
        else
        {
            cbResponseDto.cbGetConsentResponse = new CbGetConsentResponse();
            cbResponseDto.Status = "FAILED";
            await _sendpoint.GetConsentResponse!.Send(cbResponseDto);

            return NotFound(new ErrorResponse { errorCode = "404", errorMessage = apiResult.Message ?? "FxQuote data not found." });
        }
    }


    [HttpGet("/consents/{consentId}")]
    public async Task<IActionResult> GetConsentById(string consentId)
    {
        var stopwatch = Stopwatch.StartNew();
        string requestJson;
        string responseJson = string.Empty;
        var endPointUrl = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.GetConsentById!);

        if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
            !Guid.TryParse(corrIdObj?.ToString(), out var guid))
        {
            _logger.Info($"Invalid CorrelationId: {corrIdObj}");
            return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
        }

        var cbRequestDto = new CbGetConsentRequestDto
        {
            CorrelationId = guid,
            ConsentId = consentId,
            cbGetConsentQueryParameters = new CbGetConsentQueryParameters(),
        };

        requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);
        _logger.Info($"Request received:\n{requestJson}");

        await _sendpoint.GetConsentRequest!.Send(cbRequestDto);

        var cbsRequest = new CbsGetConsentRequest
        {
            PaymentId = "",
            ConsentId = "",
            CorrelationId = guid,
        };

        if (_serviceParams.Value.Online)
        {
            var apiResult = await _consentservice.GetConsentByIdFromCoreBankAsync(cbsRequest, _logger.Log);

            if (apiResult == null)
                return NotFound(new ErrorResponse { errorCode = "404", errorMessage = "Payment data not found." });

            var cbResponseDto = new CbGetConsentResponseDto { CorrelationId = guid };

            if (apiResult.Success)
            {
                var cbResponse = ConsentGetResponseSample.GetSampleResponse(consentId);
                cbResponseDto.cbGetConsentResponse = cbResponse;
                cbResponseDto.Status = "Accepted";

                await _sendpoint.GetConsentResponse!.Send(cbResponseDto);

                var response = await GetResponseObject(cbResponse);
                responseJson = response.ToString();
                stopwatch.Stop();

                var log = AuditLogFactory.CreateAuditLog(
                    correlationId: guid,
                    sourceSystem: "CentralBankAPI",
                    targetSystem: "CoreBankAPI",
                    endpoint: endPointUrl,
                    requestPayload: requestJson,
                    responsePayload: responseJson,
                    statusCode: "200",
                    requestType: MessageTypeMappings.RetrieveConsent,
                    executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

                await _sendpoint.AuditLog!.Send(log);

                return Ok(cbResponse);
            }
            else
            {
                cbResponseDto.cbGetConsentResponse = new CbGetConsentResponse();
                cbResponseDto.Status = "FAILED";
                await _sendpoint.GetConsentResponse!.Send(cbResponseDto);

                return NotFound(new ErrorResponse { errorCode = "404", errorMessage = apiResult.Message ?? "FxQuote data not found." });
            }

        }
        else
        {
            var cbResponseDto = new CbGetConsentResponseDto { CorrelationId = guid };
            var cbResponse = ConsentGetResponseSample.GetSampleResponse(consentId);
            cbResponseDto.cbGetConsentResponse = cbResponse;
            cbResponseDto.Status = "Accepted";

            await _sendpoint.GetConsentResponse!.Send(cbResponseDto);

            var response = await GetResponseObject(cbResponse);
            responseJson = response.ToString();
            stopwatch.Stop();

            var log = AuditLogFactory.CreateAuditLog(
                correlationId: guid,
                sourceSystem: "CentralBankAPI",
                targetSystem: "CoreBankAPI",
                endpoint: endPointUrl,
                requestPayload: requestJson,
                responsePayload: responseJson,
                statusCode: "200",
                requestType: MessageTypeMappings.RetrieveConsent,
                executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

            await _sendpoint.AuditLog!.Send(log);

            return Ok(cbResponse);
        }

    }

    [HttpGet("/consents/{consentId}/audit")]
    public async Task<IActionResult> GetConsentAuditById(string consentId)
    {
        var stopwatch = Stopwatch.StartNew();
        string requestJson;
        string responseJson = string.Empty;
        var endPointUrl = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.GetConsentById!);

        if (!HttpContext.Items.TryGetValue("X-Correlation-ID", out var corrIdObj) ||
            !Guid.TryParse(corrIdObj?.ToString(), out var guid))
        {
            _logger.Info($"Invalid CorrelationId: {corrIdObj}");
            return BadRequest(new ErrorResponse { errorCode = "400", errorMessage = $"Invalid CorrelationId: {corrIdObj}" });
        }

        var cbRequestDto = new CbGetConsentRequestDto
        {
            CorrelationId = guid,
            ConsentId = consentId,
            cbGetConsentQueryParameters = new CbGetConsentQueryParameters(),
        };

        requestJson = JsonConvert.SerializeObject(cbRequestDto, Formatting.Indented);
        _logger.Info($"Request received:\n{requestJson}");

        await _sendpoint.GetConsentAuditRequest!.Send(cbRequestDto);



        var cbResponseDto = new CbGetConsentAuditResponseDto { CorrelationId = guid };

        if (true)
        {
            var cbResponse = SampleAuditResponseBuilder.GetSampleResponse();
            cbResponseDto.cbGetConsentAuditResponse = cbResponse;
            cbResponseDto.Status = "Accepted";

            await _sendpoint.GetConsentAuditResponse!.Send(cbResponseDto);

            var response = await GetResponseObject(cbResponse);
            responseJson = response.ToString();
            stopwatch.Stop();

            var log = AuditLogFactory.CreateAuditLog(
                correlationId: guid,
                sourceSystem: "CentralBankAPI",
                targetSystem: "CoreBankAPI",
                endpoint: endPointUrl,
                requestPayload: requestJson,
                responsePayload: responseJson,
                statusCode: "200",
                requestType: MessageTypeMappings.RetrieveConsent,
                executionTimeMs: (int)stopwatch.ElapsedMilliseconds);

            await _sendpoint.AuditLog!.Send(log);

            return Ok(cbResponse);
        }
        //else
        //{
        //    cbResponseDto.cbGetConsentResponse = new CbGetConsentResponse();
        //    cbResponseDto.Status = "FAILED";
        //    await _sendpoint.GetConsentResponse!.Send(cbResponseDto);

        //    return NotFound(new ErrorResponse { errorCode = "404", errorMessage = apiResult.Message ?? "FxQuote data not found." });
        //}
    }

    private CbGetConsentQueryParameters MapQueryParameterToCbGetConsentQueryParameter(long? updatedAt, string consentType, string status, int page, int pageSize, Guid correlationId)
       => new()

       {
           UpdatedAt = updatedAt,
           ConsentType = consentType,
           Status = status,
           Page = page,
           PageSize = pageSize,
           CorrelationId = correlationId
       };

    //private CbPostConsentHeader MapHeadersToCbPostConsentHeader(
    //   string providerId, string aspspId, string callerOrgId, string callerClientId,
    //   string softwareStatementId, string apiUri, string apiOperation,
    //   string consentId, string callerInteractionId, string ozoneInteractionId, string psuIdentifier, Guid correlationId)
    //   => new()
    //   {
    //       O3ProviderId = providerId,
    //       O3AspspId = aspspId,
    //       O3CallerOrgId = callerOrgId,
    //       O3CallerClientId = callerClientId,
    //       O3CallerSoftwareStatementId = softwareStatementId,
    //       O3ApiUri = apiUri,
    //       O3ApiOperation = apiOperation,
    //       O3ConsentId = consentId,
    //       O3CallerInteractionId = callerInteractionId,
    //       O3OzoneInteractionId = ozoneInteractionId,
    //       O3PsuIdentifier = psuIdentifier,
    //       CorrelationId = correlationId
    //   };

    private Task<JObject> GetResponseObject<T>(T responseObject)
    {
        try
        {
            var response = JObject.FromObject(responseObject!, new JsonSerializer
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
