﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace App.Dialogs.ChatDialog
{
    public static class ChatDialogGraphBuilder
    {
        private static readonly (ConstructorInfo branchType, ChatBranchAttribute attribute)[] branchesInfos;

        static ChatDialogGraphBuilder()
        {
            var branchesTypes = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .Where(type => type.BaseType == typeof(DialogBranch<IChatMessage>))
                .Where(type => type.GetCustomAttribute<ChatBranchAttribute>() is not null);
            
            branchesInfos = branchesTypes
                .Select(type => (
                    type.GetConstructor(new[] {typeof(IUi), typeof(IApplication)}), 
                    type.GetCustomAttribute<ChatBranchAttribute>()))
                .ToArray();
        }

        public static ChatDialogGraph Build(
            IUi ui, 
            IApplication application, 
            string startBranch)
        {
            var branchByName = new Dictionary<string, DialogBranch<IChatMessage>>();
            var branchByCommand = new Dictionary<string, DialogBranch<IChatMessage>>();

            foreach (var (constructor, attribute) in branchesInfos)
            {
                var branch = (DialogBranch<IChatMessage>)constructor.Invoke(new object[] {ui, application});
                branchByName[branch.Name] = branch;
                branchByCommand[attribute.CommandName] = branch;
            }

            return new ChatDialogGraph(ui, startBranch, branchByName, branchByCommand);
        }
    }
}