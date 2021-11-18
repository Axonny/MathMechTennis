using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Rating;
using TableTennisDomain.DomainRepositories;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace App
{
    public class TelegramBot
    {
        private readonly AppPresenter<EloRecord> presenter;
        private static int AdminId => 0;

        public TelegramBot(string token)
        {
            presenter = new AppPresenter<EloRecord>(
                this,
                new MatchesRepository(),
                new UserRepository(),
                new EloRating());
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
            var messageTokens = message.Text?.Split(' ') ?? new[] {""};
            var command = messageTokens[0];
            Console.WriteLine(command);
            await bot.SendTextMessageAsync(
                message.Chat, 
                "Hello, I've registered you", 
                cancellationToken: token);
            
            switch (command)
            {
                case "/start":
                    await Task.Run(() => presenter.RegisterPlayer(message.Chat), token);
                    await bot.SendTextMessageAsync(
                        message.Chat, 
                        "Hello, I've registered you", 
                        cancellationToken: token);
                    break;
                case "/help":
                    await bot.SendTextMessageAsync(
                        message.Chat, 
                        "/start\n/help\n/match\n/rating", 
                        cancellationToken: token);
                    break;
                case "/match":
                {
                    var (p1, gw1, gw2) = (0, 0, 0);
                    try
                    {
                        (p1, gw1, gw2) = (
                            int.Parse(messageTokens[1]),
                            int.Parse(messageTokens[3]),
                            int.Parse(messageTokens[4]));
                    }
                    catch (Exception e)
                    {
                        await bot.SendTextMessageAsync(
                            message.Chat, 
                            "Can't parse a data for command",
                            cancellationToken: token);
                        break;
                    }
            
                    await Task.Run(() => presenter.RegisterMatch(p1, messageTokens[2], gw1, gw2), token);
                    await bot.SendTextMessageAsync(message.Chat, "Match is registered", cancellationToken: token);
                    break;
                }
                case "/rating":
                {
                    try
                    {
                        var rating = await Task.Run(() => presenter.GetRating(message.Chat.Id), token);
                        await bot.SendTextMessageAsync(
                            message.Chat,
                            $"Your rating is {rating}",
                            cancellationToken: token);
                    }
                    catch (KeyNotFoundException)
                    {
                        await bot.SendTextMessageAsync(
                            message.Chat,
                            "You you are not in rating",
                            cancellationToken: token);
                    }
            
                    break;
                }
                default:
                    await bot.SendTextMessageAsync(
                        message.Chat, 
                        "I dont know this command :(", 
                        cancellationToken: token);
                    break;
            }
        }
    }
}