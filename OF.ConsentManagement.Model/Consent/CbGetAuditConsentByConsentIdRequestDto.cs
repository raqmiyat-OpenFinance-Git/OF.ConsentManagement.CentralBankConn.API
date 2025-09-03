using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentMangerModel.Consent
{
    public class CbGetAuditConsentByConsentIdRequestDto
    {
        public Guid CorrelationId { get; set; }
        public string? ConsentId { get; set; }
        public string? AccountId { get; set; }

    }
}
