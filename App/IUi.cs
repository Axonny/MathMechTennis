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

        Task ShowMessageWithTwoButtonFor(string messageText,
            string firstButtonText,
            string secondButtonText,
            string firstCallbackData,
            string secondCallbackData,
            string receiverNickname);
    }
}