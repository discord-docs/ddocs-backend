namespace DDocsBackend.Routes;

public class Index : RestModuleBase
{
    [Route("/", "GET")]
    public Task<RestResult> ExecuteIndexAsync()
    {
        Response.Redirect("uri");

        return Task.FromResult(new RestResult(300));

    }
}
