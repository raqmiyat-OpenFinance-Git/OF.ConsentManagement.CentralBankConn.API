using OF.ConsentManagement.Model.CentralBank.Consent.GetAuditResponse;
using Meta = OF.ConsentManagement.Model.CentralBank.Consent.GetAuditResponse.Meta;

namespace OF.ConsentManagement.CentralBankConn.API.Stub;
public static class SampleAuditResponseBuilder
{
    public static CbGetConsentAuditResponse GetSampleResponse()
    {
        return new CbGetConsentAuditResponse
        {
            Data = new List<ConsentAuditData>
            {
                new ConsentAuditData
                {
                    ProviderId = "BankXYZ",
                    Operation = "CREATED",
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    FkMongoId = "64b8ac0f1234",
                    FkId = "5678",
                    Id = "CONS12345",   // ConsentId
                    OzoneInteractionId = "oz-987654",
                    CallerDetails = new CallerDetails
                    {
                        CallerOrgId = "Org001",
                        CallerClientId = "ClientABC",
                        CallerSoftwareStatementId = "SwSt12345"
                    },
                    PatchFilter = null,
                    Patch = null
                },
                new ConsentAuditData
                {
                    ProviderId = "BankXYZ",
                    Operation = "UPDATED",
                    Timestamp = DateTimeOffset.UtcNow.AddMinutes(-30).ToUnixTimeMilliseconds(),
                    FkMongoId = "64b8ac0f5678",
                    FkId = "9876",
                    Id = "CONS12345",   // same consent
                    OzoneInteractionId = "oz-987655",
                    CallerDetails = new CallerDetails
                    {
                        CallerOrgId = "Org001",
                        CallerClientId = "ClientABC",
                        CallerSoftwareStatementId = "SwSt12345"
                    },
                    PatchFilter = "/accountIds",
                    Patch = "{ \"op\": \"add\", \"path\": \"/accountIds/-\", \"value\": \"ACC7890\" }"
                },
                new ConsentAuditData
                {
                    ProviderId = "BankXYZ",
                    Operation = "REVOKED",
                    Timestamp = DateTimeOffset.UtcNow.AddMinutes(-5).ToUnixTimeMilliseconds(),
                    FkMongoId = "64b8ac0f9999",
                    FkId = "1111",
                    Id = "CONS12345",
                    OzoneInteractionId = "oz-987656",
                    CallerDetails = new CallerDetails
                    {
                        CallerOrgId = "Org001",
                        CallerClientId = "ClientABC",
                        CallerSoftwareStatementId = "SwSt12345"
                    },
                    PatchFilter = null,
                    Patch = null
                }
            },
            Meta = new Meta()
        };
    }
}