using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using App.Dialogs.ChatDialog;
using App.Rating;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace App
{
    [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
    [SuppressMessage("ReSharper", "CA2016")]
    public class TelegramBot<TRatingRecord> where TRatingRecord : class, IRatingRecord
    {
        private static long BugReportChannelId => -1001610224482;
        private readonly Application<TRatingRecord> application;
        private readonly Dictionary<long, TelegramBranchManager> dialogByChatId = new();

        public TelegramBot(string token, Application<TRatingRecord> application)
        {
            var bot = new TelegramBotClient(token);

            this.application = application;
                // new Application<EloRecord>(
                // new MatchesRepository(),
                // new MatchStatusRepository(),
                // new PlayersRepository(),
                // new EloRating(new EloRatingRepository()));

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
            if (update.Message is { } message)
            {
                try
                {
                    if (dialogByChatId.TryGetValue(message.Chat.Id, out var manager))
                    {
                        await Task.Run(() => manager.HandleMessage(new TelegramMessageAdapter(message)));
                        return;
                    }
                    
                    manager = TelegramBranchManagerBuilder.Build(
                        new TelegramChatUi(bot, message.Chat.Id), 
                        application, 
                        application.IsRegisteredPlayer(message.Chat.Id) ? "Default" : "Start");

                    dialogByChatId[message.Chat.Id] = manager;
                    await Task.Run(() => manager.HandleMessage(new TelegramMessageAdapter(message)));
                }
                catch (Exception exception)
                {
                    BugReporter.SendReport(exception);
                }
            }
        }
    }
}