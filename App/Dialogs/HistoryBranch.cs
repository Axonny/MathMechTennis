using System.Threading;
using System.Threading.Tasks.Dataflow;
using App;
using App.Dialogs;

namespace DmLab2
{
    [DialogBranch("History")]
    public class HistoryBranch : IDialogBranch
    {
        private readonly IUi ui;
        private readonly Application application;

        public HistoryBranch(IUi ui, Application application)
        {
            this.ui = ui;
            this.application = application;
        }
        
        public async void RunAsync(
            IDialogManager dialogManager, 
            BufferBlock<IUserMessage> messageQueue, 
            CancellationToken token)
        {
            ui.ShowMessage("Enter period");
            
            var period = int.Parse((await messageQueue.ReceiveAsync(token)).Text);
            var matches = new string[period];
            
            for (var i = 0; i < period; i++)
                matches[i] = "Match" + i;
            
            ui.ShowMessage(string.Join('\n', matches));
            
            dialogManager.SetCurrentBranchByName("Start");
        }
    }
}