using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuzzDotNet.Core.Notification
{
    public class CompositeNotifier : INotifier
    {
        private readonly IEnumerable<INotifier> _notifiers;

        public CompositeNotifier(params INotifier[] notifiers)
        {
            _notifiers = notifiers;
        }

        public CompositeNotifier(IEnumerable<INotifier> notifiers)
        {
            _notifiers = notifiers.ToList();
        }

        public async Task NotifyCounterexampleAsync(Counterexample counterexample)
        {
            var notificationTasks = _notifiers.Select(n => n.NotifyCounterexampleAsync(counterexample));

            await Task.WhenAll(notificationTasks.ToArray());
        }
    }
}
