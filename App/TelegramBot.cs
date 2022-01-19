using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using App.Dialogs.ChatDialog;
using App.Rating;
using TableTennisDomain.DomainRepositories;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace App
{
    [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
    [SuppressMessage("ReSharper", "CA2016")]
    public class TelegramBot
    {
        private static long BugReportChannelId => -1001610224482;
        private readonly Application<EloRecord> application;
        private readonly Dictionary<long, TelegramBranchManager> dialogByChatId = new();

        public TelegramBot(string token)
        {
            var bot = new TelegramBotClient(token);
            
            application = new Application<EloRecord>(
                new MatchesRepository(),
                new MatchStatusRepository(),
                new PlayersRepository(),
                new EloRating(new EloRatingRepository()));

            // BugReporter.OnReportSend += async exception => 
            //     await HandleErrorAsync(bot, exception, CancellationToken.None); 
            
            BugReporter.OnReportSend += async exception => 
                await Console.Out.WriteLineAsync(exception.ToString());

            var receiverOptions = new ReceiverOptions {ThrowPendingUpdates = true};

            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions
            );
        }
        
        private static async Task HandleErrorAsync(
            ITelegramBotClient botClient,
            Exception exception,
            CancellationToken _)
        {
            await botClient.SendTextMessageAsync(BugReportChannelId, exception.ToString());
        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken _)
        {
            try
            {
                var chatIdNullable = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id;
                if (!chatIdNullable.HasValue)
                    return;

                var chatId = chatIdNullable.Value;
                var chatMessage = ConvertToChatMessage(update);
                
                if (dialogByChatId.TryGetValue(chatId, out var manager))
                {
                    await Task.Run(() => manager.HandleMessage(chatMessage));
                    return;
                }
                    
                manager = TelegramBranchManagerBuilder.Build(
                    new TelegramChatUi(bot, chatId), 
                    application, 
                    application.IsRegisteredPlayer(chatId) ? "Default" : "Start");

                dialogByChatId[chatId] = manager;
                await Task.Run(() => manager.HandleMessage(chatMessage));
            }
            catch (Exception exception)
            {
                BugReporter.SendReport(exception);
            }
        }

        private IChatMessage ConvertToChatMessage(Update update)
        {
            if (update.Message is not null)
                return new TelegramMessageAdapter(update.Message);
            if (update.CallbackQuery is not null)
                return new TelegramCallbackAdapter(update.CallbackQuery);

            throw new ArgumentException("Not supported update type: " +
                                        "can convert only text messages and callbacks");
        }
    }
}