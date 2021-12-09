using System;

namespace App.Dialogs.ChatDialog
{
    public class TelegramBranchAttribute : Attribute
    {
        public string CommandName { get; }

        public TelegramBranchAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }
}