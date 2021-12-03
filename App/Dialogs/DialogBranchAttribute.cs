using System;

namespace App.Dialogs
{
    public class DialogBranchAttribute : Attribute
    {
        public string Name { get; }

        public DialogBranchAttribute(string name)
        {
            Name = name;
        }
    }
}