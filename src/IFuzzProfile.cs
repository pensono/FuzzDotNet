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
        /// <returns>
        /// The generator, or null if that type cannot be generated.
        /// </returns>
        IGenerator? GeneratorFor(Type type);

        INotifier Notifier { get; }
    }
}
