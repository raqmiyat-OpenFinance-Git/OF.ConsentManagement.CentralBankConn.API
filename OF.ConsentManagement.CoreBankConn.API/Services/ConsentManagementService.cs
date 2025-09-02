using OF.ConsentManagement.CentralBankConn.API.Model;
using OF.ConsentManagement.Common.AES;
using OF.ConsentManagement.Common.Custom;
using OF.ConsentManagement.Common.Helpers;
using OF.ConsentManagement.CoreBankConn.API.IServices;
using OF.ConsentManagement.Model.Common;
using OF.ConsentManagement.Model.CoreBank;
using OF.ConsentManagement.Model.EFModel;


namespace OF.ConsentManagement.CoreBankConn.API.Services;

public class ConsentManagementService : IConsentManagementService
{
    private readonly SecurityParameters _securityParameters;
    private readonly AesCbcGenericService _aesCbcGenericService;
    private readonly HttpClient _httpClient;
    private readonly IOptions<CoreBankApis> _coreBankApis;
    private readonly IOptions<ServiceParams> _serviceParams;
    
    private readonly SendPointInitialize _sendPointInitialize;
    public ConsentManagementService(AesCbcGenericService aesCbcGenericService, IOptions<SecurityParameters> securityParameters, HttpClient httpClient, IOptions<CoreBankApis> coreBankApis, CbsDbContext context, IOptions<ServiceParams> serviceParams, SendPointInitialize sendPointInitialize)
    {
        _aesCbcGenericService = aesCbcGenericService;
        _httpClient = httpClient;
        _securityParameters = securityParameters.Value;
        _coreBankApis = coreBankApis;
        _serviceParams = serviceParams;
        _sendPointInitialize = sendPointInitialize;
    }

