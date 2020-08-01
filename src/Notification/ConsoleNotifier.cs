﻿using System;
using System.Threading.Tasks;
using FuzzDotNet.Formatting;

namespace FuzzDotNet.Notification
{
    public class ConsoleNotifier : Notifier
    {
        public ConsoleNotifier(IFormatter formatter) 
            : base(formatter) {}

        public override Task NotifyCounterexampleAsync(Counterexample counterexample)
        {
            var formatted = Formatter.Format(counterexample);

            Console.WriteLine($"Counter-example for {counterexample.TestMethod.TestMethodName}:");
            Console.WriteLine(formatted);

            return Task.CompletedTask;
        }
    }
}
