using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace DmLab2
{
    public interface IDialogBranch
    {
        void RunAsync(
            IDialogManager dialogManager, 
            BufferBlock<IUserMessage> messageQueue, 
            CancellationToken token);
    }
}