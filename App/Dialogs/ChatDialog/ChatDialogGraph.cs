﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App.Dialogs.ChatDialog
{
    [SuppressMessage("ReSharper", "CommentTypo")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ChatDialogGraph: IDialogGraph<IChatMessage>
    {
        private static readonly Regex CommandRegex = new(@"^/\w+");
        
        private readonly Dictionary<string, DialogBranch<IChatMessage>> branchByName;
        private readonly Dictionary<string, DialogBranch<IChatMessage>> branchByCommand;
        private BufferBlock<IChatMessage> messageQueue;
        private CancellationTokenSource currentBranchCancellationTokenSource = new();
        
        public DialogBranch<IChatMessage> CurrentBranch { get; private set; }
        public IUi Ui { get; }

        public ChatDialogGraph(
            IUi ui,
            string startBranchName,
            Dictionary<string, DialogBranch<IChatMessage>> branchByName,
            Dictionary<string, DialogBranch<IChatMessage>> branchByCommand)
        {
            Ui = ui;
            this.branchByName = branchByName;
            this.branchByCommand = branchByCommand;
            
            if (startBranchName is null) 
                return;
            
            CurrentBranch = this.branchByName[startBranchName];
            StartBranch(CurrentBranch);
        }

        public async void HandleMessage(IChatMessage message)
        {
            var text = message.Text;
            if (text is not null && CommandRegex.IsMatch(text))
            {
                if (text == "/help")
                    await ShowHelp();
                else
                {
                    if (branchByCommand.TryGetValue(text, out var branch))
                    {
                        StartBranch(branch);
                        messageQueue.Post(message);
                    }
                    else
                        await Ui.ShowMessage("Unknown command");
                }

                return;
            }

            if (CurrentBranch is null)
                return;

            messageQueue.Post(message);
        }
 
        public void StartBranchByName(string name, bool isNeedToCancelPrevBranch = true)
        {
            StartBranch(branchByName[name], isNeedToCancelPrevBranch);
        }

        public async Task ShowHelp()
        {
            var text = string.Join("\n", branchByCommand.Keys.Where(key => key != "" && key != "/start"));
            
            await Ui.ShowMessage(text + "\n/help");
        }

        private async void StartBranch(
            DialogBranch<IChatMessage> branch, 
            bool isNeededToCancelPrevBranch = true)
        {
            if (isNeededToCancelPrevBranch)
                currentBranchCancellationTokenSource.Cancel();
            
            currentBranchCancellationTokenSource = new CancellationTokenSource();
            CurrentBranch = branch;
            messageQueue = new BufferBlock<IChatMessage>();

            try
            {
                //No await - no exceptions
                await CurrentBranch.RunAsync(this, messageQueue, currentBranchCancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                //Console.WriteLine("Task was cancelled");
            }
        }
    }
}