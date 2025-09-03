using MassTransit;

namespace OF.ConsentManagement.CentralBankConn.API.Model
{
    public class SendPointInitialize
    {
        public ISendEndpoint? AugmentConsentRequest { get; set; }
        public ISendEndpoint? AugmentConsentResponse { get; set; }
        public ISendEndpoint? ValidateConsentRequest { get; set; }
        public ISendEndpoint? ValidateConsentResponse { get; set; }
        public ISendEndpoint? ConsentOperationRequest { get; set; }
        public ISendEndpoint? ConsentOperationResponse { get; set; }
        public ISendEndpoint? AuditLog { get; set; }
        public ISendEndpoint? CbsPostingRequest { get; set; }
        public ISendEndpoint? CbsPostingResponse { get; set; }
        public ISendEndpoint? CbsEnquiryRequest { get; set; }
        public ISendEndpoint? CbsEnquiryResponse { get; set; }
        public ISendEndpoint? PostConsentRequest { get; set; }
        public ISendEndpoint? PostConsentResponse { get; set; }
        public ISendEndpoint? GetConsentRequest { get; set; }
        public ISendEndpoint? GetConsentResponse { get; set; }
        public ISendEndpoint? PatchConsentRequest { get; set; }
        public ISendEndpoint? PatchConsentResponse { get; set; }
        public ISendEndpoint? GetConsentAuditRequest { get; set; }
        public ISendEndpoint? GetConsentAuditResponse { get; set; }
        public ISendEndpoint? GetPaymentLogRequest { get; set; }
        public ISendEndpoint? GetPaymentLogResponse { get; set; }
        public ISendEndpoint? RevokeConsentGroupIdRequest { get; set; }
        public ISendEndpoint? RevokeConsentIdRequest { get; set; }
        
    }
}
