using System;

namespace FuzzDotNet
{
    [AttributeUsage(System.AttributeTargets.Method)]  
    public class FuzzTestAttribute : Attribute
    {
    }
}
