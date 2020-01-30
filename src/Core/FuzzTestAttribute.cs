using System;

namespace FuzzDotNet
{
    [AttributeUsage(AttributeTargets.Method)]  
    public class FuzzTestAttribute : Attribute
    {
    }
}
