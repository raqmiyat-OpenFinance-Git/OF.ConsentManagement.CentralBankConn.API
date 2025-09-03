namespace ConsentManagerService.Controllers
{
    public class HealthCheckApiController : Controller
    {
        [HttpGet]
        [Route("/hello-mtls")]
        public virtual IActionResult HelloMtls()
        {

            string exampleJson = null;
            exampleJson = "{\r\n  \"hostName\" : \"hostName\",\r\n  \"clientCertificate\" : {\r\n    \"subject\" : \"subject\",\r\n    \"issuer\" : \"issuer\"\r\n  },\r\n  \"mtlsStatus\" : \"established\",\r\n  \"connectionEstablished\" : true\r\n}";

            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<HealthCheckCertResponse>(exampleJson)
            : default(HealthCheckCertResponse);
            return new ObjectResult(example);
        }
    }
}
