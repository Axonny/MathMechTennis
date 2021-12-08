using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs
{
    public abstract class DialogBranch<TMessage>
    {
        protected readonly IUi Ui;
        protected readonly IApplication Application;
        
        public abstract string Name { get; }

        protected DialogBranch(
            IUi ui,
            IApplication application)
        {
            Ui = ui;
            Application = application;
        }

        public abstract Task RunAsync(
            IBranchesManager<TMessage> manager,
            BufferBlock<TMessage> messageQueue,
            CancellationToken token);
    }
}