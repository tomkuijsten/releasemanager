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
            var solutions = SolutionDiscovery.Default.Discover(@"c:\_projects\Portal");

            foreach (var s in solutions)
            {
                SolutionDiscovery.Default.DiscoverDetails(s);
            }

            Assert.IsTrue(solutions.Any());
        }
    }
}
