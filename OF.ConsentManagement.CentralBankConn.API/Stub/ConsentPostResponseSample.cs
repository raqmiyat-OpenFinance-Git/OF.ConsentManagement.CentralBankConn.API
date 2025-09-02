using OF.ConsentManagement.Model.CentralBank.Consent.PostResponse;
using Authorization = OF.ConsentManagement.Model.CentralBank.Consent.PostResponse.Authorization;

namespace OF.ConsentManagement.CentralBankConn.API.Stub;
public static class ConsentPostResponseSample
{
    public static CbPostConsentResponse GetSampleResponse()
    {
        return new CbPostConsentResponse
        {
            Data = new ConsentResponseData
            {
                Id = "resp-12345",
                ParId = "par-98765",
                RarType = "RAR-Type-Example",
                StandardVersion = "1.0.0",
                ConsentGroupId = "group-001",
                RequestUrl = "https://bankapi.com/consents/resp-12345",
                ConsentType = "AccountAccess",
                Status = "Authorised",

                Request = new ConsentRequestDto
                {
                    Type = "urn:openfinanceuae:insurance-consent:v1",
                    Consent = new ConsentDetails
                    {
                        BaseConsentId = "base-111",
                        ExpirationDateTime = DateTime.UtcNow.AddDays(90),
                        TransactionFromDateTime = DateTime.UtcNow.AddMonths(-3),
                        TransactionToDateTime = DateTime.UtcNow,
                        AccountType = new List<string> { "Retail" },
                        AccountSubType = new List<string> { "CurrentAccount" },
                        OnBehalfOf = new OnBehalfOf
                        {
                            TradingName = "ABC Brokers",
                            LegalName = "ABC Brokerage LLC",
                            IdentifierType = "TradeLicense",
                            Identifier = "TL-123456"
                        },
                        ConsentId = "consent-001",
                        Permissions = new List<string> { "ReadAccountsBasic", "ReadTransactionsDetail" },
                        OpenFinanceBilling = new OpenFinanceBilling
                        {
                            UserType = "Retail",
                            Purpose = "AccountAggregation"
                        }
                    },
                    Subscription = new Subscription
                    {
                        Webhook = new Webhook
                        {
                            Url = "https://tpp.com/webhook",
                            IsActive = true
                        }
                    }
                },
                RequestHeaders = new Dictionary<string, string>
                {
                    { "x-correlation-id", "corr-12345" },
                    { "authorization", "Bearer abcdefg" }
                },

                ConsentBody = new ConsentBody
                {
                    Data = new ConsentBodyData
                    {
                        BaseConsentId = "base-111",
                        ExpirationDateTime = DateTime.UtcNow.AddDays(90),
                        TransactionFromDateTime = DateTime.UtcNow.AddMonths(-3),
                        TransactionToDateTime = DateTime.UtcNow,
                        AccountType = new List<string> { "Retail" },
                        AccountSubType = new List<string> { "CurrentAccount" },
                        OnBehalfOf = new OnBehalfOf
                        {
                            TradingName = "ABC Brokers",
                            LegalName = "ABC Brokerage LLC",
                            IdentifierType = "TradeLicense",
                            Identifier = "TL-123456"
                        },
                        Status = "Active",
                        RevokedBy = null,
                        CreationDateTime = DateTime.UtcNow,
                        ConsentId = "consent-001",
                        Permissions = new List<string> { "ReadAccountsBasic", "ReadTransactionsDetail" },
                        OpenFinanceBilling = new OpenFinanceBilling
                        {
                            IsLargeCorporate = false,
                            UserType = "Retail",
                            Purpose = "AccountAggregation"
                        }
                    },
                    Meta = new ConsentMeta
                    {
                        MultipleAuthorizers = new MultipleAuthorizers
                        {
                            TotalRequired = 1,
                            Authorizations = new List<Authorization>
                            {
                                new Authorization
                                {
                                    AuthorizerId = "auth-123",
                                    AuthorizerType = "Customer",
                                    AuthorizationDate = DateTime.UtcNow,
                                    AuthorizationStatus = "Approved"
                                }
                            }
                        }
                    },
                    Subscription = new Subscription
                    {
                        Webhook = new Webhook
                        {
                            Url = "https://tpp.com/webhook",
                            IsActive = true
                        }
                    }
                },

                InteractionId = "interaction-001",
                Tpp = new TppInfo
                {
                    ClientId = "client-123",
                    TppId = "tpp-789",
                    TppName = "FinTech Aggregator",
                    SoftwareStatementId = "ssa-5555",
                    DirectoryRecord = "dir-987",
                    OrgId = "org-456",
                    DecodedSsa = new DecodedSsa
                    {
                        Redirect_uris = new List<string> { "https://tpp.com/callback" },
                        Client_name = "FinTech Aggregator App",
                        Client_uri = "https://tpp.com",
                        Logo_uri = "https://tpp.com/logo.png",
                        Jwks_uri = "https://tpp.com/jwks.json",
                        Client_id = "client-123",
                        Roles = new List<string> { "AISP", "PISP" },
                        Sector_identifier_uri = "https://tpp.com/sector",
                        Application_type = "web",
                        Organisation_id = "org-456"
                    }
                },
                OzoneSupplementaryInformation = null,
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            },
            Meta = new { traceId = "trace-abc-123" }
        };
    }
}
