using System.Net;

namespace TelegramBackend
{
    internal class HttpServer
    {
        private readonly string[] _indexFiles = { "index.html", "index.htm", "default.html", "default.htm" };
        private readonly string _rootDirectory;
        private readonly HttpListener _listener = new();

        public HttpServer(string path)
        {
            _rootDirectory = path;
            _listener.Prefixes.Add($"http://localhost:8000/");
        }

        public void Start()
        {
            _listener.Start();
            
            Console.WriteLine("Server started.");
            Console.WriteLine("Listening for connections...");

            while (_listener.IsListening)
            {
                var context = _listener.GetContext();
                ProcessRequest(context);
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var filename = context.Request.Url.AbsolutePath;
            
            Console.WriteLine("Request: " + filename);

            if (filename == "/")
            {
                foreach (var indexFile in _indexFiles)
                {
                    if (!File.Exists(Path.Combine(_rootDirectory, indexFile))) continue;
                    filename = indexFile;
                    break;
                }
            }

            filename = Path.Combine(_rootDirectory, filename.TrimStart('/'));

            if (File.Exists(filename))
            {
                try
                {
                    var buffer = File.ReadAllBytes(filename);
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                catch (Exception)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            context.Response.OutputStream.Close();
        }
    }
}