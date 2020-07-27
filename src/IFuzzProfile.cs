using FuzzDotNet.Generation;
using FuzzDotNet.Notification;
using System;

namespace FuzzDotNet
{
    public interface IFuzzProfile
    {
        /// <summary>
        /// Gets a generator for some type.
        /// </summary>
        IGenerator GeneratorFor(Type type);

        INotifier Notifier { get; }

        object? Generate(Type type, FuzzRandom random) 
        {
            return GeneratorFor(type).Generate(this, type, random);
        }
    }
}
