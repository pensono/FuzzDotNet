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
using Xunit.Abstractions;
using Xunit.Sdk;

namespace FuzzDotNet.Xunit
{
    public class FuzzDataAttributeDiscoverer : DataDiscoverer
    {
        public override bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod) => false;
    }
}
