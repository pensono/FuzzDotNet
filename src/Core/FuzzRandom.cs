﻿using System;
using System.Collections;
using System.Collections.Generic;
using FuzzDotNet.Core.Utilities;

namespace FuzzDotNet.Core
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
    }
}