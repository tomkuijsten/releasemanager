using System;

namespace Devkoes.ReleaseManager.Discovery.Model
{
    public class PackageItem
    {
        public string Name { get; private set; }
        public string VersionText { get; private set; }
        public Version Version { get; }

        public PackageItem(
            string name,
            string versionText)
        {
            this.Name = name;
            this.VersionText = versionText;
        }

        public override string ToString()
        {
            return $"{Name}, {VersionText}";
        }
    }
}
