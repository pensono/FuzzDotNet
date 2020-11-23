using System.Threading.Tasks;
using FuzzDotNet.Core.Formatting;

namespace FuzzDotNet.Core.Notification
{
    public abstract class Notifier : INotifier
    {
        protected IFormatter Formatter { get; }

        public Notifier(IFormatter formatter)
        {
            Formatter = formatter;
        }

        public abstract Task NotifyCounterexampleAsync(Counterexample counterexample);
    }
}
