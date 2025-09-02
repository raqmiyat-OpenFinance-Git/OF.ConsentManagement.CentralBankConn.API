using Microsoft.AspNetCore.Mvc;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.CoreBank;

namespace CoreBankSimulatedAPI.Controllers;
[ApiController]
//[Route("api/[controller]")]
public class ConsentManagementController : ControllerBase
{

    private readonly ConsentLogger _paymentsLogger;

    public ConsentManagementController(ConsentLogger paymentsLogger)
    {
        _paymentsLogger = paymentsLogger;
    }

    [HttpPost]
    [Route("/CreateConsent")]
    public Task<IActionResult> CreateConsent(CbsPostConsentRequest cbsRequest)
    {
        try
        {
            var paymentId = cbsRequest.PaymentId ?? string.Empty;
            if (paymentId == null)
                return Task.FromResult<IActionResult>(BadRequest("Request body is missing."));


            if (paymentId.EndsWith("X"))
                throw new TimeoutException("Timeout error");
            else if (paymentId.EndsWith("Y"))
                throw new HttpRequestException("Connectivity error");
            else if (paymentId.EndsWith("Z"))
                throw new Exception("Unknown error");
            var coreBankResponse = new CbsPostConsentResponse
            {
                OurReferenceNumber = cbsRequest.OurReferenceNumber,
                ConsentId = cbsRequest.ConsentId,
                PaymentId = paymentId,
                CorrelationId = cbsRequest.CorrelationId,
                Amount = (decimal?)10.0005,
                ReturnCode = "0000",
                ReturnDescription = "Successful"
            };


            return Task.FromResult<IActionResult>(Ok(coreBankResponse));

        }
        catch (TimeoutException ex)
        {
            _paymentsLogger.Error(ex);
            return Task.FromResult<IActionResult>(BadRequest("TimeoutException error occurred. " + ex.Message));
        }
        catch (HttpRequestException ex)
        {
            _paymentsLogger.Error(ex);
            return Task.FromResult<IActionResult>(BadRequest("HttpRequestException error occurred. " + ex.Message));
        }
        catch (Exception ex)
        {
            _paymentsLogger.Error(ex);
            return Task.FromResult<IActionResult>(BadRequest("An unexpected error occurred. " + ex.Message));
        }
    }

    [HttpPost]
    [Route("/PatchConsent")]
    public Task<IActionResult> PatchConsent(CbsPatchConsentRequest cbsRequest)
    {
        try
        {
            var paymentId = cbsRequest.PaymentId ?? string.Empty;
            if (paymentId == null)
                return Task.FromResult<IActionResult>(BadRequest("Request body is missing."));


            if (paymentId.EndsWith("X"))
                throw new TimeoutException("Timeout error");
            else if (paymentId.EndsWith("Y"))
                throw new HttpRequestException("Connectivity error");
            else if (paymentId.EndsWith("Z"))
                throw new Exception("Unknown error");
            var coreBankResponse = new CbsPatchConsentResponse
            {
                OurReferenceNumber = cbsRequest.OurReferenceNumber,
                ConsentId = cbsRequest.ConsentId,
                PaymentId = paymentId,
                CorrelationId = cbsRequest.CorrelationId,
                Amount = (decimal?)10.0005,
                ReturnCode = "0000",
                ReturnDescription = "Successful"
            };


            return Task.FromResult<IActionResult>(Ok(coreBankResponse));

        }
        catch (TimeoutException ex)
        {
            _paymentsLogger.Error(ex);
            return Task.FromResult<IActionResult>(BadRequest("TimeoutException error occurred. " + ex.Message));
        }
        catch (HttpRequestException ex)
        {
            _paymentsLogger.Error(ex);
            return Task.FromResult<IActionResult>(BadRequest("HttpRequestException error occurred. " + ex.Message));
        }
        catch (Exception ex)
        {
            _paymentsLogger.Error(ex);
            return Task.FromResult<IActionResult>(BadRequest("An unexpected error occurred. " + ex.Message));
        }
    }

    [HttpPost]
    [Route("/ReadConsents")]
    public Task<IActionResult> ReadConsents(CbsGetConsentRequest cbsRequest)
    {
        try
        {
            var paymentId = cbsRequest.PaymentId ?? string.Empty;
            if (paymentId == null)
                return Task.FromResult<IActionResult>(BadRequest("Request body is missing."));


            if (paymentId.EndsWith("X"))
                throw new TimeoutException("Timeout error");
            else if (paymentId.EndsWith("Y"))
                throw new HttpRequestException("Connectivity error");
            else if (paymentId.EndsWith("Z"))
                throw new Exception("Unknown error");
            var coreBankResponse = new CbsGetConsentResponse
            {
                CorrelationId = Guid.NewGuid(),
                ConsentId = "CONSENT123456",
                PaymentId = "PAYMENT7890",
                OurReferenceNumber = "REF123456789",
                CoreBankReferenceId = "CBANK456789",
                Amount = 2500.75M,
                Currency = "INR",
                PaymentStatus = "Completed",

                TransactionDate = DateTime.UtcNow.AddDays(-1),
                ValueDate = DateTime.UtcNow,

                PayerAccountNumber = "123456789012",
                PayerName = "John Doe",
                PayeeAccountNumber = "987654321098",
                PayeeName = "Jane Smith",

                BankResponseCode = "00",
                BankResponseMessage = "Transaction successful"
            };


            return Task.FromResult<IActionResult>(Ok(coreBankResponse));

        }
        catch (TimeoutException ex)
        {
            _paymentsLogger.Error(ex);
            return Task.FromResult<IActionResult>(BadRequest("TimeoutException error occurred. " + ex.Message));
        }
        catch (HttpRequestException ex)
        {
            _paymentsLogger.Error(ex);
            return Task.FromResult<IActionResult>(BadRequest("HttpRequestException error occurred. " + ex.Message));
        }
        catch (Exception ex)
        {
            _paymentsLogger.Error(ex);
            return Task.FromResult<IActionResult>(BadRequest("An unexpected error occurred. " + ex.Message));
        }
    }
}
