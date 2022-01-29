namespace DDocsBackend.Routes;

public class Health : RestModuleBase
{
    [Route("/health", "GET")]
    public Task<RestResult> ExecuteHealthAsync()
    {
        return Task.FromResult(RestResult.OK);
    }
}
