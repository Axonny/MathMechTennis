namespace App.Dialogs.ChatDialog
{
    public interface IChatMessage
    {
        public string SenderUsername { get; }
        public long ChatId { get; }
        public string Text { get; }
    }
}