namespace App.Dialogs.ChatDialog
{
    public interface IChatMessage
    {
        public string Username { get; }
        public long ChatId { get; }
        public string Text { get; }
    }
}