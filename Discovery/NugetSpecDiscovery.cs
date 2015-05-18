using Devkoes.ReleaseManager.Discovery.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Devkoes.ReleaseManager.Discovery
{
    public class NugetSpecDiscovery
    {
        public static NugetSpecDiscovery Default { get; } = new NugetSpecDiscovery();

        private NugetSpecDiscovery() { }

        public Nuspec Discover(string absoluteProjPath)
        {
            var name = Path.GetFileNameWithoutExtension(absoluteProjPath);
            var absoluteFolder = Path.GetDirectoryName(absoluteProjPath);
            var nuspecAbsolutePath = Path.Combine(absoluteFolder, string.Concat(name, ".nuspec"));

            if (!File.Exists(nuspecAbsolutePath))
            {
                return null;
            }

            return new Nuspec(
                nuspecAbsolutePath,
                ExtractDependencies(nuspecAbsolutePath));
        }

        private IEnumerable<PackageItem> ExtractDependencies(string nuspecAbsolutePath)
        {
            var pItems = new List<PackageItem>();
            try
            {
                var packageItems =
                        from el in XElement.Load(nuspecAbsolutePath).Descendants("dependency")
                        select new PackageItem(
                            (string)el.Attribute("id"),
                            (string)el.Attribute("version"));

                foreach (var packageItem in packageItems)
                {
                    Trace.WriteLine($"Nuspec dependency: {packageItem}");

                    pItems.Add(packageItem);
                }
            }
            catch (Exception)
            {
                Trace.WriteLine($"Nuspec parsing xml error: {nuspecAbsolutePath}");
            }

            return pItems;
        }
    }
}
