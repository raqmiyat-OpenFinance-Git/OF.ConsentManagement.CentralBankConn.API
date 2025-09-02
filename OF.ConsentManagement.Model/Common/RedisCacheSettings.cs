namespace OF.ConsentManagement.Model.Common;

public class RedisCacheSettings
{
    public string? Url { get; set; }
    public bool EnableCache { get; set; }
    public int SetAbsoluteExpirationAddMinutes { get; set; }
    public int SetSlidingExpirationFromMinutes { get; set; }
}

