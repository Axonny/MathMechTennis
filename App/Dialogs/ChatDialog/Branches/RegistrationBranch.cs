using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    [TelegramBranch("/start")]
    public class RegistrationBranch : DialogBranch<IChatMessage>
    {
        public override string Name => "Start";
        
        public RegistrationBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }
        
        public override async Task RunAsync(
            IBranchesManager<IChatMessage> manager, 
            BufferBlock<IChatMessage> messageQueue, 
            CancellationToken token)
        {
            var message = await messageQueue.ReceiveAsync(token);
            await Application.RegisterPlayer(message.Username, message.ChatId);
            await Ui.ShowMessage("Registration is completed. Try /help");
            
            manager.StartBranchByName("Default");
        }
    }
}