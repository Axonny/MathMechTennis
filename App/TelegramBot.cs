using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace App
{
    public class TelegramBot
    {
        private static long BugReportChannelId => -1001610224482;
        private static Regex matchResultRegex = new Regex(@"@(\w+) (\d):(\d)"); 

        public TelegramBot(string token)
        {
            var bot = new TelegramBotClient(token);
            var receiverOptions = new ReceiverOptions();
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions
            );

        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken _)
        {
            if (update.Message is { } message)
            {
                if (message.Text != null && message.Text.Contains('/'))
                    await HandleCommandAsync(bot, message);
                else if (message.Text != null && matchResultRegex.IsMatch(message.Text))
                    await HandleSetResultsAsync(bot, message);
                else
                    await bot.SendTextMessageAsync(message.Chat, "Use Commands");
            }
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken _)
        {
            if (exception is ApiRequestException apiRequestException)
            {
                await botClient.SendTextMessageAsync(BugReportChannelId, apiRequestException.ToString());
            }
        }

        private async Task HandleCommandAsync(ITelegramBotClient bot, Message message)
        {
            if (message.Text == "/start")
                await bot.SendTextMessageAsync(message.Chat, "Hello");
            else if(message.Text == "/set_result")
                await bot.SendTextMessageAsync(message.Chat, "Send Match result. Format @opponent yourScore:opponentScore");
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
            // ToDo Convert USERNAME to PLAYER_ID
            // ToDo Call AppPresenter
            await bot.SendTextMessageAsync(message.Chat, "Match has been registered!");
        }
    }
}