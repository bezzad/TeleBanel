﻿using System;

namespace TeleBanel.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to TeleBanel (Telegram Bot Panel)");
            Console.WriteLine("Connecting to telegram server...");
            var bot = new BotManager("414286832:AAE-VQpu32juCfeOWLX33SDnyUZ_xHdfkT0");  // TestForSelfBot
            bot.StartListening();
            Console.WriteLine("Connected Successfully.");
            Console.Read();
        }
    }
}