using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace App
{
    public class TelegramChatUi : IUi
    {
        private readonly ITelegramBotClient botClient;
        private readonly long chatId;

        public TelegramChatUi(ITelegramBotClient botClient, long chatId)
        {
            this.botClient = botClient;
            this.chatId = chatId;
        }
        
        public async Task ShowTextMessage(string text)
        {
            await ShowTextMessageFor(text, chatId);
        }

        public async Task ShowTextMessageFor(string text, long receiverChatId)
        {
            if (text is null || text == "")
                return;

            await botClient.SendTextMessageAsync(receiverChatId, text);
        }
        
        //TODO: No chatId like argument. Use Nickname for all Ui.
        public async Task ShowMessageWithButtonFor(
            string messageText, 
            string buttonText, 
            string callbackData, 
            long receiverChatId)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData(buttonText, callbackData)
                }
            });

            await botClient.SendTextMessageAsync(receiverChatId, messageText, replyMarkup: inlineKeyboard);
        }
    }
}