using OF.ConsentManagement.Model.CentralBank.Consent.GetRequestDto;
using OF.ConsentManagement.Model.CentralBank.Consent.GetResponseDto;
using OF.ConsentManagement.Model.EFModel;

namespace CentralBankReceiverWorker.Mappers;

public static class CbGetConsentAuditMapper
{
    public static ConsentAudit MapCbGetConsentAuditRequestToEF(CbGetConsentAuditRequestDto requestDto)
    {
        if (requestDto == null)
            throw new ArgumentNullException(nameof(requestDto));

        var consentAudit = new ConsentAudit
        {
            CorrelationId = requestDto.CorrelationId,
            Status = "PENDING",
            CreatedBy = "System",
            CreatedOn = DateTime.UtcNow,
            // Serialize payload
            RequestPayload = JsonConvert.SerializeObject(requestDto, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            })
        };

        return consentAudit;
    }

    public static List<ConsentAuditResponse> MapCbGetConsentAuditResponseToEF(CbGetConsentAuditResponseDto responseDto, long consentAuditId)
    {
        if (responseDto == null)
            throw new ArgumentNullException(nameof(responseDto));

        var response = responseDto.cbGetConsentAuditResponse
                       ?? throw new ArgumentNullException(nameof(responseDto.cbGetConsentAuditResponse));

        var data = responseDto.cbGetConsentAuditResponse.Data
        ?? throw new ArgumentNullException(nameof(responseDto.cbGetConsentAuditResponse.Data));

        List<ConsentAuditResponse> consentAuditResponses = new List<ConsentAuditResponse>();

        foreach (var da in data)
        {


            ConsentAuditResponse consentAuditResponse = new ConsentAuditResponse();
            consentAuditResponse.ConsentAuditId = consentAuditId;
            consentAuditResponse.ProviderId = da.ProviderId;
            consentAuditResponse.Operation = da.Operation;
            consentAuditResponse.Timestamp = da.Timestamp;
            consentAuditResponse.FkMongoId = da.FkMongoId;

            consentAuditResponse.FkId = da.FkId;
            consentAuditResponse.ConsentId = da.Id;
            consentAuditResponse.FkMongoId = da.FkMongoId;
            consentAuditResponse.OzoneInteractionId = da.OzoneInteractionId;
            consentAuditResponse.CallerOrgId = da.CallerDetails.CallerOrgId;
            consentAuditResponse.CallerClientId = da.CallerDetails.CallerClientId;
            consentAuditResponse.CallerSoftwareStatementId = da.CallerDetails.CallerSoftwareStatementId;
            consentAuditResponse.PatchFilter = da.PatchFilter;
            consentAuditResponse.Patch = da.Patch;

            consentAuditResponse.Status = "PENDING";
            consentAuditResponse.CreatedBy = "System";
            consentAuditResponse.CreatedOn = DateTime.UtcNow;

            consentAuditResponses.Add(consentAuditResponse);
        }


        return consentAuditResponses;
    }
}
