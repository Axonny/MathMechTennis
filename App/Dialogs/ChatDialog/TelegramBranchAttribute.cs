using System;

namespace App.Dialogs.ChatDialog
{
    public class TelegramBranchAttribute : Attribute
    {
        public string CommandName { get; }
        public string Description { get; }

        public TelegramBranchAttribute(string commandName, string description = null)
        {
            CommandName = commandName;
            Description = description;
        }
    }
}