using Devkoes.ReleaseManager.Discovery.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;
using System.Linq;

namespace Devkoes.ReleaseManager.Discovery
{
    public class ProjectDiscovery
    {
        public static ProjectDiscovery Default { get; } = new ProjectDiscovery();

        private ProjectDiscovery() { }

        public Project Discover(string absoluteProjPath)
        {
            Trace.WriteLine($"Project found: {absoluteProjPath}");

            var absoluteFolder = Path.GetDirectoryName(absoluteProjPath);

            var nuspec = NugetSpecDiscovery.Default.Discover(absoluteProjPath);
            var nugetPkg = NugetPackageDiscovery.Default.Discover(absoluteFolder);
            var assRef = AssemblyReferenceDiscovery.Default.Discover(absoluteProjPath);
            IEnumerable<ReferenceOverviewItem> refOverview = GetReferenceOverview(nuspec, nugetPkg, assRef);

            return new Project(absoluteFolder, nuspec, nugetPkg, assRef, refOverview);
        }

        private IEnumerable<ReferenceOverviewItem> GetReferenceOverview(Nuspec nuspec, PackageConfig nugetPkg, IEnumerable<AssemblyReference> assRef)
        {
            if(nugetPkg == null || assRef == null)
            {
                return Enumerable.Empty<ReferenceOverviewItem>();
            }

            var allNames =
                nugetPkg.Items.Select(n => n.Name).
                Union(assRef.Select(a => a.Name)).
                Distinct();

            if (nuspec != null)
            {
                allNames = allNames.Union(nuspec.Dependencies.Select(d => d.Name)).Distinct();

                var x =
                    from n in allNames
                    join s in nuspec.Dependencies on n equals s.Name into nuspecGroup
                    from sItem in nuspecGroup.DefaultIfEmpty(new PackageItem(n, null))
                    join p in nugetPkg.Items on n equals p.Name into nupkgGroup
                    from pItem in nupkgGroup.DefaultIfEmpty(new PackageItem(n, null))
                    join r in assRef on n equals r.Name into assGroup
                    from rItem in assGroup.DefaultIfEmpty(new AssemblyReference(n, null))
                    select new ReferenceOverviewItem()
                    {
                        AssemblyVersion = rItem.VersionText,
                        Name = n,
                        NugetPackageVersion = pItem.VersionText,
                        NugetSpecVersion = sItem.VersionText
                    };

                return x.ToArray();
            }
            else
            {
                var x =
                    from n in allNames
                    join p in nugetPkg.Items on n equals p.Name into nupkgGroup
                    from pItem in nupkgGroup.DefaultIfEmpty(new PackageItem(n, null))
                    join r in assRef on n equals r.Name into assGroup
                    from rItem in assGroup.DefaultIfEmpty(new AssemblyReference(n, null))
                    select new ReferenceOverviewItem()
                    {
                        AssemblyVersion = rItem.VersionText,
                        Name = n,
                        NugetPackageVersion = pItem.VersionText
                    };

                return x.ToArray();
            }

        }
    }
}
