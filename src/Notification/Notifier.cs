using System.Threading.Tasks;
using FuzzDotNet.Formatting;

namespace FuzzDotNet.Notification
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
