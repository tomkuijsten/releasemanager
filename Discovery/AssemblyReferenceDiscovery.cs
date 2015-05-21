using Devkoes.ReleaseManager.Discovery.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Devkoes.ReleaseManager.Discovery
{
    public class AssemblyReferenceDiscovery
    {
        public static AssemblyReferenceDiscovery Default { get; } = new AssemblyReferenceDiscovery();

        private AssemblyReferenceDiscovery() { }

        public IEnumerable<AssemblyReference> Discover(string projectFileAbsolutePath)
        {
            if(!File.Exists(projectFileAbsolutePath))
            {
                return Enumerable.Empty<AssemblyReference>();
            }

            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var absoluteDirPath = Path.GetDirectoryName(projectFileAbsolutePath);
            var allReferences =
                from assRef in XDocument.Load(projectFileAbsolutePath).Descendants(ns + "Reference")
                let absFilePath = GetAbsoluteFilePath(absoluteDirPath, assRef.Element(ns + "HintPath"))
                where !string.IsNullOrEmpty(absFilePath)
                select new AssemblyReference(absFilePath, GetVersionFromPackagePath(absFilePath));

            return allReferences.ToArray();
        }

        private string GetVersionFromPackagePath(string absFilePath)
        {
            var match = Regex.Match(absFilePath, @"packages\\.*?(?<version>\d.\d.\d.*?)\\");
            if(!match.Success)
            {
                return null;
            }

            return match.Groups["version"].Value;
        }

        private string GetAbsoluteFilePath(string absoluteDirPath, XElement refElement)
        {
            if(refElement == null)
            {
                return null;
            }

            return Path.GetFullPath(Path.Combine(absoluteDirPath, refElement.Value));
        }
    }
}
