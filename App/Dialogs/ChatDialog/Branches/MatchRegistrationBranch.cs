using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using App.Dialogs.ChatDialog;

namespace App.Dialogs
{
    [ChatBranch("/set_result")]
    public class MatchRegistrationBranch : DialogBranch<IChatMessage>
    {
        private static readonly Regex matchResultRegex = new(@"@(\w+) (\d)[:;., ](\d)");

        public override string Name => "Match";

        public MatchRegistrationBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }

        public override async Task RunAsync(
            IDialogGraph<IChatMessage> graph,
            BufferBlock<IChatMessage> messageQueue,
            CancellationToken token)
        {
            await Ui.ShowMessage("Send Match result. Format @opponent yourScore:opponentScore");
            
            await messageQueue.ReceiveAsync(token); //first message is command
            
            var message = await messageQueue.ReceiveAsync(token);

            var groups = matchResultRegex.Match(message.Text).Groups;
            var player1 = message.SenderUsername;

            if (groups[1].Value == "" || groups[2].Value == "" || groups[3].Value == "")
            {
                await Ui.ShowMessage("Wrong syntax");
                graph.StartBranchByName("Default");
                return;
            }
            
            var player2 = groups[1].Value;
            var gamesWon1 = int.Parse(groups[2].Value);
            var gamesWon2 = int.Parse(groups[3].Value);
            
            await Application.RegisterMatch(player1, player2, gamesWon1, gamesWon2);
            
            await Ui.ShowMessage("Match registration is completed!");
            graph.StartBranchByName("Default");
        }
    }
}