namespace OF.ConsentManagement.Model.CentralBank.Consent.GetAuditResponse;

public class CbGetConsentAuditResponse
{
    public List<ConsentAuditData> Data { get; set; }
    public Meta Meta { get; set; }
}

public class ConsentAuditData
{
    public string ProviderId { get; set; }
    public string Operation { get; set; }
    public long Timestamp { get; set; }   // epoch milliseconds
    public string FkMongoId { get; set; }
    public string FkId { get; set; }
    public string Id { get; set; }
    public string OzoneInteractionId { get; set; }
    public CallerDetails CallerDetails { get; set; }
    public string PatchFilter { get; set; }
    public string Patch { get; set; }
}

public class CallerDetails
{
    public string CallerOrgId { get; set; }
    public string CallerClientId { get; set; }
    public string CallerSoftwareStatementId { get; set; }
}

public class Meta
{
    // Add fields if API starts returning pagination, totals, etc.
}

