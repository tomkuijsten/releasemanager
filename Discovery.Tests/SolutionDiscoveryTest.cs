using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Devkoes.ReleaseManager.Discovery.Tests
{
    [TestClass]
    public class SolutionDiscoveryTest
    {
        [TestMethod]
        public void Discover()
        {
            var x = new SolutionDiscovery();

            var solutions = x.Discover(@"c:\_projects\Portal");

            foreach (var s in solutions)
            {
                x.DiscoverDetails(s);
            }

            Assert.IsTrue(solutions.Any());
        }
    }
}
