using System;
using System.Collections.Generic;
using System.Text;
using FuzzDotNet.Formatting;
using System.Threading.Tasks;
using System.IO;

namespace FuzzDotNet.Notification
{
    public class FileNotifier : Notifier
    {
        private Func<Counterexample, string> _pathGenerator;

        public FileNotifier(IFormatter formatter, Func<Counterexample, string> pathGenerator)
            : base(formatter)
        {
            _pathGenerator = pathGenerator;
        }

        public override async Task NotifyCounterexampleAsync(Counterexample counterexample)
        {
            var formatted = Formatter.Format(counterexample);
            var path = _pathGenerator(counterexample);

            await File.WriteAllTextAsync(path, formatted, Encoding.UTF8);
        }
    }
}