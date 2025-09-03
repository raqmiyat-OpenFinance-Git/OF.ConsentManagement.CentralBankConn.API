using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentMangerModel.Consent
{
    public class CbPatchPaymentRecordRequestDto
    {
        public Guid CorrelationId { get; set; }
        public string? Id { get; set; }
        public PatchPaymentRecordBody? paymentRecordBody { get; set; }
    }
}
