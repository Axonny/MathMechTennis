using System.Threading.Tasks;

namespace App
{
    public interface IUi
    {
        Task ShowTextMessage(string text);
        Task ShowTextMessageFor(string text, long receiverChatId);
        
        Task ShowMessageWithButtonFor(
            string messageText,
            string buttonText,
            string callbackData,
            long receiverChatId);
    }
}