    public async Task<ApiResult<CbsPostConsentResponse>> SendConsentToCoreBankAsync(CbsPostConsentRequest request, Logger logger)
    {
        bool isSuccessful = false;
        CbsPostConsentResponse response = new();
        CoreBankPosting posting = new();
        PostStatus postStatus = PostStatus.SUCCESS;
        string requestPayload = string.Empty;
        string responsePayload = string.Empty;
        DateTime messageSentAt = DateTime.Now;
        DateTime messageReceivedAt;
        try
        {
            requestPayload = JsonConvert.SerializeObject(request, Formatting.Indented);

            var content = GetStringContent(requestPayload, request.OurReferenceNumber!, logger);

            var url = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.PostConsent!);

            messageSentAt = DateTime.Now;

            HttpResponseMessage apiResponse = await _httpClient.PostAsync(url, content);

            messageReceivedAt = DateTime.Now;


            //var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7286/CreateConsent");
            //var content = new StringContent("{\r\n  \"ourReferenceNumber\": \"20250901000001\",\r\n  \"consentId\": \"20250901000001\",\r\n  \"paymentId\": \"20250901000001\",\r\n  \"correlationId\": \"3fa85f64-5717-4562-b3fc-2c963f66afa6\"\r\n}", null, "application/json");
            //request.Content = content;
            //var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());

            responsePayload = await apiResponse.Content.ReadAsStringAsync();

            logger.Debug($"CorrelationId: {request.CorrelationId}, IsSuccessStatusCode:{apiResponse.IsSuccessStatusCode}");

            if (apiResponse.IsSuccessStatusCode)
            {
                string message = GetResponseString(responsePayload, request.OurReferenceNumber!, logger);
                response = JsonConvert.DeserializeObject<CbsPostConsentResponse>(message)!;
                if (response == null)
                {

                    logger.Error($"CorrelationId: {request.CorrelationId} || Failed to deserialize customer response.");
                    postStatus = PostStatus.ERROR;
                    posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(postStatus), "500", "Failed to process response.", messageSentAt, messageReceivedAt, requestPayload, responsePayload, logger);
                }
                else
                {
                    isSuccessful = true;
                    postStatus = PostStatus.SUCCESS;
                    posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(postStatus), "000", "Successful", messageSentAt, messageReceivedAt, requestPayload, responsePayload, logger);
                }
            }
            else
            {
                string statusCode = ((int)apiResponse.StatusCode).ToString() ?? "500";
                postStatus = PostStatus.ERROR;
                posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(postStatus), statusCode, "API Response: FAILED", messageSentAt, messageReceivedAt, requestPayload, responsePayload, logger);

                logger.Debug($"CorrelationId: {request.CorrelationId} || API Response: FAILED");
            }

            //else
            //{
            //    var resposne = GetofflinePostResponse();
            //    responsePayload = JsonConvert.SerializeObject(resposne);
            //    messageReceivedAt = DateTime.Now;
            //    isSuccessful = true;
            //    postStatus = PostStatus.SUCCESS;
            //    posting = CreateCoreBankPosting(request.CorrelationId, null, Utils.GetStatus(postStatus), "000", "Successful", messageSentAt, messageReceivedAt, requestPayload, responsePayload, logger);
            //}


        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase))
        {
            messageReceivedAt = DateTime.Now;
            postStatus = PostStatus.TIMEOUT;
            posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(postStatus), "504", "Request is Timeout", messageSentAt, messageReceivedAt, requestPayload, ex.Message, logger);

            logger.Error(ex, $"CorrelationId: {request.CorrelationId!} || TimeoutException: {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            messageReceivedAt = DateTime.Now;
            postStatus = PostStatus.HOST_ERROR;
            posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(postStatus), "502", "Connection failed: " + ex.Message, messageSentAt, messageReceivedAt, requestPayload, ex.Message, logger);

            logger.Error(ex, $"CorrelationId: {request.CorrelationId!} || Network error or server unreachable.");

        }
        catch (Exception ex)
        {
            messageReceivedAt = DateTime.Now;
            postStatus = PostStatus.ERROR;
            posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(PostStatus.ERROR), "500", "n unexpected error occurred. Contact support. " + ex.Message, messageSentAt, messageReceivedAt, requestPayload, ex.Message, logger);

            logger.Error(ex, $"CorrelationId: {request.CorrelationId!} || Exception: {ex.Message}");
        }
        await _sendPointInitialize.CbsPostingRequest!.Send(posting);

        logger.Info(WirteCoreBankPostingLog(logger, posting));
        if (isSuccessful)
        {
            return ApiResultFactory.Success(data: response!, posting.ResponsePayload!, "200");

        }
        else
        {
            return ApiResultFactory.Failure<CbsPostConsentResponse>(posting.ReturnDescription!, posting.ReturnCode!);
        }
    }
    public async Task<ApiResult<CbsPatchConsentResponse>> SendConsentUpdateToCoreBankAsync(CbsPatchConsentRequest request, Logger logger)
    {
        bool isSuccessful = false;
        CbsPatchConsentResponse response = new();
        CoreBankPosting posting = new();
        PostStatus postStatus = PostStatus.SUCCESS;
        string requestPayload = string.Empty;
        string responsePayload = string.Empty;
        DateTime messageSentAt = DateTime.Now;
        DateTime messageReceivedAt;
        try
        {
            requestPayload = JsonConvert.SerializeObject(request, Formatting.Indented);

            var content = GetStringContent(requestPayload, request.OurReferenceNumber!, logger);

            var url = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.UpdateConsent!);

            messageSentAt = DateTime.Now;



            messageReceivedAt = DateTime.Now;
            
                HttpResponseMessage apiResponse = await _httpClient.PostAsync(url, content);
                responsePayload = await apiResponse.Content.ReadAsStringAsync();

                logger.Debug($"CorrelationId: {request.CorrelationId}, IsSuccessStatusCode:{apiResponse.IsSuccessStatusCode}");

                if (apiResponse.IsSuccessStatusCode)
                {
                    string message = GetResponseString(responsePayload, request.OurReferenceNumber!, logger);
                    response = JsonConvert.DeserializeObject<CbsPatchConsentResponse>(message)!;
                    if (response == null)
                    {

                        logger.Error($"CorrelationId: {request.CorrelationId} || Failed to deserialize customer response.");
                        postStatus = PostStatus.ERROR;
                        posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(postStatus), "500", "Failed to process response.", messageSentAt, messageReceivedAt, requestPayload, responsePayload, logger);
                    }
                    else
                    {
                        isSuccessful = true;
                        postStatus = PostStatus.SUCCESS;
                        posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(postStatus), "000", "Successful", messageSentAt, messageReceivedAt, requestPayload, responsePayload, logger);
                    }
                }
                else
                {
                    string statusCode = ((int)apiResponse.StatusCode).ToString() ?? "500";
                    postStatus = PostStatus.ERROR;
                    posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(postStatus), statusCode, "API Response: FAILED", messageSentAt, messageReceivedAt, requestPayload, responsePayload, logger);

                    logger.Debug($"CorrelationId: {request.CorrelationId} || API Response: FAILED");
                }

            
            //else
            //{
            //    var resposne = OfflineFxQuoteUpdateResponse();
            //    responsePayload = JsonConvert.SerializeObject(resposne);
            //    messageReceivedAt = DateTime.Now;
            //    isSuccessful = true;
            //    postStatus = PostStatus.SUCCESS;
            //    posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(postStatus), "000", "Successful", messageSentAt, messageReceivedAt, requestPayload, responsePayload, logger);

            //}
        }

        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase))
        {
            messageReceivedAt = DateTime.Now;
            postStatus = PostStatus.TIMEOUT;
            posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(postStatus), "504", "Request is Timeout", messageSentAt, messageReceivedAt, requestPayload, ex.Message, logger);

            logger.Error(ex, $"CorrelationId: {request.CorrelationId!} || TimeoutException: {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            messageReceivedAt = DateTime.Now;
            postStatus = PostStatus.HOST_ERROR;
            posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(postStatus), "502", "Connection failed: " + ex.Message, messageSentAt, messageReceivedAt, requestPayload, ex.Message, logger);

            logger.Error(ex, $"CorrelationId: {request.CorrelationId!} || Network error or server unreachable.");

        }
        catch (Exception ex)
        {
            messageReceivedAt = DateTime.Now;
            postStatus = PostStatus.ERROR;
            posting = CreateCoreBankPosting(request.CorrelationId, request.OurReferenceNumber!, Utils.GetStatus(PostStatus.ERROR), "500", "n unexpected error occurred. Contact support. " + ex.Message, messageSentAt, messageReceivedAt, requestPayload, ex.Message, logger);

            logger.Error(ex, $"CorrelationId: {request.CorrelationId!} || Exception: {ex.Message}");
        }
        await _sendPointInitialize.CbsPostingRequest!.Send(posting);

        logger.Info(WirteCoreBankPostingLog(logger, posting));
        if (isSuccessful)
        {
            return ApiResultFactory.Success(data: response!, posting.ResponsePayload!, "200");

        }
        else
        {
            return ApiResultFactory.Failure<CbsPatchConsentResponse>(posting.ReturnDescription!, posting.ReturnCode!);
        }
    }
    public async Task<ApiResult<CbsGetConsentResponse>> GetConsentsFromCoreBankAsync(CbsGetConsentRequest cbsRequest, Logger logger)
    {
        bool isSuccessful = false;
        CbsGetConsentResponse response = new();
        CoreBankEnquiry paymentEnquiry = new();
        PostStatus postStatus = PostStatus.SUCCESS;
        string requestPayload = string.Empty;
        string responsePayload = string.Empty;
        DateTime requestTimestamp = DateTime.Now;
        DateTime responseTimestamp;
        try
        {
            requestPayload = JsonConvert.SerializeObject(cbsRequest, Formatting.Indented);

            var content = GetStringContent(requestPayload, cbsRequest.OurReferenceNumber!, logger);

            var url = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.GetConsents!);

            requestTimestamp = DateTime.Now;


            responseTimestamp = DateTime.Now;

            
                HttpResponseMessage apiResponse = await _httpClient.PostAsync(url, content);
                responsePayload = await apiResponse.Content.ReadAsStringAsync();
                logger.Debug($"CorrelationId: {cbsRequest.CorrelationId}, IsSuccessStatusCode:{apiResponse.IsSuccessStatusCode}");
                responseTimestamp = DateTime.Now;

                if (apiResponse.IsSuccessStatusCode)
                {
                    string message = GetResponseString(responsePayload, cbsRequest.OurReferenceNumber!, logger);
                    response = JsonConvert.DeserializeObject<CbsGetConsentResponse>(message)!;
                    if (response == null)
                    {

                        logger.Error($"CorrelationId: {cbsRequest.CorrelationId} || Failed to deserialize customer response.");
                        postStatus = PostStatus.ERROR;


                        paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveAllConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, "", null, "", Utils.GetStatus(postStatus), null, null, null, null, null, null, "500", "Failed to process response.", requestTimestamp, responseTimestamp, isSuccessful, "Corebank response is null", requestPayload, responsePayload, logger);
                    }
                    else
                    {
                        isSuccessful = true;
                        postStatus = PostStatus.SUCCESS;

                        paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveAllConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, response.CoreBankReferenceId, response.Amount, response.Currency, Utils.GetStatus(postStatus), response.TransactionDate, response.ValueDate, response.PayerAccountNumber, response.PayerName, response.PayeeAccountNumber, response.PayeeName, response.BankResponseCode, response.BankResponseMessage, requestTimestamp, responseTimestamp, isSuccessful, "", requestPayload, responsePayload, logger);
                    }
                }
                else
                {
                    string statusCode = ((int)apiResponse.StatusCode).ToString() ?? "500";
                    postStatus = PostStatus.ERROR;

                    paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveAllConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, "", null, "", Utils.GetStatus(postStatus), null, null, null, null, null, null, statusCode, "API Response: FAILED", requestTimestamp, responseTimestamp, isSuccessful, "API Response: FAILED", requestPayload, responsePayload, logger);

                    logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || API Response: FAILED");
                }

            
            //else
            //{
            //    var resposne = OfflineGetFxQuoteResponse();
            //    responsePayload = JsonConvert.SerializeObject(resposne);
            //    responseTimestamp = DateTime.Now;
            //    isSuccessful = true;
            //    postStatus = PostStatus.SUCCESS;

            //    paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.PaymentEnquiry, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, response.CoreBankReferenceId, response.Amount, response.Currency, Utils.GetStatus(postStatus), response.TransactionDate, response.ValueDate, response.PayerAccountNumber, response.PayerName, response.PayeeAccountNumber, response.PayeeName, response.BankResponseCode, response.BankResponseMessage, requestTimestamp, responseTimestamp, isSuccessful, "", requestPayload, responsePayload, logger);
            //}


        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase))
        {
            responseTimestamp = DateTime.Now;
            postStatus = PostStatus.TIMEOUT;

            paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveAllConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, "", null, "", Utils.GetStatus(postStatus), null, null, null, null, null, null, "504", "Request is Timeout", requestTimestamp, responseTimestamp, isSuccessful, ex.Message, requestPayload, responsePayload, logger);

            logger.Error(ex, $"CorrelationId: {cbsRequest.CorrelationId!} || TimeoutException: {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            responseTimestamp = DateTime.Now;
            postStatus = PostStatus.HOST_ERROR;

            paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveAllConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, "", null, "", Utils.GetStatus(postStatus), null, null, null, null, null, null, "502", "Connection failed", requestTimestamp, responseTimestamp, isSuccessful, ex.Message, requestPayload, responsePayload, logger);

            logger.Error(ex, $"CorrelationId: {cbsRequest.CorrelationId!} || Network error or server unreachable.");

        }
        catch (Exception ex)
        {
            responseTimestamp = DateTime.Now;
            postStatus = PostStatus.ERROR;


            paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveAllConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, "", null, "", Utils.GetStatus(postStatus), null, null, null, null, null, null, "500", "An unexpected error occurred. Contact support.", requestTimestamp, responseTimestamp, isSuccessful, ex.Message, requestPayload, responsePayload, logger);

            logger.Error(ex, $"CorrelationId: {cbsRequest.CorrelationId!} || Exception: {ex.Message}");
        }
        //paymentEnquiry.EnquiryId = await _cbsRepository.SaveCbsEnquiryAsync(paymentEnquiry, logger);
        await _sendPointInitialize.CbsEnquiryRequest!.Send(paymentEnquiry);

        logger.Info(WriteCoreBankEnquiryLog(logger, paymentEnquiry));
        if (isSuccessful)
        {
            return ApiResultFactory.Success(data: response!, paymentEnquiry.ResponsePayload!, "200");

        }
        else
        {
            return ApiResultFactory.Failure<CbsGetConsentResponse>(paymentEnquiry.BankResponseMessage!, paymentEnquiry.BankResponseCode!);
        }
    }
    public async Task<ApiResult<CbsGetConsentResponse>> GetConsentByIdFromCoreBankAsync(CbsGetConsentRequest cbsRequest, Logger logger)
    {
        bool isSuccessful = false;
        CbsGetConsentResponse response = new();
        CoreBankEnquiry paymentEnquiry = new();
        PostStatus postStatus = PostStatus.SUCCESS;
        string requestPayload = string.Empty;
        string responsePayload = string.Empty;
        DateTime requestTimestamp = DateTime.Now;
        DateTime responseTimestamp;
        try
        {
            requestPayload = JsonConvert.SerializeObject(cbsRequest, Formatting.Indented);

            var content = GetStringContent(requestPayload, cbsRequest.OurReferenceNumber!, logger);

            var url = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.GetConsents!);

            requestTimestamp = DateTime.Now;


            responseTimestamp = DateTime.Now;

            
                HttpResponseMessage apiResponse = await _httpClient.PostAsync(url, content);
                responsePayload = await apiResponse.Content.ReadAsStringAsync();
                logger.Debug($"CorrelationId: {cbsRequest.CorrelationId}, IsSuccessStatusCode:{apiResponse.IsSuccessStatusCode}");
                responseTimestamp = DateTime.Now;

                if (apiResponse.IsSuccessStatusCode)
                {
                    string message = GetResponseString(responsePayload, cbsRequest.OurReferenceNumber!, logger);
                    response = JsonConvert.DeserializeObject<CbsGetConsentResponse>(message)!;
                    if (response == null)
                    {

                        logger.Error($"CorrelationId: {cbsRequest.CorrelationId} || Failed to deserialize customer response.");
                        postStatus = PostStatus.ERROR;


                        paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, "", null, "", Utils.GetStatus(postStatus), null, null, null, null, null, null, "500", "Failed to process response.", requestTimestamp, responseTimestamp, isSuccessful, "Corebank response is null", requestPayload, responsePayload, logger);
                    }
                    else
                    {
                        isSuccessful = true;
                        postStatus = PostStatus.SUCCESS;

                        paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, response.CoreBankReferenceId, response.Amount, response.Currency, Utils.GetStatus(postStatus), response.TransactionDate, response.ValueDate, response.PayerAccountNumber, response.PayerName, response.PayeeAccountNumber, response.PayeeName, response.BankResponseCode, response.BankResponseMessage, requestTimestamp, responseTimestamp, isSuccessful, "", requestPayload, responsePayload, logger);
                    }
                }
                else
                {
                    string statusCode = ((int)apiResponse.StatusCode).ToString() ?? "500";
                    postStatus = PostStatus.ERROR;

                    paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, "", null, "", Utils.GetStatus(postStatus), null, null, null, null, null, null, statusCode, "API Response: FAILED", requestTimestamp, responseTimestamp, isSuccessful, "API Response: FAILED", requestPayload, responsePayload, logger);

                    logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || API Response: FAILED");
                }

            
            //else
            //{
            //    var resposne = OfflineGetFxQuoteResponse();
            //    responsePayload = JsonConvert.SerializeObject(resposne);
            //    responseTimestamp = DateTime.Now;
            //    isSuccessful = true;
            //    postStatus = PostStatus.SUCCESS;

            //    paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.PaymentEnquiry, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, response.CoreBankReferenceId, response.Amount, response.Currency, Utils.GetStatus(postStatus), response.TransactionDate, response.ValueDate, response.PayerAccountNumber, response.PayerName, response.PayeeAccountNumber, response.PayeeName, response.BankResponseCode, response.BankResponseMessage, requestTimestamp, responseTimestamp, isSuccessful, "", requestPayload, responsePayload, logger);
            //}


        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase))
        {
            responseTimestamp = DateTime.Now;
            postStatus = PostStatus.TIMEOUT;

            paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, "", null, "", Utils.GetStatus(postStatus), null, null, null, null, null, null, "504", "Request is Timeout", requestTimestamp, responseTimestamp, isSuccessful, ex.Message, requestPayload, responsePayload, logger);

            logger.Error(ex, $"CorrelationId: {cbsRequest.CorrelationId!} || TimeoutException: {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            responseTimestamp = DateTime.Now;
            postStatus = PostStatus.HOST_ERROR;

            paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, "", null, "", Utils.GetStatus(postStatus), null, null, null, null, null, null, "502", "Connection failed", requestTimestamp, responseTimestamp, isSuccessful, ex.Message, requestPayload, responsePayload, logger);

            logger.Error(ex, $"CorrelationId: {cbsRequest.CorrelationId!} || Network error or server unreachable.");

        }
        catch (Exception ex)
        {
            responseTimestamp = DateTime.Now;
            postStatus = PostStatus.ERROR;


            paymentEnquiry = CreateCoreBankEnquiry(MessageTypeMappings.RetrieveConsent, cbsRequest.CorrelationId, cbsRequest.PaymentId, cbsRequest.OurReferenceNumber!, "", null, "", Utils.GetStatus(postStatus), null, null, null, null, null, null, "500", "An unexpected error occurred. Contact support.", requestTimestamp, responseTimestamp, isSuccessful, ex.Message, requestPayload, responsePayload, logger);

            logger.Error(ex, $"CorrelationId: {cbsRequest.CorrelationId!} || Exception: {ex.Message}");
        }
        //paymentEnquiry.EnquiryId = await _cbsRepository.SaveCbsEnquiryAsync(paymentEnquiry, logger);
        await _sendPointInitialize.CbsEnquiryRequest!.Send(paymentEnquiry);

        logger.Info(WriteCoreBankEnquiryLog(logger, paymentEnquiry));
        if (isSuccessful)
        {
            return ApiResultFactory.Success(data: response!, paymentEnquiry.ResponsePayload!, "200");

        }
        else
        {
            return ApiResultFactory.Failure<CbsGetConsentResponse>(paymentEnquiry.BankResponseMessage!, paymentEnquiry.BankResponseCode!);
        }
    }
    private string WirteCoreBankPostingLog(Logger logger, CoreBankPosting posting)
    {
        return $@"
                    ====== WirteCoreBankPostingLog Log ======
                    EnquiryId        : {posting.Id}
                    RequestType        : {posting.RequestType}
                    OurReferenceNumber     : {posting.OurReferenceNumber}
                    Status             : {posting.Status}
                    Account            : {posting.AccountNumber}
                    DebitCredit        : {posting.TransactionType}
                    Amount             : {posting.Amount}
                    CreatedOn          : {posting.CreatedOn:yyyy-MM-dd HH:mm:ss}
                    CreatedBy          : {posting.CreatedBy}
                    Module             : {posting.Module}
                    ReturnCode         : {posting.ReturnCode}
                    ReturnDescription  : {posting.ReturnDescription}
                    HostRefNbr         : {posting.HostReferenceNumber}
                    SentDatetime       : {posting.MessageSentAt:yyyy-MM-dd HH:mm:ss}
                    ReceviedDatetime   : {posting.MessageReceivedAt:yyyy-MM-dd HH:mm:ss}
                    RetryCount         : {posting.RetryCount}
                    RetryOn            : {posting.RetryOn:yyyy-MM-dd HH:mm:ss}
                    Comments           : {posting.Comments}
                    Request            : {PciDssSecurity.MaskCardInDynamicJson(posting.RequestPayload!, logger)}
                    Response           : {PciDssSecurity.MaskCardInDynamicJson(posting.ResponsePayload!, logger)}
                    ===============================";
    }

    private CoreBankPosting CreateCoreBankPosting(Guid correlationId, string? ourReferenceNumber, string status, string returnCode, string returnDescription, DateTime messageSentAt, DateTime messageReceivedAt, string requestPayload, string responsePayload, Logger logger)
    {
        CoreBankPosting coreBankPosting = new();
        try
        {

            coreBankPosting.RequestType = MessageTypeMappings.CreateConsent;
            coreBankPosting.OurReferenceNumber = ourReferenceNumber;
            coreBankPosting.Status = status;
            coreBankPosting.AccountNumber = "";
            coreBankPosting.TransactionType = "CR";
            coreBankPosting.Amount = 0;
            coreBankPosting.CreatedOn = DateTime.Now;
            coreBankPosting.CreatedBy = "CoreBankAPI";
            coreBankPosting.Module = "Inward";
            coreBankPosting.ReturnCode = returnCode;
            coreBankPosting.ReturnDescription = returnDescription;
            coreBankPosting.MessageSentAt = messageSentAt;
            coreBankPosting.MessageReceivedAt = messageReceivedAt;
            coreBankPosting.RetryCount = 0;
            coreBankPosting.RetryOn = null;
            coreBankPosting.RequestPayload = requestPayload;
            coreBankPosting.ResponsePayload = responsePayload;
            coreBankPosting.Comments = "Initiated via CoreBankAPI";
            coreBankPosting.LastUpdatedOn = null;

        }
        catch (Exception ex)
        {
            logger.Error(ex, $"[CreateCoreBankPosting] CorrelationId: {correlationId} || Error: {ex.Message}");
        }
        return coreBankPosting;
    }

    private string WriteCoreBankEnquiryLog(Logger logger, CoreBankEnquiry enquiry)
    {
        return $@"
============ WriteCoreBankEnquiryLog ============

EnquiryId           : {enquiry.EnquiryId}
EnquiryType         : {enquiry.EnquiryType}
CorrelationId       : {enquiry.CorrelationId}
PaymentId           : {enquiry.OfReferenceId}
OurReferenceNumber  : {enquiry.OurReferenceNumber}
CoreBankRefNbr      : {enquiry.CoreBankReferenceId}
Amount              : {enquiry.Amount}
Currency            : {enquiry.Currency}
PaymentStatus       : {enquiry.Status}

TransactionDate     : {enquiry.TransactionDate:yyyy-MM-dd HH:mm:ss}
ValueDate           : {enquiry.ValueDate:yyyy-MM-dd HH:mm:ss}

PayerAccountNumber  : {enquiry.PayerAccountNumber}
PayerName           : {enquiry.PayerName}
PayeeAccountNumber  : {enquiry.PayeeAccountNumber}
PayeeName           : {enquiry.PayeeName}

BankResponseCode    : {enquiry.BankResponseCode}
BankResponseMessage : {enquiry.BankResponseMessage}

RequestTimestamp    : {enquiry.RequestTimestamp:yyyy-MM-dd HH:mm:ss}
ResponseTimestamp   : {enquiry.ResponseTimestamp:yyyy-MM-dd HH:mm:ss}
IsSuccess           : {enquiry.IsSuccess}
ErrorMessage        : {enquiry.ErrorMessage}

Module              : {enquiry.Module}

RetryCount          : {enquiry.RetryCount}
RetryOn             : {enquiry.RetryOn:yyyy-MM-dd HH:mm:ss}

Comments            : {enquiry.Comments}
CreatedBy           : {enquiry.CreatedBy}
CreatedDate         : {enquiry.CreatedDate:yyyy-MM-dd HH:mm:ss}
ModifiedDate        : {enquiry.ModifiedDate:yyyy-MM-dd HH:mm:ss}
LastUpdatedOn       : {enquiry.LastUpdatedOn:yyyy-MM-dd HH:mm:ss}

RequestPayload      : {PciDssSecurity.MaskCardInDynamicJson(enquiry.RequestPayload!, logger)}
ResponsePayload     : {PciDssSecurity.MaskCardInDynamicJson(enquiry.ResponsePayload!, logger)}

=================================================
";
    }
    private CoreBankEnquiry CreateCoreBankEnquiry(string? enquiryType, Guid correlationId, string? paymentId, string? ourReferenceNumber, string? coreBankReferenceId, decimal? amount, string? currency, string? paymentStatus, DateTime? transactionDate, DateTime? valueDate, string? payerAccountNumber, string? payerName, string? payeeAccountNumber, string? payeeName, string? bankResponseCode, string? bankResponseMessage, DateTime? requestTimestamp, DateTime? responseTimestamp, bool isSuccess, string? errorMessage, string? requestPayload, string? responsePayload, Logger logger)
    {
        CoreBankEnquiry paymentEnquiry = new();

        try
        {
            paymentEnquiry.EnquiryType = enquiryType;
            paymentEnquiry.CorrelationId = correlationId;

            paymentEnquiry.OfReferenceId = paymentId;
            paymentEnquiry.OurReferenceNumber = ourReferenceNumber;
            paymentEnquiry.CoreBankReferenceId = coreBankReferenceId;

            paymentEnquiry.Amount = amount;
            paymentEnquiry.Currency = currency;
            paymentEnquiry.Status = paymentStatus;

            paymentEnquiry.TransactionDate = transactionDate;
            paymentEnquiry.ValueDate = valueDate;

            paymentEnquiry.PayerAccountNumber = payerAccountNumber;
            paymentEnquiry.PayerName = payerName;
            paymentEnquiry.PayeeAccountNumber = payeeAccountNumber;
            paymentEnquiry.PayeeName = payeeName;

            paymentEnquiry.BankResponseCode = bankResponseCode;
            paymentEnquiry.BankResponseMessage = bankResponseMessage;

            paymentEnquiry.RequestTimestamp = requestTimestamp;
            paymentEnquiry.ResponseTimestamp = responseTimestamp;

            paymentEnquiry.IsSuccess = isSuccess;
            paymentEnquiry.ErrorMessage = errorMessage;

            paymentEnquiry.RetryCount = 0;
            paymentEnquiry.RetryOn = null;

            paymentEnquiry.Module = "Inward";

            paymentEnquiry.RequestPayload = requestPayload;
            paymentEnquiry.ResponsePayload = responsePayload;
            paymentEnquiry.Comments = "Initiated via CoreBankAPI";

            var now = DateTime.UtcNow;
            paymentEnquiry.CreatedBy = "CoreBankAPI";
            paymentEnquiry.CreatedDate = now;
            paymentEnquiry.ModifiedDate = now;
            paymentEnquiry.LastUpdatedOn = now;
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"[CreatePaymentEnquiryEntity] CorrelationId: {correlationId} || Error: {ex.Message}");
        }

        return paymentEnquiry;
    }

    private StringContent GetStringContent(string jsonContent, string ourReferenceNumber, Logger logger)
    {
        try
        {
            if (_securityParameters.IsEncrypted)
            {
                jsonContent = _aesCbcGenericService.Encrypt(jsonContent, _securityParameters.KeyValue!, _securityParameters.InitVector!, logger);
                logger.Debug($"OurReferenceNumber: {ourReferenceNumber} || Encrypted String: {jsonContent}");
            }
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            return content;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error during encryption/decryption", ex);
        }
    }

    private string GetResponseString(string jsonResponse, string ourReferenceNumber, Logger logger)
    {
        try
        {
            if (_securityParameters.IsEncrypted)
            {
                jsonResponse = _aesCbcGenericService.Decrypt(jsonResponse, _securityParameters.KeyValue!, _securityParameters.InitVector!, logger);
            }
            logger.Debug($" OurReferenceNumber: {ourReferenceNumber} || Core Response data: {jsonResponse}");

            return jsonResponse;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error during encryption/decryption", ex);
        }
    }
    
}
