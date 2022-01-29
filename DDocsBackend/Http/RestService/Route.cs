namespace DDocsBackend;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
internal class Route : Attribute
{
    public string Name { get; }
    public bool IsRegex { get; }
    public string Method { get; }

    public Route(string name, string method)
    {
        Name = name;
        Method = method;
    }

    public Route(string name, string method, bool isRegex)
    {
        Name = name;
        Method = method;
        IsRegex = isRegex;
    }
}
