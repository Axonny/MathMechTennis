using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace App
{
    public class TelegramChatUi : IUi
    {
        private readonly ITelegramBotClient botClient;
        private readonly IApplication application;
        private readonly string nickname;

        public TelegramChatUi(ITelegramBotClient botClient, string nickname, IApplication application)
        {
            this.botClient = botClient;
            this.application = application;
            this.nickname = nickname;
        }
        
        public async Task ShowTextMessage(string text)
        {
            await ShowTextMessageFor(text, nickname);
        }

        public async Task ShowTextMessageFor(string text, string receiverNickname)
        {
            // ReSharper disable once MergeIntoLogicalPattern
            if (text is null || text == "")
                return;

            await botClient.SendTextMessageAsync(await application.GetChatIdByNickname(receiverNickname), text);
        }
        
        public async Task ShowMessageWithButtonFor(
            string messageText, 
            string buttonText, 
            string callbackData, 
            string receiverNickname)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(buttonText, callbackData)
                }
            });

            await botClient.SendTextMessageAsync(
                await application.GetChatIdByNickname(receiverNickname), 
                messageText, 
                replyMarkup: inlineKeyboard);
        }
    }
}