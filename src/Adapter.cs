using System;
using System.Reflection;
using System.Linq;

namespace FuzzDotNet
{
    public class Adapter
    {
        public void RunTests(Type testClass) {
            // How much of this can be static?

            var constructor = testClass.GetConstructor(new Type[] {});

            var instance = constructor.Invoke(new object[] {});

            foreach (var method in testClass.GetMethods()) {
                var attribute = method.GetCustomAttribute<FuzzTestAttribute>();
                if (attribute != null) {
                    // Just call the method for now. Generate the arguments later
                    method.Invoke(instance, new object[] {});
                }
            }
        }
    }
}
