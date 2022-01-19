using System.Threading.Tasks;

namespace App
{
    public interface IUi
    {
        Task ShowTextMessage(string text);
        Task ShowTextMessageFor(string text, string receiverNickname);
        
        Task ShowMessageWithButtonFor(
            string messageText,
            string buttonText,
            string callbackData,
            string receiverNickname);
    }
}