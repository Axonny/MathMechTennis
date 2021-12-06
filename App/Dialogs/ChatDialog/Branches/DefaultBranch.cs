using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    [ChatBranch("")]
    public class DefaultBranch : DialogBranch<IChatMessage>
    {
        public override string Name => "Default";
        
        public DefaultBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }
        
        public override async Task RunAsync(
            IDialogGraph<IChatMessage> dialogGraph, 
            BufferBlock<IChatMessage> messageQueue, 
            CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await messageQueue.ReceiveAsync(token);
                await Ui.ShowMessage("Try /help");
            }
        }
    }
}