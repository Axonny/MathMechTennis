using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    [TelegramBranch("/history", "show all my matches")]
    public class HistoryBranch : DialogBranch<IChatMessage>
    {
        public HistoryBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }
        
        public override async Task RunAsync(
            IBranchesManager<IChatMessage> manager, 
            BufferBlock<IChatMessage> messageQueue, 
            CancellationToken token)
        {
            var message = await messageQueue.ReceiveAsync(token);
            var player = message.Username;
            var offset = 0;
            
            try
            {
                offset = int.Parse(message.Text.Split(' ')[1]);
            }
            catch
            {
                // ignored
            }

            var matches = await Application.GetLastMatchesInfos(message.Username, offset + 6);
            var showMatches = matches.Skip(offset).Take(5).ToList();
            
            if (showMatches.Count == 0)
                await Ui.ShowTextMessage("You has no matches.");
            else
                await BranchHelpers.ShowInParts(Ui, showMatches, messageQueue, token,
                    matches.Count == offset + 6 ? ("history", player, offset + 5) : default);
        }
    }
}