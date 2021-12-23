using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    [TelegramBranch("/confirm")]
    public class ConfirmationBranch : DialogBranch<IChatMessage>
    {
        public override string Name => "Confirm";
        
        public ConfirmationBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }
        
        public override async Task RunAsync(
            IBranchesManager<IChatMessage> manager, 
            BufferBlock<IChatMessage> messageQueue, 
            CancellationToken token)
        {
            var nickname = (await messageQueue.ReceiveAsync(token)).Username; 

            var unconfirmedMatches = await Application.GetUnconfirmedMatchesIds(nickname);
            var infos = await Application.GetMatchesInfos(unconfirmedMatches);

            if (infos.Count == 0)
            {
                await Ui.ShowMessage("No matches to confirm");
                manager.StartBranchByName("Default");
                return;
            }
            
            for (var i = 0; i < infos.Count; i++)
                infos[i] = $"{i + 1}. " + infos[i];

            await Ui.ShowMessage(string.Join("\n-----\n", infos));

            var message = await messageQueue.ReceiveAsync(token);

            var matchIndexesToConfirm = message.Text
                .Split(" ")
                .Select(strIndex => int.TryParse(strIndex, out var result) ? result : int.MinValue)
                .Where(index => index != int.MinValue);
            
            foreach (var index in matchIndexesToConfirm)
            {
                await Application.ConfirmMatchBy(nickname, unconfirmedMatches[index - 1]);
            }

            await Ui.ShowMessage("Confirmed");

            manager.StartBranchByName("Default");
        }
    }
}