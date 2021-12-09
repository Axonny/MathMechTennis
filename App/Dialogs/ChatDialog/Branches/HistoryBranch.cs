using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    [TelegramBranch("/history")]
    public class HistoryBranch : DialogBranch<IChatMessage>
    {
        public override string Name => "History";
        
        public HistoryBranch(IUi ui, IApplication application) : base(ui, application)
        {
        }

        public override async Task RunAsync(
            IBranchesManager<IChatMessage> manager, 
            BufferBlock<IChatMessage> messageQueue, 
            CancellationToken token)
        {
            await messageQueue.ReceiveAsync(token);
            await Ui.ShowMessage("Enter number of matches");

            var message = await messageQueue.ReceiveAsync(token);
            var matches = await Application.GetLastMatchesInfos(message.Username, int.Parse(message.Text));
            const int ResponseLinesLimit = 5;

            for (var i = 0; i < matches.Count; i += ResponseLinesLimit)
            {
                if (i != 0 && i % ResponseLinesLimit == 0)
                {
                    await Ui.ShowMessage("Send something to continue");
                    await messageQueue.ReceiveAsync(token);
                }

                var tmpBuffer = new List<string>();
                for (var j = i; j < i + ResponseLinesLimit && j < matches.Count; j++)
                {
                    tmpBuffer.Add(matches[j]);
                }

                await Ui.ShowMessage(string.Join("\n-----\n", tmpBuffer));
            }

            manager.StartBranchByName("Default");
        }
    }
}