namespace TelegramBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var bot = new TelegramBot();
        
            bot.Run();

            Console.ReadLine();
        }
    }
}