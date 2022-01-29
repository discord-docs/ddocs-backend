using Newtonsoft.Json;

namespace DDocsBackend;

public class RestResult
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("data")]
    public object? Data { get; set; } = null;

    internal bool TakeAction { get; set; } = true;

    /// <summary>
    ///     Includes the 200 status code with this response
    /// </summary>
    public static RestResult OK
        => new RestResult(200);

    /// <summary>
    ///     Includes the 401 status code with this response
    /// </summary>
    public static RestResult Unauthorized
        => new RestResult(401);

    /// <summary>
    ///     Includes the 403 status code with this response
    /// </summary>
    public static RestResult Forbidden
        => new RestResult(403);

    /// <summary>
    ///     Includes the 404 status code with this response
    /// </summary>
    public static RestResult NotFound
        => new RestResult(404);

    /// <summary>
    ///     Includes the 400 status code with this response
    /// </summary>
    public static RestResult BadRequest
        => new RestResult(400);

    /// <summary>
    ///     Includes the 500 status code with this response
    /// </summary>
    public static RestResult InternalServerError
        => new RestResult(500);

    /// <summary>
    ///     Keeps this request open.
    /// </summary>
    public static RestResult KeepOpen
        => new RestResult(int.MaxValue);

    public static RestResult NoAction
    {
        get
        {
            var a = new RestResult(200);
            a.TakeAction = false;

            return a;
        }
    }

    public RestResult WithData(object? data)
    {
        this.Data = data;
        return this;
    }

    public RestResult WithCode(int code)
    {
        this.Code = code;
        return this;
    }

    public RestResult() { }
    public RestResult(int code)
    {
        this.Code = code;
    }
}
