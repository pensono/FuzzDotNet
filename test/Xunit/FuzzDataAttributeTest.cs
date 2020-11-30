using System;
using System.Linq;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Generation;
using FuzzDotNet.Test.Common;
using FuzzDotNet.Xunit;
using Xunit;

namespace FuzzDotNet.Test.Xunit
{
    public class FuzzDataAttributeTest
    {
        [Theory]
        [FuzzData]
        public void TestIterations([UniformIntGenerator(Min = 0, Max = 100)] int iterations)
        {
            var testMethod = typeof(FuzzDataAttributeTest).GetMethod(nameof(TestIterations))!;
            var dataAttribute = new FuzzDataAttribute()
            {
                Iterations = iterations,
            };

            var results = dataAttribute.GetData(testMethod);

            Assert.Equal(results.Count(), iterations);
        }

        private class UnitTestFuzzProfile : NaughtyFuzzProfile { }

        [Fact]
        public void FuzzProfileByType()
        {
            var dataAttribute = new FuzzDataAttribute()
            {
                FuzzProfileType = typeof(UnitTestFuzzProfile),
            };

            Assert.IsType<UnitTestFuzzProfile>(dataAttribute.FuzzProfile);
        }

        [Fact]
        public void FuzzProfileByTypeNotAFuzzProfile()
        {
            var annotation = new FuzzDataAttribute
            {
                FuzzProfileType = typeof(object),
            };

            Assert.Throws<ArgumentException>(() => annotation.FuzzProfile);
        }

        [Fact]
        public void FuzzProfileByValue()
        {
            var annotation = new FuzzDataAttribute
            {
                FuzzProfile = new UnitTestFuzzProfile(),
            };

            Assert.IsType<UnitTestFuzzProfile>(annotation.FuzzProfile);
        }

        private class UnitTestInheritanceFuzzProfile : NaughtyFuzzProfile { }

        private class FuzzTestMethodWithProfile : FuzzDataAttribute
        {
            protected override IFuzzProfile CreateFuzzProfile()
            {
                return new UnitTestInheritanceFuzzProfile();
            }
        }

        [Fact]
        public void FuzzProfileByInheritance()
        {
            var annotation = new FuzzTestMethodWithProfile();

            Assert.IsType<UnitTestInheritanceFuzzProfile>(annotation.FuzzProfile);
        }

        [Fact]
        public void FuzzProfileByTypeOverridesInheritance()
        {
            var annotation = new FuzzTestMethodWithProfile
            {
                FuzzProfileType = typeof(UnitTestFuzzProfile),
            };

            Assert.IsType<UnitTestFuzzProfile>(annotation.FuzzProfile);
        }
    }
}
