using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    [TelegramBranch("")]
    public class DefaultBranch : DialogBranch<IChatMessage>
    {
        public DefaultBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }
        
        public override async Task RunAsync(
            IBranchesManager<IChatMessage> manager, 
            BufferBlock<IChatMessage> messageQueue, 
            CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await messageQueue.ReceiveAsync(token);
                await Ui.ShowTextMessage("Try /help");
            }
        }
    }
}