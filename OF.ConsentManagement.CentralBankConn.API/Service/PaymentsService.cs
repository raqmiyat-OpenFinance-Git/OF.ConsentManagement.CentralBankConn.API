using ConsentMangerModel.Consent;
using OF.ConsentManagement.CentralBankConn.API.Repositories;
using OF.ConsentManagement.Model.Common;

namespace ConsentManagerService.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<CoreBankApis> _backEndApis;
        private readonly IMasterRepository _masterRepository;
        public PaymentsService(HttpClient httpClient, IOptions<CoreBankApis> backEndApis, IMasterRepository masterRepository)
        {
            _httpClient = httpClient;
            _backEndApis = backEndApis;
            _masterRepository = masterRepository;
        }

        public async Task<GetAuditConsentsByConsentIdResponse> GetAuditConsentResponse(string consentId, string AccountId, Logger logger)
        {
            logger.Info("GetAuditConsentResponse started");
            var response = new GetAuditConsentsByConsentIdResponse();
            await Task.Run(() =>
            {
                try
                {

                    response = new GetAuditConsentsByConsentIdResponse
                    {
                        data = new List<Datum>
                        {
                            new Datum
                            {
                                consentId = "cns-4a2b3f7d-7f32-4f17-90a2-33b2a54f4567",
                                paymentType = "Single",
                                paymentId = "pay-9c4a7d91-2e84-4e5d-bb44-1f9f68bca123",
                                idempotencyKey = "idem-6e5c1d2a-8c4f-41e6-a127-64afc3c72d91",

                                paymentResponse = new PaymentResponse
                                {
                                    id = "resp-7fdd2ab9-42e6-4d2b-bc4a-b7af6714f123",
                                    status = "Pending",
                                    creationDateTime = "2025-08-31T10:30:00Z",
                                    statusUpdateDateTime = "2025-08-31T10:31:05Z",
                                    OpenFinanceBilling = new OpenFinanceBilling
                                    {
                                        Type = "Collection",
                                        MerchantId = "MER-567890",
                                        NumberOfSuccessfulTransactions = 0
                                    }
                                },

                                signedResponse = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8A....",

                                tpp = new Tpp
                                {
                                    clientId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                                    tppId = "TPP-001",
                                    tppName = "ABC Fintech Solutions",
                                    softwareStatementId = "ssa-42b3a71a-6e59-41f3-97a1-83b36f91c333",
                                    directoryRecord = "https://directory.openfinance.ae/records/TPP-001",
                                    decodedSsa = new DecodedSsa
                                    {
                                        redirect_uris = new List<string> { "https://abcfintech.com/callback" },
                                        client_name = "ABC Fintech",
                                        client_uri = "https://abcfintech.com",
                                        logo_uri = "https://abcfintech.com/logo.png",
                                        jwks_uri = "https://abcfintech.com/jwks.json",
                                        client_id = "abc-client-123",
                                        roles = new List<string> { "PISP", "AISP" },
                                        sector_identifier_uri = "https://abcfintech.com/sector.json",
                                        application_type = "web",
                                        organisation_id = "6f8d8c34-2f91-4eaf-83f9-2b71c1e3d511"
                                    },
                                    orgId = "6f8d8c34-2f91-4eaf-83f9-2b71c1e3d511"
                                },

                                accountId = 100200300,
                                psuIdentifiers = new PsuIdentifiers { userId = "user-445566" },
                                interactionId = new InteractionId
                                {
                                    ozoneInteractionId = "oz-int-9876",
                                    clientInteractionId = "cl-int-5678"
                                },

                                authorizationCode = new AuthorizationCode
                                {
                                    paymentId = "pay-9c4a7d91-2e84-4e5d-bb44-1f9f68bca123",
                                    accessTokenHash = "ab347c8e9f6d3f8d5b12a1f9084c77c9",
                                    currentDateTime = "2025-08-31T10:31:00Z"
                                },

                                requestBody = new RequestBody
                                {
                                    Data = new RequestData
                                    {
                                        ConsentId = "cns-4a2b3f7d-7f32-4f17-90a2-33b2a54f4567",
                                        Instruction = new Instruction
                                        {
                                            Amount = new AmountData
                                            {
                                                Amount = "250.75",
                                                Currency = "AED"
                                            }
                                        },
                                        CurrencyRequest = new CurrencyRequest
                                        {
                                            InstructionPriority = "Normal",
                                            ExtendedPurpose = "Utility Bill Payment",
                                            ChargeBearer = "BorneByCreditor",
                                            CurrencyOfTransfer = "AED",
                                            DestinationCountryCode = "AE",
                                            ExchangeRateInformation = new ExchangeRateInformation
                                            {
                                                UnitCurrency = "USD",
                                                ExchangeRate = 0.27,
                                                RateType = "Actual",
                                                ContractIdentification = "FX-CTR-2025-0098"
                                            },
                                            FxQuoteId = "fxq-8732d821-9ac2-43ef-82c9-f1298d7a99f0"
                                        },
                                        PaymentPurposeCode = "SVC",
                                        DebtorReference = "TPP=3fa85f64-5717-4562-b3fc-2c963f66afa6,Merchant=MER-5678-TLCOMP-2025,BIC=CRESCHZZ80A",
                                        CreditorReference = "TPP=3fa85f64-5717-4562-b3fc-2c963f66afa6,BIC=CBDUAEADXXX"
                                    }
                                },

                                signedRequestBody = "MIICXQIBAAKBgQC7...",
                                requestHeaders = new RequestHeaders
                                {
                                    O3ProviderId = "Request",
                                    O3CallerOrgId = "1.0",
                                    O3CallerClientId = "Request",
                                    O3CallerSoftwareStatementId = "Request",
                                    O3ApiUri = "Request",
                                    O3ApiOperation = "Request",
                                    O3CallerInteractionId = "Request",
                                    O3OzoneInteractionId = "Request",
                                }
                            }
                        },
                        meta = new Meta
                        {
                            TotalRecords = 1,
                        }
                    };
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error in GetAuditConsentSample()");
                }
            });
            logger.Info("GetAuditConsentResponse completed");
            return response;
        }

        public async Task<string> PatchPaymentLogResponse(string Id, PatchPaymentRecordBody patchPaymentRecordBody, Logger logger)
        {
            logger.Info("PatchPaymentLogResponse started");
            string responseStr = string.Empty;
            await Task.Run(() =>
            {
                try
                {
                    responseStr = "Payment Log Response updated successfully";
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error in PatchPaymentLogResponse()");
                }
            });
            logger.Info("PatchPaymentLogResponse completed");
            return responseStr;
        }

    }
}
