using System;

namespace App
{
    public static class Program
    {
        private static void Main()
        {
            var tgBot = new TelegramBot(Environment.GetEnvironmentVariable(
                "TgBotToken", 
                EnvironmentVariableTarget.User));
            Console.ReadLine();
        }
    }
}