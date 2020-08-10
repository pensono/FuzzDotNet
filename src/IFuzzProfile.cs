using System;
using FuzzDotNet.Generation;
using FuzzDotNet.Notification;

namespace FuzzDotNet
{
    public interface IFuzzProfile
    {
        /// <summary>
        /// Gets a generator for some type.
        /// </summary>
        IGenerator GeneratorFor(Type type);

        INotifier Notifier { get; }
    }
}
