using System;
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
        private static int AdminId => 0;

        public TelegramBot(string token)
        {
            var bot = new TelegramBotClient(token);
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions();
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
        {
            if (update.Message is { } message)
            {
                if (message.Text != null && message.Text.Contains('/'))
                    await HandleCommandAsync(bot, message, token);
                else
                    await bot.SendTextMessageAsync(message.Chat, "Use Commands", cancellationToken: token);
            }
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            if (exception is ApiRequestException apiRequestException)
            {
                await botClient.SendTextMessageAsync(AdminId, apiRequestException.ToString(), cancellationToken: token);
            }
        }

        private async Task HandleCommandAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            if (message.Text == "/start")
                await bot.SendTextMessageAsync(message.Chat, "Hello", cancellationToken: token);
            else
                await bot.SendTextMessageAsync(message.Chat, "I dont know this command :(", cancellationToken: token);
        }
    }
}