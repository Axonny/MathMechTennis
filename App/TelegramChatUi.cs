using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
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
        
        public async Task ShowMessage(string text)
        {
            if (text is null || text == "")
                return;

            await botClient.SendTextMessageAsync(chatId, text);
        }
    }
}