using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DmLab2
{
    [SuppressMessage("ReSharper", "CommentTypo")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DialogManager : IDialogManager
    {
        private readonly Dictionary<string, IDialogBranch> branchByName;
        private BufferBlock<IUserMessage> messageQueue;
        private CancellationTokenSource currentBranchCancellationTokenSource = new();

        public IDialogBranch CurrentBranch { get; private set; }
        public bool IsStarted { get; private set; }

        public DialogManager(Dictionary<string, IDialogBranch> branchByName, string startBranchName)
        {
            this.branchByName = branchByName;
            CurrentBranch = this.branchByName[startBranchName];
        }

        public void Start()
        {
            StartBranch(CurrentBranch, false);
            IsStarted = true;
        }

        public void HandleMessage(IUserMessage message)
        {
            if (!IsStarted)
                throw new InvalidOperationException("You need to invoke Start()");
            
            messageQueue.Post(message);
        }
 
        public void SetCurrentBranchByName(string name, bool isNeedToCancelPrevBranch = true)
        {
            StartBranch(branchByName[name], isNeedToCancelPrevBranch);
        }

        private void StartBranch(IDialogBranch branch, bool isNeededToCancelPrevBranch = true)
        {
            if (isNeededToCancelPrevBranch)
                currentBranchCancellationTokenSource.Cancel();
            
            currentBranchCancellationTokenSource = new CancellationTokenSource();
            CurrentBranch = branch;
            messageQueue = new BufferBlock<IUserMessage>();

            Task.Run(() =>
            {
                try
                {
                    CurrentBranch.RunAsync(this, messageQueue, currentBranchCancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                { 
                    Console.WriteLine("Task was cancelled");
                }
            });
        }
    }
}