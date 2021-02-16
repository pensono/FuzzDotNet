using System;
using System.Collections.Generic;
using System.Text;
using FuzzDotNet.Core.Formatting;
using System.Threading.Tasks;
using System.IO;

namespace FuzzDotNet.Core.Notification
{
    public class ConditionalNotifier : INotifier
    {
        private readonly INotifier _innerNotifier;

        private readonly Func<Counterexample, bool> _predicate;

        public ConditionalNotifier(INotifier notifier, Func<Counterexample, bool> predicate)
        {
            _innerNotifier = notifier;
            _predicate = predicate;
        }

        public async Task NotifyCounterexampleAsync(Counterexample counterexample)
        {
            if (_predicate(counterexample))
            {
                await _innerNotifier.NotifyCounterexampleAsync(counterexample);
            }
        }
    }
}
