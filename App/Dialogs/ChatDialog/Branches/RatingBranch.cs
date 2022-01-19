using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    [TelegramBranch("/my_rating", "show current rating")]
    public class RatingBranch : DialogBranch<IChatMessage>
    {
        public RatingBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }
        
        public override async Task RunAsync(
            IBranchesManager<IChatMessage> manager, 
            BufferBlock<IChatMessage> messageQueue, 
            CancellationToken token)
        {
            var command = await messageQueue.ReceiveAsync(token);

            await Ui.ShowTextMessage($"You rating is {await Application.GetRatingValue(command.Username)}");
        }
    }
}