using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    public class TelegramBot
    {
        private const string TOKEN = "<Token>";
        private const string WEB_APP_URL = "<WebAppUrl>";
    
        public async void Run()
        {
            var botClient = new TelegramBotClient(TOKEN);
            var me = await botClient.GetMeAsync();
            
            Console.WriteLine($"Start listening for @{me.Username}");
            
            botClient.StartReceiving(UpdateHandler, ErrorHandler, new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            });
        }
    
        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                    {
                        OnReceiveMessage(botClient, update, cancellationToken);
                        break;
                    }
                    case UpdateType.CallbackQuery:
                    {
                        OnReceiveCallbackQuery(botClient, update, cancellationToken);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static async void OnReceiveMessage(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            if (update.Message == null)
                return;
            
            Console.WriteLine($"Receive message: {update.Message.Text}");

            if (update.Message.Text == null) 
                return;
            
            if (update.Message.Text.StartsWith("/start"))
            {
                var keyboard = new ReplyKeyboardMarkup(new KeyboardButton("Play") {WebApp = new WebAppInfo(){ Url = WEB_APP_URL } });
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Привет! Нажми на кнопку, чтобы открыть игру!", replyMarkup: keyboard, cancellationToken: cancellationToken);       
            }

            if (update.Message.Text.StartsWith("/game"))
            {
                await botClient.SendGameAsync(update.Message.Chat.Id, "clicker", cancellationToken: cancellationToken);
            }
        }

        private static async void OnReceiveCallbackQuery(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            if (update.CallbackQuery is {IsGameQuery: true}) await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, url: WEB_APP_URL, cancellationToken: cancellationToken);
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var errorMessage = error switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}