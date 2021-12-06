using System.Threading.Tasks;
using Telegram.Bot;

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
        
        public async Task ShowMessage(string text)
        {
            await botClient.SendTextMessageAsync(chatId, text);
        }
    }
}