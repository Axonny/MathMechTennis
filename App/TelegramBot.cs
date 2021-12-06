using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using App.Dialogs.ChatDialog;
using App.Rating;
using Ninject;
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
        private readonly Dictionary<long, ChatDialogGraph> dialogByChatId = new();

        public TelegramBot(string token)
        {
            var bot = new TelegramBotClient(token);
            
            application = new Application<EloRecord>(
                new MatchesRepository(),
                new PlayersRepository(),
                new EloRating());
            
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
            //await botClient.SendTextMessageAsync(BugReportChannelId, exception.ToString());
        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken _)
        {
            if (update.Message is { } message)
            {
                try
                {
                    if (dialogByChatId.TryGetValue(message.Chat.Id, out var dialogGraph))
                    {
                        await Task.Run(() => dialogGraph.HandleMessage(new TelegramMessageAdapter(message)));
                        return;
                    }

                    var isRegistered = application.IsRegisteredPlayer(message.Chat.Id);
                    dialogGraph = ChatDialogGraphBuilder.Build(
                        new TelegramChatUi(bot, message.Chat.Id), 
                        application, 
                        isRegistered ? "Default" : "Start");

                    dialogByChatId[message.Chat.Id] = dialogGraph;
                    await Task.Run(() => dialogGraph.HandleMessage(new TelegramMessageAdapter(message)));
                }
                catch (Exception exception)
                {
                    await Console.Out.WriteLineAsync(exception.ToString());
                    //await bot.SendTextMessageAsync(BugReportChannelId, exception.ToString());
                }
            }
        }

        private async Task HandleCommandAsync(ITelegramBotClient bot, Message message)
        {
            if (message.Text == "/start")
            {
                await application.RegisterPlayer(message.Chat.Username, message.Chat.Id);
                await bot.SendTextMessageAsync(message.Chat, "Hello");
            }
            else if (message.Text == "/set_result")
                await bot.SendTextMessageAsync(
                    message.Chat, 
                    "Send Match result. Format @opponent yourScore:opponentScore");
            else if (message.Text == "/show_rating")
            {
                var rating = await application.GetRating(message.Chat.Username);
                await bot.SendTextMessageAsync(message.Chat, $"You rating is {rating}");
            }
            else
                await bot.SendTextMessageAsync(message.Chat, "I dont know this command :(");
        }
    }
}