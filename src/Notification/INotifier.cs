using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzDotNet.Notification
{
    public interface INotifier
    {
        /// <summary>
        /// Called any time a counterexample is discovered.
        /// </summary>
        /// <param name="counterexample">The counterexample that was discovered.</param>
        public void NotifyCounterexample(Counterexample counterexample) { }
    }
}
