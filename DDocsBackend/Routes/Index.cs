namespace DDocsBackend.Routes;

public class Index : RestModuleBase
{
    [Route("/", "GET")]
    public Task<RestResult> ExecuteIndexAsync()
    {
        return Task.FromResult(RestResult.OK);
    }
}
