using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using App.Dialogs.ChatDialog.Branches;

namespace App.Dialogs.ChatDialog
{
    [SuppressMessage("ReSharper", "CommentTypo")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class TelegramBranchManager: IBranchesManager<IChatMessage>
    {
        private static readonly Regex CommandRegex = new(@"^/\w+");
        
        private readonly Dictionary<Type, DialogBranch<IChatMessage>> branchByType;
        private readonly Dictionary<DialogBranch<IChatMessage>, TelegramBranchAttribute> infoByBranch;
        private readonly Dictionary<string, DialogBranch<IChatMessage>> branchByCommand;
        private BufferBlock<IChatMessage> messageQueue;
        private CancellationTokenSource currentBranchCancellationTokenSource = new();
        
        public DialogBranch<IChatMessage> CurrentBranch { get; private set; }
        public IUi Ui { get; }

        public TelegramBranchManager(
            IUi ui,
            Type startBranchType,
            Dictionary<Type, DialogBranch<IChatMessage>> branchByType,
            Dictionary<DialogBranch<IChatMessage>, TelegramBranchAttribute> infoByBranch,
            Dictionary<string, DialogBranch<IChatMessage>> branchByCommand)
        {
            Ui = ui;
            this.branchByType = branchByType;
            this.infoByBranch = infoByBranch;
            this.branchByCommand = branchByCommand;
            
            if (startBranchType is null) 
                return;
            
            CurrentBranch = this.branchByType[startBranchType];
            StartBranch(CurrentBranch);
        }

        public async void HandleMessage(IChatMessage message)
        {
            var command = message.Text?.Split(' ')[0];
            if (command is not null && CommandRegex.IsMatch(command))
            {
                if (command == "/help")
                    await ShowHelp();
                else
                {
                    if (branchByCommand.TryGetValue(command, out var branch))
                    {
                        StartBranch(branch);
                        messageQueue.Post(message);
                    }
                    else
                        await Ui.ShowTextMessage("Unknown command");
                }

                return;
            }

            if (CurrentBranch is null)
                throw new InvalidOperationException("No current branch");

            messageQueue.Post(message);
        }

        public void StartBranch<TBranch>(bool isNeedToCancelPrevBranch = true)
        {
            StartBranch(branchByType[typeof(TBranch)], isNeedToCancelPrevBranch);
        }

        public async Task ShowHelp()
        {
            var text = string.Join(
                "\n", 
                branchByCommand
                    .Where(pair => pair.Key.Contains("/") && pair.Key != "/start")
                    .Select(pair => infoByBranch[pair.Value])
                    .Select(info =>
                    {
                        var fullDescription = info.Description is null ? "" : "- " + info.Description; 
                        
                        return $"{info.CommandName} {fullDescription}";
                    }));
            
            StartBranch<DefaultBranch>();
            await Ui.ShowTextMessage(text + "\n/help");
        }

        public string GetCommandByBranch<TBranch>()
        {
            return branchByCommand
                .First(pair => ReferenceEquals(pair.Value, branchByType[typeof(TBranch)]))
                .Key;
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
                await CurrentBranch.RunAsync(this, messageQueue, currentBranchCancellationTokenSource.Token);
            }
            catch (Exception exception)
            {
                if (exception is TaskCanceledException)
                    return;

                BugReporter.SendReport(exception);
            }
            
            StartBranch<DefaultBranch>();
        }
    }
}