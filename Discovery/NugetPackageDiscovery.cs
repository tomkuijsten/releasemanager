using Devkoes.ReleaseManager.Discovery.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Devkoes.ReleaseManager.Discovery
{
    public class NugetPackageDiscovery
    {
        private const string PACKAGE_FILENAME = "packages.config";

        public static NugetPackageDiscovery Default { get; } = new NugetPackageDiscovery();

        private NugetPackageDiscovery() { }

        public PackageConfig Discover(string absoluteFolder)
        {
            string absolutePackagePath = Path.Combine(absoluteFolder, PACKAGE_FILENAME);

            if (!File.Exists(absolutePackagePath))
            {
                Trace.WriteLine("No packages file found.");

                return null;
            }

            Trace.WriteLine("Parsing packages...");

            return new PackageConfig(
                absolutePackagePath,
                ExtractPackageItems(absolutePackagePath));
        }

        public IEnumerable<PackageItem> ExtractPackageItems(string absolutePackagePath)
        {
            var pItems = new List<PackageItem>();
            try
            {
                var packageItems =
                        from el in XElement.Load(absolutePackagePath).Elements("package")
                        select new PackageItem(
                            (string)el.Attribute("id"),
                            (string)el.Attribute("version"));

                foreach (var packageItem in packageItems)
                {
                    Trace.WriteLine($"Package: {packageItem}");

                    pItems.Add(packageItem);
                }
            }
            catch (Exception)
            {
                Trace.WriteLine($"Package parsing xml error: {absolutePackagePath}");
            }

            return pItems;
        }
    }
}
