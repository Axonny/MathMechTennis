using System;

namespace App.Dialogs.ChatDialog
{
    public class ChatBranchAttribute : Attribute
    {
        public string CommandName { get; }

        public ChatBranchAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }
}