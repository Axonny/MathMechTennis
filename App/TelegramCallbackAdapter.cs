using System;
using App.Dialogs.ChatDialog;
using Telegram.Bot.Types;

namespace App
{
    public class TelegramCallbackAdapter : IChatMessage
    {
        private readonly CallbackQuery query;
        private readonly Message message;

        public string Username => message.Chat.Username;
        public long ChatId => message.Chat.Id;
        public string Text => query.Data;

        public TelegramCallbackAdapter(CallbackQuery query)
        {
            this.query = query;
            message = query.Message;

            if (message is null)
                throw new NullReferenceException("Can't parse query without message");
        }
    }
}