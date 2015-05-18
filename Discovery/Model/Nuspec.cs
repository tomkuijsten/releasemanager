using System.Collections.Generic;

namespace Devkoes.ReleaseManager.Discovery.Model
{
    public class Nuspec : FileBase
    {
        public IEnumerable<PackageItem> Dependencies { get; private set; }

        public Nuspec(
            string absolutePath,
            IEnumerable<PackageItem> dependencies) : base(absolutePath)
        {
            this.Dependencies = dependencies;
        }
    }
}
