using System.Threading;
using System.Threading.Tasks.Dataflow;
using App;
using App.Dialogs;

namespace DmLab2
{
    [DialogBranch("Match")]
    public class MatchRegistrationBranch : IDialogBranch
    {
        private readonly IUi ui;
        private readonly Application application;

        public MatchRegistrationBranch(IUi ui, Application application)
        {
            this.ui = ui;
            this.application = application;
        }
        
        public async void RunAsync(
            IDialogManager manager, 
            BufferBlock<IUserMessage> messageQueue,
            CancellationToken token)
        {
            ui.ShowMessage("Enter @Opponent");
            
            var message = await messageQueue.ReceiveAsync(token);
            var user1 = message.Username;

            if (!message.Text.Contains('@'))
            {
                ui.ShowMessage("Wrong name syntax");
                manager.SetCurrentBranchByName("Start");
                return;
            }

            var user2 = message.Text;
            
            ui.ShowMessage("Enter match result s1:s2");
            message = await messageQueue.ReceiveAsync(token);
            
            if (!message.Text.Contains(':'))
            {
                ui.ShowMessage("Wrong match result syntax");
                manager.SetCurrentBranchByName("Start");
                return;
            }

            application.RegisterMatch(user1, user2, message.Text);
            ui.ShowMessage("Match registration is completed!");
            
            manager.SetCurrentBranchByName("Start");
        }
    }
}