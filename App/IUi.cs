using System;

namespace App
{
    public interface IUi
    {
        void ShowMessage(string text);
        void ShowWarning(Exception exception);
    }
    
    public class ConsoleUi : IUi
    {
        public void ShowMessage(string text)
        {
            Console.WriteLine(text);
        }

        public void ShowWarning(Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}