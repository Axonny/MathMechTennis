using System;

namespace App.Dialogs.ChatDialog
{
    public class TelegramBranchAttribute : Attribute
    {
        public string CommandName { get; }
        public string Description { get; }
        
        public bool Hidden { get; }

        public TelegramBranchAttribute(string commandName, string description = null, bool hidden = false)
        {
            CommandName = commandName;
            Description = description;
            Hidden = hidden;
        }
    }
}