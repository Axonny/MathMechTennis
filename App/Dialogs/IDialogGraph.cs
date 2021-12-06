using System.Diagnostics.CodeAnalysis;

namespace App.Dialogs
{
    [SuppressMessage("ReSharper", "CommentTypo")]
    public interface IDialogGraph<TMessage>
    {
        DialogBranch<TMessage> CurrentBranch { get; }
        void HandleMessage(TMessage message);
        void StartBranchByName(string name, bool isNeedToCancelPrevBranch = true);
    }
}