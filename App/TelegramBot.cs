using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
        private static Regex matchResultRegex = new Regex(@"@(\w+) (\d)[:;., ](\d)");

        private readonly Application<EloRecord> application;

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
            await botClient.SendTextMessageAsync(BugReportChannelId, exception.ToString());
        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken _)
        {
            if (update.Message is { } message)
            {
                try
                {
                    if (message.Text != null && message.Text.Contains('/'))
                        await HandleCommandAsync(bot, message);
                    else if (message.Text != null && matchResultRegex.IsMatch(message.Text))
                        await HandleSetResultsAsync(bot, message);
                    else
                        await bot.SendTextMessageAsync(message.Chat, "Use Commands");
                }
                catch (Exception exception)
                {
                    await bot.SendTextMessageAsync(message.Chat, "Something was wrong");
                    await Console.Out.WriteLineAsync(exception.ToString());
                    await bot.SendTextMessageAsync(BugReportChannelId, exception.ToString());
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

        private async Task HandleSetResultsAsync(ITelegramBotClient bot, Message message)
        {
            var groups = matchResultRegex.Match(message.Text!).Groups;
            var player1 = message.Chat.Username;
            var player2 = groups[1].Value;
            var gamesWon1 = int.Parse(groups[2].Value);
            var gamesWon2 = int.Parse(groups[3].Value);

            await application.RegisterMatch(player1, player2, gamesWon1, gamesWon2);
            await bot.SendTextMessageAsync(message.Chat, "Match has been registered!");
        }
    }
}