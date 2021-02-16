using System;
using System.Threading.Tasks;
using FuzzDotNet.Core.Formatting;

namespace FuzzDotNet.Core.Notification
{
    public class ConsoleNotifier : Notifier
    {
        public ConsoleNotifier(IFormatter formatter)
            : base(formatter) { }

        public override Task NotifyCounterexampleAsync(Counterexample counterexample)
        {
            var formatted = Formatter.Format(counterexample);

            Console.WriteLine($"Counter-example for {counterexample.TestMethod.Name}:");
            Console.WriteLine(formatted);

            return Task.CompletedTask;
        }
    }
}
