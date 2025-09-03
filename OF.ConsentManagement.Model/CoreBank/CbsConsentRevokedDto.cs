namespace ConsentMangerModel.CoreBank
{
    public class CbsConsentRevokedDto
    {
        public Guid CorrelationId { get; set; }
        public string? ConsentGroupId { get; set; }
        public string? ConsentId { get; set; }
        public string? PsuUserId { get; set; }
        public string? Revokedby { get; set; }
        public string? Status { get; set; }
    }
}
