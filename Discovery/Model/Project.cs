﻿using System.Collections.Generic;

namespace Devkoes.ReleaseManager.Discovery.Model
{
    public class Project : FileBase
    {
        public PackageConfig PackageConfig { get; private set; }

        public Nuspec Nuspec { get; private set; }

        public IEnumerable<AssemblyReference> References { get; private set; }

        public IEnumerable<ReferenceOverviewItem> ReferenceOverview { get; private set; }

        public Project(
            string absolutePath, 
            Nuspec nuspec,
            PackageConfig packageConfig,
            IEnumerable<AssemblyReference> references,
            IEnumerable<ReferenceOverviewItem> refOverview) : base(absolutePath)
        {
            Nuspec = nuspec;
            PackageConfig = packageConfig;
            References = references;
            ReferenceOverview = refOverview;
        }
    }
}
