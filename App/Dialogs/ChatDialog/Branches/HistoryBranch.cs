using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    [TelegramBranch("/history")]
    public class HistoryBranch : DialogBranch<IChatMessage>
    {
        public override string Name => "History";
        
        public HistoryBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }

        public override async Task RunAsync(
            IBranchesManager<IChatMessage> manager, 
            BufferBlock<IChatMessage> messageQueue, 
            CancellationToken token)
        {
            await messageQueue.ReceiveAsync(token);
            await Ui.ShowTextMessage("Enter number of matches");

            var message = await messageQueue.ReceiveAsync(token);
            var matches = await Application.GetLastMatchesInfos(message.Username, int.Parse(message.Text));

            await BranchHelpers.ShowInParts(Ui, matches, messageQueue, token);
        }
    }
}