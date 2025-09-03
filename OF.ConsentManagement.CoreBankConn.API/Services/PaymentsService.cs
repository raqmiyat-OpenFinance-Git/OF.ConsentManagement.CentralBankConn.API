
using ConsentManagerBackEndAPI.IServices;
using ConsentManagerCommon.NLog;
using ConsentMangerModel.Consent;
using OF.ConsentManagement.Common.Helpers;
using OF.ConsentManagement.Model.CoreBank;

namespace ConsentManagerBackEndAPI.Services
{
    public class PaymentsService : IPaymentsService
    {

        private readonly IDbConnection _dbConnection;
        private readonly PaymentsApiLogger _logger;
        public PaymentsService(IDbConnection dbConnection, PaymentsApiLogger logger)
        {
            _logger = logger;
            _dbConnection = dbConnection;
        }

        public async Task<ApiResult<GetAuditConsentsByConsentIdResponse>> GetPaymentLogDetails(CbGetAuditConsentByConsentIdRequestDto cbGetAuditConsentByConsentIdRequestDto)
        {
            _logger.Info("GetPaymentLogDetails started.");
            var getAuditConsentsByConsentIdResponse = new GetAuditConsentsByConsentIdResponse();
            try
            {
                await Task.Run(() =>
                {
                    getAuditConsentsByConsentIdResponse = new GetAuditConsentsByConsentIdResponse
                    {
                        data = new List<Datum>
                        {
                            new Datum
                            {
                                consentId = "cns-948ad3f2-6a2c-4a51-bf2f-7f1d98f8d123",
                                paymentType = "Single",
                                paymentId = "pay-53b7b292-5f91-42f1-b56a-8f7e1ad9a321",
                                idempotencyKey = "idem-95f0a3a1-4447-47b1-94b6-3f89d7cd77f2",
                                paymentResponse = new PaymentResponse
                                {
                                    id = "resp-8d3b6dbf-22fd-4e9b-9cb4-7df85db9d9b0",
                                    status = "Pending",
                                    creationDateTime = "2025-08-31T10:30:00Z",
                                    statusUpdateDateTime = "2025-08-31T10:31:15Z",
                                    OpenFinanceBilling = new OpenFinanceBilling
                                    {
                                        Type = "Collection",
                                        MerchantId = "MER-123456789",
                                        NumberOfSuccessfulTransactions = 0
                                    }
                                },
                                signedResponse = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCA...",
                                tpp = new Tpp
                                {
                                    clientId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                                    tppId = "TPP-001",
                                    tppName = "ABC Fintech Solutions",
                                    softwareStatementId = "ssa-789c5a7f-234a-42cb-8c4b-8adfbbfe9a90",
                                    directoryRecord = "https://directory.openfinance.ae/records/TPP-001",
                                    decodedSsa = new DecodedSsa
                                    {
                                        redirect_uris = new List<string>
                                        {
                                            "https://abcfintech.com/callback"
                                        },
                                        client_name = "ABC Fintech",
                                        client_uri = "https://abcfintech.com",
                                        logo_uri = "https://abcfintech.com/logo.png",
                                        jwks_uri = "https://abcfintech.com/jwks.json",
                                        client_id = "abc-client-123",
                                        roles = new List<string> { "AISP", "PISP" },
                                        sector_identifier_uri = "https://abcfintech.com/sector.json",
                                        application_type = "web",
                                        organisation_id = "1b2d5c3a-55e9-4a61-9cde-22bffb14a812"
                                    },
                                    orgId = "1b2d5c3a-55e9-4a61-9cde-22bffb14a812"
                                },
                                accountId = 100200300,
                                psuIdentifiers = new PsuIdentifiers
                                {
                                    userId = "user-99887766"
                                },
                                interactionId = new InteractionId
                                {
                                    ozoneInteractionId = "oz-int-0001",
                                    clientInteractionId = "cl-int-12345"
                                },
                                authorizationCode = new AuthorizationCode
                                {
                                    paymentId = "pay-53b7b292-5f91-42f1-b56a-8f7e1ad9a321",
                                    accessTokenHash = "8d969eef6ecad3c29a3a629280e686cf",
                                    currentDateTime = "2025-08-31T10:31:00Z"
                                },
                                requestBody = new RequestBody
                                {
                                    Data = new RequestData
                                    {
                                        ConsentId = "cns-948ad3f2-6a2c-4a51-bf2f-7f1d98f8d123",
                                        Instruction = new Instruction
                                        {
                                            Amount = new AmountData
                                            {
                                                Amount = "100.00",
                                                Currency = "AED"
                                            }
                                        },
                                        CurrencyRequest = new CurrencyRequest
                                        {
                                            InstructionPriority = "Normal",
                                            ExtendedPurpose = "Tuition Fees Payment",
                                            ChargeBearer = "BorneByCreditor",
                                            CurrencyOfTransfer = "AED",
                                            DestinationCountryCode = "AE",
                                            ExchangeRateInformation = new ExchangeRateInformation
                                            {
                                                UnitCurrency = "USD",
                                                ExchangeRate = 0.5,
                                                RateType = "Actual",
                                                ContractIdentification = "FX-CTR-2025-001"
                                            },
                                            FxQuoteId = "fxq-77f4dd21-bb23-41e1-9ad3-0e90f1b671f1"
                                        },
                                        PersonalIdentifiableInformation = "eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZHQ00ifQ...",
                                        PaymentPurposeCode = "SVC",
                                        DebtorReference = "TPP=3fa85f64-5717-4562-b3fc-2c963f66afa6,Merchant=MER-1234-TLXYZ-2025,BIC=CRESCHZZ80A",
                                        CreditorReference = "TPP=3fa85f64-5717-4562-b3fc-2c963f66afa6,BIC=CBDUAEADXXX"
                                    }
                                },
                                signedRequestBody = "MIICWwIBAAKBgQDlJxjX..",
                                requestHeaders = new RequestHeaders
                                {
                                    O3ProviderId = "TPP-001",
                                    O3CallerOrgId = "1b2d5c3a-55e9-4a61-9cde-22bffb14a812",
                                    O3CallerClientId = "abc-client-123",
                                    O3CallerSoftwareStatementId = "ssa-789c5a7f-234a-42cb-8c4b-8adfbbfe9a90",
                                    O3ApiUri = "https://abcfintech.com/api/v1/consents",
                                    O3ApiOperation = "POST",
                                    O3CallerInteractionId = "cl-int-12345",
                                    O3OzoneInteractionId = "oz-int-0001"
                                }
                            }
                        },
                        meta = new Meta
                        {
                            TotalRecords = 1
                        }
                    };
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while fetching GetPaymentLogDetails");
            }
            _logger.Info($"GetPaymentLogDetails completed.");

            if (getAuditConsentsByConsentIdResponse != null)
            {
                return ApiResultFactory.Success(getAuditConsentsByConsentIdResponse!, "200");
            }
            else
            {
                return ApiResultFactory.Failure<GetAuditConsentsByConsentIdResponse>("Payment log details not found"!, "400");
            }
        }

        public async Task<ApiResult<string>> PatchPaymentLogDetails(CbPatchPaymentRecordRequestDto patchPaymentLogDetailsRequest)
        {
            _logger.Info($"PatchPaymentLogDetails started.");
            string response = string.Empty;
            try
            {
                await Task.Run(() =>
                {
                    response = "Payment log details updated successfully.";
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while fetching PatchPaymentLogDetails");
            }
            _logger.Info($"PatchPaymentLogDetails completed.");


            if (response != null)
            {
                return ApiResultFactory.Success<string>("Payment log details updated successfully"!, "200");
            }
            else
            {
                return ApiResultFactory.Failure<string>("Payment log details not found"!, "400");
            }
        }

    }
}
