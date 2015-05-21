using Devkoes.ReleaseManager.Discovery.Model;
using System.Diagnostics;
using System.IO;

namespace Devkoes.ReleaseManager.Discovery
{
    public class ProjectDiscovery
    {
        public static ProjectDiscovery Default { get; } = new ProjectDiscovery();

        private ProjectDiscovery(){}

        public Project Discover(string absoluteProjPath)
        {
            Trace.WriteLine($"Project found: {absoluteProjPath}");

            var absoluteFolder = Path.GetDirectoryName(absoluteProjPath);

            return new Project(
                absoluteProjPath,
                NugetSpecDiscovery.Default.Discover(absoluteProjPath),
                NugetPackageDiscovery.Default.Discover(absoluteFolder),
                AssemblyReferenceDiscovery.Default.Discover(absoluteProjPath));
        }
    }
}
