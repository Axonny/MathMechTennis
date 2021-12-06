using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    [ChatBranch("/history")]
    public class HistoryBranch : DialogBranch<IChatMessage>
    {
        public override string Name => "History";
        
        public HistoryBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }

        public override async Task RunAsync(
            IDialogGraph<IChatMessage> dialogGraph, 
            BufferBlock<IChatMessage> messageQueue, 
            CancellationToken token)
        {
            await Ui.ShowMessage("Enter period");
            var command = (await messageQueue.ReceiveAsync(token)).Text;
            var text = (await messageQueue.ReceiveAsync(token)).Text;
            var period = int.Parse(text);
            var matches = new string[period];
            
            for (var i = 0; i < period; i++)
            {
                matches[i] = "Match" + text;
                await Ui.ShowMessage("Send something to continue");
                text = (await messageQueue.ReceiveAsync(token)).Text;
            }
            
            await Ui.ShowMessage(string.Join('\n', matches));
            
            dialogGraph.StartBranchByName("Default");
        }
    }
}