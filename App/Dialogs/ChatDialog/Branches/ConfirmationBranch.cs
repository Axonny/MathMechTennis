using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MongoDB.Bson;

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
            var message = await messageQueue.ReceiveAsync(token);
            // ReSharper disable once RedundantAssignment
            var matchId = ObjectId.Empty;

            try
            {
                matchId = ObjectId.Parse(message.Text.Split(' ')[1]);
            }
            catch (IndexOutOfRangeException)
            {
                await Ui.ShowTextMessage("No matchId. Must be: /confirm <matchId>");
                return;
            }

            if (await Application.TryConfirmMatchBy(message.Username, matchId))
            {
                await Ui.ShowTextMessage($"CONFIRMED:\n{Application.GetMatchInfo(matchId)}");
            }
            else
            {
                await Ui.ShowTextMessage("CAN'T CONFIRM (MAYBE IT'S ALREADY CONFIRMED):\n" +
                                      $"{Application.GetMatchInfo(matchId)}");
            } 
        }
    }
}