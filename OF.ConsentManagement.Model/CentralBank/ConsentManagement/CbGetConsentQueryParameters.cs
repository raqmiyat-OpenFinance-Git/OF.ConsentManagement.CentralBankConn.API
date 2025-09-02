namespace OF.ConsentManagement.Model.CentralBank.Consent.GetQuery;

public class CbGetConsentQueryParameters
{

    /// <summary>
    /// Last updated timestamp (optional)
    /// </summary>
    public long? UpdatedAt { get; set; }

    /// <summary>
    /// Type of consent (optional)
    /// </summary>
    public string? ConsentType { get; set; }

    /// <summary>
    /// Consent status (optional)
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Page number for pagination (default = 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Page size for pagination (default = 25)
    /// </summary>
    public int PageSize { get; set; } = 25;

    public Guid CorrelationId { get; set; }

    public string? ConsentId { get; set; }

    public string? RequestJson { get; set; }
}
