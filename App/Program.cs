using System;
using App.Rating;
using TableTennisDomain.DomainRepositories;

namespace App
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var tgBot = new TelegramBot(Environment.GetEnvironmentVariable("TgBotToken", EnvironmentVariableTarget.User));
            Console.ReadLine();
        }
    }
}