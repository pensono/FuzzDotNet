using FuzzDotNet.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FuzzDotNet.Core.Generation
{
    /// <summary>
    /// Generates an infinite series of fuzz arguments.
    /// </summary>
    public class FuzzArgumentGenerator
    {
        private IEnumerable<(IGenerator Generator, Type ParameterType)> _argumentGenerators;
        private FuzzRandom _seedGenerator;
        private IFuzzProfile _profile;

        private object?[]? _currentArguments = null;

        public object?[] CurrentArguments {
            get
            {
                if (_currentArguments == null)
                {
                    throw new InvalidOperationException();
                }

                return _currentArguments;
            }
        }

        private int? _currentSeed = null;
        public int CurrentSeed { 
            get 
            {
                if (_currentSeed is int i)
                {
                    return i;
                }

                throw new InvalidOperationException();
            }
        }

        public FuzzArgumentGenerator(IFuzzProfile profile, MethodInfo method, int? seed = null)
        {
            _profile = profile;

            _seedGenerator = seed is int s ? new FuzzRandom(s) : new FuzzRandom();

            _argumentGenerators = method.GetParameters()
            .Select(parameter => {
                var generator = GetGenerator(profile, parameter);

                return (Generator: generator, parameter.ParameterType);
            }).ToList();
        }

        public void MoveNext()
        {
            var seed = _seedGenerator.Uniform(int.MinValue, int.MaxValue);

            var arguments = _argumentGenerators
                    .Select(g => g.Generator.Generate(_profile, g.ParameterType, new FuzzRandom(seed)))
                    .ToArray();

            UpdateCurrent(arguments, seed);
        }

        private static IGenerator GetGenerator(IFuzzProfile profile, ParameterInfo parameter)
        {
            var generatorAttribute = parameter.GetCustomAttribute<Generator>();

            if (generatorAttribute != null)
            {
                if (!generatorAttribute.CanGenerate(profile, parameter.ParameterType))
                {
                    throw new IncompatibleGeneratorException($"The generator of type {generatorAttribute.GetType()} cannot generate the parameter {parameter.Name} of type {parameter.ParameterType}");
                }

                return generatorAttribute;
            }
            else
            {
                return profile.GeneratorForOrThrow(parameter.ParameterType);
            }
        }

        private void UpdateCurrent(object?[] arguments, int seed)
        {
            _currentArguments = arguments;
            _currentSeed = seed;
        }
    }
}
