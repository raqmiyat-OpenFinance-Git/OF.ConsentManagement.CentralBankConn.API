using OF.ConsentManagement.Model.CentralBank.Consent.GetResponse;
using Authorization = OF.ConsentManagement.Model.CentralBank.Consent.GetResponse.Authorization;
using ConsentBody = OF.ConsentManagement.Model.CentralBank.Consent.GetResponse.ConsentBody;
using ConsentBodyData = OF.ConsentManagement.Model.CentralBank.Consent.GetResponse.ConsentBodyData;
using ConsentDetails = OF.ConsentManagement.Model.CentralBank.Consent.GetResponse.ConsentDetails;
using ConsentMeta = OF.ConsentManagement.Model.CentralBank.Consent.GetResponse.ConsentMeta;
using MultipleAuthorizers = OF.ConsentManagement.Model.CentralBank.Consent.GetResponse.MultipleAuthorizers;
using OnBehalfOf = OF.ConsentManagement.Model.CentralBank.Consent.GetResponse.OnBehalfOf;
using OpenFinanceBilling = OF.ConsentManagement.Model.CentralBank.Consent.GetResponse.OpenFinanceBilling;
using Subscription = OF.ConsentManagement.Model.CentralBank.Consent.GetResponse.Subscription;
using Webhook = OF.ConsentManagement.Model.CentralBank.Consent.GetResponse.Webhook;

namespace OF.ConsentManagement.CentralBankConn.API.Stub;
public static class ConsentGetResponseSample
{
    public static CbGetConsentResponse GetSampleResponse()
    {
        return new CbGetConsentResponse
        {
            Meta = new Meta
            {
                PageNumber = 1,
                PageSize = 10,
                TotalPages = 1,
                TotalRecords = 2
            },
            Data = new List<ConsentData>
            {
                new ConsentData
                {
                    Id = "CD-1001",
                    ParId = "PAR-001",
                    RarType = "RA",
                    StandardVersion = "1.0",
                    ConsentGroupId = "GRP-789",
                    RequestUrl = "https://api.centralbank.com/consent",
                    ConsentType = "ACCOUNT_ACCESS",
                    Status = "ACTIVE",
                    InteractionId = "INT-123",
                    UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    AuthorizationChannel = "WEB",

                    Request = new ConsentRequest
                    {
                        Type = "ACCOUNT",
                        Consent = new ConsentDetails
                        {
                            BaseConsentId = "BC-001",
                            ConsentId = "CONSENT-12345",
                            ExpirationDateTime = DateTime.UtcNow.AddMonths(6),
                            TransactionFromDateTime = DateTime.UtcNow.AddMonths(-1),
                            TransactionToDateTime = DateTime.UtcNow,
                            AccountType = new List<string> { "SAVINGS", "CURRENT" },
                            AccountSubType = new List<string> { "INDIVIDUAL" },
                            OnBehalfOf = new OnBehalfOf
                            {
                                TradingName = "FinCorp",
                                LegalName = "FinCorp Ltd",
                                IdentifierType = "LEI",
                                Identifier = "LEI123456789"
                            },
                            Permissions = new List<string> { "ReadAccounts", "ReadBalances" },
                            OpenFinanceBilling = new OpenFinanceBilling
                            {
                                IsLargeCorporate = false,
                                UserType = "Retail",
                                Purpose = "Personal Finance"
                            }
                        },
                        Subscription = new Subscription
                        {
                            Webhook = new Webhook
                            {
                                Url = "https://clientapp.com/webhook",
                                IsActive = true
                            }
                        }
                    },

                    ConsentBody = new ConsentBody
                    {
                        Data = new ConsentBodyData
                        {
                            BaseConsentId = "BC-001",
                            ConsentId = "CONSENT-12345",
                            ExpirationDateTime = DateTime.UtcNow.AddMonths(6),
                            TransactionFromDateTime = DateTime.UtcNow.AddMonths(-1),
                            TransactionToDateTime = DateTime.UtcNow,
                            Status = "ACTIVE",
                            RevokedBy = null,
                            CreationDateTime = DateTime.UtcNow,
                            AccountType = new List<string> { "SAVINGS" },
                            AccountSubType = new List<string> { "INDIVIDUAL" },
                            OnBehalfOf = new OnBehalfOf
                            {
                                TradingName = "FinCorp",
                                LegalName = "FinCorp Ltd",
                                IdentifierType = "LEI",
                                Identifier = "LEI123456789"
                            },
                            Permissions = new List<string> { "ReadAccounts", "ReadTransactions" },
                            OpenFinanceBilling = new OpenFinanceBilling
                            {
                                IsLargeCorporate = false,
                                UserType = "Retail",
                                Purpose = "Personal Finance"
                            }
                        },
                        Meta = new ConsentMeta
                        {
                            MultipleAuthorizers = new MultipleAuthorizers
                            {
                                TotalRequired = 2,
                                Authorizations = new List<Authorization>
                                {
                                    new Authorization
                                    {
                                        AuthorizerId = "USR-1",
                                        AuthorizerType = "ADMIN",
                                        AuthorizationDate = DateTime.UtcNow,
                                        AuthorizationStatus = "APPROVED"
                                    },
                                    new Authorization
                                    {
                                        AuthorizerId = "USR-2",
                                        AuthorizerType = "MANAGER",
                                        AuthorizationDate = DateTime.UtcNow,
                                        AuthorizationStatus = "PENDING"
                                    }
                                }
                            }
                        }
                    },

                    Tpp = new Tpp
                    {
                        ClientId = "CLIENT-001",
                        TppId = "TPP-987",
                        TppName = "TrustedApp",
                        SoftwareStatementId = "SSA-2025-001",
                        DirectoryRecord = "DirectoryEntry-001",
                        OrgId = "ORG-123",
                        DecodedSsa = new DecodedSsa
                        {
                            Client_id = "client-abc",
                            Client_name = "TrustedApp",
                            Client_uri = "https://trustedapp.com",
                            Logo_uri = "https://trustedapp.com/logo.png",
                            Jwks_uri = "https://trustedapp.com/jwks",
                            Organisation_id = "ORG-123",
                            Application_type = "web",
                            Redirect_uris = new List<string> { "https://trustedapp.com/callback" },
                            Roles = new List<string> { "AISP", "PISP" },
                            Sector_identifier_uri = "https://trustedapp.com/sector"
                        }
                    },

                    PsuIdentifiers = new PsuIdentifiers
                    {
                        UserId = "USER-123"
                    },

                    ConsentUsage = new ConsentUsage
                    {
                        LastDataShared = DateTime.UtcNow.AddDays(-2),
                        LastServiceInitiationAttempt = DateTime.UtcNow.AddDays(-1)
                    },

                    AccountIds = new List<string> { "ACC-111", "ACC-222" },
                    InsurancePolicyIds = new List<string> { "POLICY-001" },
                    ConnectToken = "TOKEN-123456",

                    RequestHeaders = new Dictionary<string, object>
                    {
                        { "Authorization", "Bearer xyz" },
                        { "RequestId", Guid.NewGuid().ToString() }
                    },

                    SupplementaryInformation = new Dictionary<string, object>
                    {
                        { "Channel", "MOBILE" },
                        { "Device", "iOS" }
                    },

                    OzoneSupplementaryInformation = new Dictionary<string, object>
                    {
                        { "Region", "Asia" },
                        { "Branch", "Mumbai" }
                    },

                    PaymentContext = new Dictionary<string, object>
                    {
                        { "PaymentPurpose", "Bill Payment" },
                        { "Frequency", "Monthly" }
                    }
                }
            }
        };
    }
}
