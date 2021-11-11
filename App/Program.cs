using System;

namespace App
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var tgBot = new TelegramBot(Environment.GetEnvironmentVariable("TG_BOT", EnvironmentVariableTarget.User));
            Console.ReadLine();
        }
    }
}