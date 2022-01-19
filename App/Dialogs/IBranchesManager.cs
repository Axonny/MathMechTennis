using System.Diagnostics.CodeAnalysis;

namespace App.Dialogs
{
    [SuppressMessage("ReSharper", "CommentTypo")]
    public interface IBranchesManager<TMessage>
    {
        DialogBranch<TMessage> CurrentBranch { get; }
        void HandleMessage(TMessage message);
        public string GetCommandByBranch<TBranch>();
    }
}