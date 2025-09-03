public class ClientCertificate
{
    public string? subject { get; set; }
    public string? issuer { get; set; }
}

public class HealthCheckCertResponse
{
    public bool connectionEstablished { get; set; }
    public string? mtlsStatus { get; set; }
    public string? hostName { get; set; }
    public ClientCertificate? clientCertificate { get; set; }
}