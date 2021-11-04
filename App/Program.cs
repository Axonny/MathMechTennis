using System;

namespace App
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var p = new TableTennisDomain.Player("Pl", 100);
            Console.WriteLine(p);
        }
    }
}