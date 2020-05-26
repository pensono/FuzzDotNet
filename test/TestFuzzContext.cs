﻿using System.Collections.Generic;
using FuzzDotNet.Generators;

namespace FuzzDotNet
{
    /// <summary>
    /// A test fuzz context to make things predictable.
    /// </summary>
    public class TestFuzzContext : FuzzContext
    {
        public const int GeneratedInt = 42;

        public const string GeneratedString = "The spanish inquisition";

        public override IEnumerable<IGenerator> DefaultGenerators => new List<IGenerator> {
            new ConstantGenerator(GeneratedInt),
            new ConstantGenerator(GeneratedString),
            new EnumGenerator(),
            new EnumerableGenerator(),
            new DataObjectGenerator(),
            new ConstructedObjectGenerator(),
        };
    }
}