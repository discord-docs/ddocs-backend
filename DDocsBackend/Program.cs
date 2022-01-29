using DDocsBackend;

Logger.AddStream(Console.OpenStandardOutput(), StreamType.StandardOut);
Logger.AddStream(Console.OpenStandardError(), StreamType.StandardError);

// TODO: Configure port?
var httpServer = new HttpServer(4048);

await Task.Delay(-1);