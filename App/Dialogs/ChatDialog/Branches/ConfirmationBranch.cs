using System;
using System.Linq;
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
            var matchId = message.Text.Split(' ')[1];
            
            Console.WriteLine("matchId");
            
            await Application.ConfirmMatchBy(message.Username, ObjectId.Parse(matchId));

            await Ui.ShowTextMessage($"Confirmed: {matchId}");
        }
    }
}