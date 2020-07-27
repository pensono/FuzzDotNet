using System;
using FuzzDotNet.Formatting;

namespace FuzzDotNet.Notification
{
    public class ConsoleNotifier : INotifier
    {
        private IFormatter _formatter;

        public ConsoleNotifier(IFormatter formatter)
        {
            _formatter = formatter;
        }

        public void NotifyFailure(Counterexample counterexample)
        {
            var formatted = _formatter.Format(counterexample);

            Console.WriteLine($"Counter-example for {counterexample.TestMethod.TestMethodName}:");
            Console.WriteLine(formatted);
        }
    }
}
