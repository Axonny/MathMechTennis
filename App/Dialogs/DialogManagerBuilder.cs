using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DmLab2;
using Ninject;

namespace App.Dialogs
{
    public class DialogManagerBuilder
    {
        private static readonly (Type branchType, DialogBranchAttribute attribute)[] branchesInfos;

        static DialogManagerBuilder()
        {
            var branchesTypes = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .Where(type => type.GetInterfaces().Contains(typeof(IDialogBranch)));
            
            branchesInfos = branchesTypes
                .Select(type =>
                {
                    var attribute = type.GetCustomAttribute<DialogBranchAttribute>() 
                                    ?? throw new CustomAttributeFormatException(
                                        $"{type.FullName} doesn't " +
                                        $"have attribute: {nameof(DialogBranchAttribute)}");
                    
                    return (type, attribute);
                })
                .ToArray();
        }

        public static DialogManager CreateManager(
            StandardKernel kernel, 
            string startBranch)
        {
            var branchByName = new Dictionary<string, IDialogBranch>();
            
            foreach (var (branchType, attribute) in branchesInfos)
            {
                var branch = (IDialogBranch)kernel.Get(branchType);
                branchByName[attribute.Name] = branch;
            }

            return new DialogManager(branchByName, startBranch);
        }
    }
}