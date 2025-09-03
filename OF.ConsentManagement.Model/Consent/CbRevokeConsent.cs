namespace ConsentMangerModel.Consent
{
    public class CbRevokeConsent
    {
        public string RevokedBy { get; set; }
        public RevokedModel RevokedByPsu { get; set; }

    }
    public class RevokedModel
    {
        public string UserId { get; set; }

    }
}
