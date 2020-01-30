using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test
{
    [TestClass]
    public class AdapterTest
    {
        public static int DiscoveryCount { get; private set; } = 0;

        private class TestDiscoveryClass {
            [FuzzTest]
            public void Method() {
                DiscoveryCount++;
            }
        }

        [TestMethod]
        public void TestDiscovery() {
            var adapter = new Adapter();
            adapter.RunTests(typeof(TestDiscoveryClass));

            Assert.AreEqual(1, DiscoveryCount);
        }
    }
}
