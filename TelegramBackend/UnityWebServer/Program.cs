namespace TelegramBackend
{
    internal class Program
    {
        public const string Path = "app";

        private static void Main(string[] args)
        {
            var server = new HttpServer(Path);
            server.Start();
            Console.ReadKey();
            server.Stop();
        }
    }
}