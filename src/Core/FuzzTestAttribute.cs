using System;

namespace FuzzDotNet.Core
{
    [AttributeUsage(AttributeTargets.Method)]  
    public class FuzzTestAttribute : Attribute
    {
    }
}
