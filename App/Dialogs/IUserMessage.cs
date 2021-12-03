namespace DmLab2
{
    public interface IUserMessage
    {
        public string Username { get; }
        public string Text { get; }
    }

    public record UserMessage(string Username, string Text) : IUserMessage;
}