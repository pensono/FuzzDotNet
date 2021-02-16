using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Formatting;
using FuzzDotNet.Core.Generation;
using FuzzDotNet.Core.Utilities;
using Xunit;
using Xunit.Sdk;

namespace FuzzDotNet.Xunit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    [DataDiscoverer("FuzzDotNet.Xunit.FuzzDataAttributeDiscoverer", "FuzzDotNet.Xunit")]
    public class FuzzDataAttribute : DataAttribute
    {
        private IFuzzProfile? _fuzzProfile;

        public IFuzzProfile FuzzProfile
        {
            // Note that this requirement prohibits parameterized fuzz profiles
            // The best way to do this is to subclass FuzzTestMethodAttribute and forward the parameters from that to the fuzz profile
            get => ReflectionExtensions.GetDefaultConstructableParameter(ref _fuzzProfile, FuzzProfileType, CreateFuzzProfile);
            set => _fuzzProfile = value;
        }

        /// <summary>
        /// The type of the fuzz profile to use.
        /// </summary>
        /// <remarks>
        /// Example usage:
        /// <code>
        /// [Theory]
        /// [FuzzData(FuzzProfileType = typeof(CustomFuzzProfile)]
        /// public void FuzzTest(int value)
        /// {
        ///     // ...
        /// }
        /// </code>
        /// </remarks>
        public Type? FuzzProfileType { get; set; }

        public int Iterations { get; set; } = 20;

        public override IEnumerable<object?[]> GetData(MethodInfo testMethod)
        {
            var argumentGenerator = new FuzzArgumentGenerator(FuzzProfile, testMethod);

            for (int i = 0; i < Iterations; i++)
            {
                argumentGenerator.MoveNext();
                yield return argumentGenerator.CurrentArguments;
            }
        }

        /// <summary>
        /// Creates a fuzz profile to be used during fuzz tests.
        /// </summary>
        /// <remarks>
        /// This may be overridden to use a custom fuzz profile anywhere a subclass of this attribute is used.
        /// </remarks>
        /// <returns>The fuzz profile.</returns>
        protected virtual IFuzzProfile CreateFuzzProfile()
        {
            return new NaughtyFuzzProfile();
        }
    }
}
