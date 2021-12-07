using App.Dialogs.ChatDialog;
using Telegram.Bot.Types;

namespace App
{
    public class TelegramMessageAdapter : IChatMessage
    {
        private readonly Message message;

        public string Username => message.Chat.Username;
        public long ChatId => message.Chat.Id;
        public string Text => message.Text;

        public TelegramMessageAdapter(Message message)
        {
            this.message = message;
        }
    }
}