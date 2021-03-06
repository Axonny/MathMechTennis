using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog.Branches
{
    public static class BranchHelpers
    {
        public static async Task ShowInParts(
            IUi ui, 
            List<string> textParts,
            BufferBlock<IChatMessage> messageQueue,
            CancellationToken token,
            (string command, string username, int count) callback = default,
            string separator = "\n-----\n",
            int partsInMessageLimit = 5)
        {
            for (var i = 0; i < textParts.Count; i += partsInMessageLimit)
            {
                if (i != 0 && i % partsInMessageLimit == 0)
                {
                    await ui.ShowTextMessage("Send something to continue");
                    await messageQueue.ReceiveAsync(token);
                }

                var tmpBuffer = new List<string>();
                for (var j = i; j < i + partsInMessageLimit && j < textParts.Count; j++)
                {
                    tmpBuffer.Add(textParts[j]);
                }

                if (!string.IsNullOrEmpty(callback.command))
                {
                    await ui.ShowMessageWithButtonFor(
                        string.Join(separator, tmpBuffer), 
                        "Continue",
                        $"/{callback.command} {callback.count}", 
                        callback.username);
                }
                else
                {
                    await ui.ShowTextMessage(string.Join(separator, tmpBuffer));
                }
            }
        }
    }
}