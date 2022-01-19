﻿using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TableTennisDomain.Infrastructure;

namespace App.Dialogs.ChatDialog.Branches
{
    [TelegramBranch("/set_result")]
    public class MatchRegistrationBranch : DialogBranch<IChatMessage>
    {
        private static readonly Regex matchResultRegex = new(@"@(\w+) (\d+)[:;., ](\d+)");

        public override string Name => "Match";

        public MatchRegistrationBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }

        public override async Task RunAsync(
            IBranchesManager<IChatMessage> manager,
            BufferBlock<IChatMessage> messageQueue,
            CancellationToken token)
        {
            await messageQueue.ReceiveAsync(token); //first message is command
            
            await Ui.ShowMessage("Send Match result. Format @opponent yourScore:opponentScore");
            var message = await messageQueue.ReceiveAsync(token);

            var groups = matchResultRegex.Match(message.Text).Groups;
            var player1 = message.Username;

            if (groups[1].Value == "" || groups[2].Value == "" || groups[3].Value == "")
            {
                await Ui.ShowMessage("Wrong format");
                return;
            }
            
            var player2 = groups[1].Value;
            var gamesWon1 = int.Parse(groups[2].Value);
            var gamesWon2 = int.Parse(groups[3].Value);

            try
            {
                var matchId = await Application.RegisterMatch(player1, player2, gamesWon1, gamesWon2);
                await Application.ConfirmMatchBy(player1, matchId);
                
                await Ui.ShowMessage("Match registration is completed!");
            }
            catch (RepositoryException)
            {
                await Ui.ShowMessage("It's not possible to register match. " +
                                     "Maybe your opponent is not registered.");
            }
        }
    }
}