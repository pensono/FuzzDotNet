using System;
using System.Collections.Generic;
using System.Text;
using FuzzDotNet.Formatting;
using System.Threading.Tasks;
using System.IO;

namespace FuzzDotNet.Notification
{
    public class ConditionalNotifier : INotifier
    {
        private INotifier _innerNotifier;

        private Func<Counterexample, bool> _predicate;

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