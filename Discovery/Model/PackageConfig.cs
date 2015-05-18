using System.Collections.Generic;

namespace Devkoes.ReleaseManager.Discovery.Model
{
    public class PackageConfig : FileBase
    {
        public IEnumerable<PackageItem> Items { get; private set; }

        public PackageConfig(
            string absolutePath,
            IEnumerable<PackageItem> items) : base(absolutePath)
        {
            this.Items = items;
        }
    }
}
