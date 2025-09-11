namespace OF.ConsentManagement.Model.EFModel;

[Table("PatchResourceLogFxQuote")]

public class PatchResourceLogFxQuote
{
    [Key]
    public long Id { get; set; }  //bigint identity
    public Guid CorrelationId { get; set; } //uniqueidentifier

    public long FxQuoteId { get; set; }

    public decimal SellAmount { get; set; }
    public string? SellCurrency { get; set; }

    public decimal BuyAmount { get; set; }
    public string? BuyCurrency { get; set; }

    public string? ChargeBearer { get; set; }
    public string? ChargeType { get; set; }

    public decimal ChargeAmount { get; set; }
    public string? ChargeCurrency { get; set; }
    public string? ChargeDescription { get; set; }
   
    public decimal CommissionAmount { get; set; }
    public string? CommissionCurrency { get; set; }
   
    public string? Status { get; set; }

    public DateTime? CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public string? RequestPayload { get; set; }
    public string? ResponsePayload { get; set; }


}
