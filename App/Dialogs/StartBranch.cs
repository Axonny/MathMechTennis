using System.Threading;
using System.Threading.Tasks.Dataflow;
using App;
using App.Dialogs;

namespace DmLab2
{
    [DialogBranch("Start")]
    public class StartBranch : IDialogBranch
    {
        private readonly IUi ui;
        private readonly Application application;

        public StartBranch(IUi ui, Application application)
        {
            this.ui = ui;
            this.application = application;
        }
        
        public async void RunAsync(
            IDialogManager dialogManager, 
            BufferBlock<IUserMessage> messageQueue,
            CancellationToken token)
        {
            while (true)
            {
                var message = await messageQueue.ReceiveAsync(token);

                //TODO: commands names can be given from constructor
                if (message.Text == "/set_result")
                {
                    dialogManager.SetCurrentBranchByName("Match");
                    break;
                }
                
                if (message.Text == "/history")
                {
                    dialogManager.SetCurrentBranchByName("History");
                    break;
                }

                if (message.Text == "/echo")
                {
                    ui.ShowMessage("This is echo: " + message.Text);
                    continue;
                }
                
                if (message.Text == "")
                    ui.ShowMessage("You can: /set_result /echo /history");
            }
        }
    }
}