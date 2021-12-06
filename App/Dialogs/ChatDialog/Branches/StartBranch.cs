using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    [ChatBranch("/start")]
    public class StartBranch : DialogBranch<IChatMessage>
    {
        public override string Name => "Start";
        
        public StartBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }
        
        public override async Task RunAsync(
            IDialogGraph<IChatMessage> dialogGraph, 
            BufferBlock<IChatMessage> messageQueue, 
            CancellationToken token)
        {
            var message = await messageQueue.ReceiveAsync(token);
            await Application.RegisterPlayer(message.SenderUsername, message.ChatId);
            await Ui.ShowMessage("Registration is completed. Try /help");
            
            dialogGraph.StartBranchByName("Default");
        }
    }
}