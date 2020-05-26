using System;
using System.Collections.Generic;
using FuzzDotNet.Utilities;
using Distributions = MathNet.Numerics.Distributions;

namespace FuzzDotNet
{
    public class FuzzRandom
    {
        private readonly Random _random = new Random();

        public T Choice<T>(IList<T> items)
        {
            Check.IsTrue(items.Count > 0);

            var index = _random.Next(items.Count);
            return items[index];
        }
        
        public object? Choice(Array items)
        {
            Check.IsTrue(items.Length > 0);

            var index = _random.Next(items.Length);
            return items.GetValue(index);
        }
        
        public T Choice<T>(params T[] items)
        {
            Check.IsTrue(items.Length > 0);
        
            var index = _random.Next(items.Length);
            return items[index];
        }

        /// <summary>
        /// Uniformly choose a value from 0 to 1.
        /// </summary>
        public double Uniform()
        {
            return _random.NextDouble();
        }

        public double Poisson(double mean)
        {
            return Distributions.Poisson.Sample(_random, mean);
        }

        /// <summary>
        /// Uniformly choose a value from min to max inclusive.
        /// </summary>
        /// <param name="min">The inclusive minimum value in the range to sample from.</param>
        /// <param name="max">The inclusive maximum value in the range to sample from.</param>
        public int Uniform(int min, int max)
        {
            if (max < min) {
                throw new ArgumentException($"{nameof(min)} is less than {nameof(max)}");
            }

            return (int) (_random.NextDouble() * (max - min + 1) + min);
        }
    }
}
