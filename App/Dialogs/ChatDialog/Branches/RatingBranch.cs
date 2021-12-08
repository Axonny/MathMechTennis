using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    [ChatBranch("/show_rating")]
    public class RatingBranch : DialogBranch<IChatMessage>
    {
        public override string Name => "Rating";
        
        public RatingBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }
        
        public override async Task RunAsync(
            IBranchesManager<IChatMessage> manager, 
            BufferBlock<IChatMessage> messageQueue, 
            CancellationToken token)
        {
            var command = await messageQueue.ReceiveAsync(token);

            await Ui.ShowMessage($"You rating is {await Application.GetRatingValue(command.Username)}");
            manager.StartBranchByName("Default");
        }
    }
}