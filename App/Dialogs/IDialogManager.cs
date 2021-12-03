using System.Diagnostics.CodeAnalysis;

namespace DmLab2
{
    [SuppressMessage("ReSharper", "CommentTypo")]
    public interface IDialogManager
    {
        //Только получение/хранение сообщений, выбор нужной ветки, никакого анализа сообщения
        
        IDialogBranch CurrentBranch { get; }
        void HandleMessage(IUserMessage message);
        void SetCurrentBranchByName(string name, bool isNeedToCancelPrevBranch = true);
    }
}