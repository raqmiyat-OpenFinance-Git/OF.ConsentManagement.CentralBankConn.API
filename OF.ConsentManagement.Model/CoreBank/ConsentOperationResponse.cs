namespace OF.ConsentManagement.Model.CoreBank;

public class ConsentOperationResponse
{
  public Guid CorrelationID { get; set; }
  public string? Status { get; set; }
  public string? StatusCode { get; set; }
  public string? StatusDescription { get; set; }
}
