using System;
using FuzzDotNet.Generation;
using FuzzDotNet.Notification;
using FuzzDotNet.Simplification;

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

        /// <summary>
        /// Gets a simplifier for some type.
        /// </summary>
        /// <param name="type">The type to simplify.</param>
        /// <returns>The simplifier</returns>
        ISimplifier SimplifierFor(Type type);

        /// <summary>
        /// Gets the notifier.
        /// </summary>
        INotifier Notifier { get; }
    }
}
