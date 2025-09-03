using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentMangerModel.EFModel.Payments
{
    [Table("GetPaymentLog")]
    public class GetPaymentLog
    {
        [Key]
        public long Id { get; set; }  // bigint identity

        public Guid CorrelationId { get; set; }  // uniqueidentifier

        public string ConsentId { get; set; } = string.Empty; // NOT NULL

        public string AccountId { get; set; } = string.Empty; // NOT NULL

        public string? Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public string? RequestPayload { get; set; }

        public string? ResponsePayload { get; set; }
    }

}
