using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentMangerModel.EFModel.Payments
{
    [Table("PatchPaymentLog")]
    public class PatchPaymentLog
    {
        [Key]
        public long Id { get; set; }  // bigint identity

        public Guid CorrelationId { get; set; }  // uniqueidentifier

        public string? RequestId { get; set; }

        public string? PaymentResponseStatus { get; set; }

        public int? NumberOfSuccessfulTransactions { get; set; }

        public string? PaymentResponsePaymentTransactionId { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public string? RequestPayload { get; set; }

        public string? ResponsePayload { get; set; }
    }
}
