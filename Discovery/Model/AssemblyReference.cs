using System;
using System.Diagnostics;

namespace Devkoes.ReleaseManager.Discovery.Model
{
    public class AssemblyReference : FileBase
    {
        public string VersionText { get; private set; }
        public Version Version { get; }

        public AssemblyReference(string absoluteFilePath, string versionText) :base(absoluteFilePath)
        {
            VersionText = versionText;

            Trace.WriteLine($"AssemblyReference created {ToString()}");
        }

        public override string ToString()
        {
            return $"{Name}, {VersionText}";
        }
    }
}
