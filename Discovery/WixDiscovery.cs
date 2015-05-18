using Devkoes.ReleaseManager.Discovery.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Devkoes.ReleaseManager.Discovery
{
    public class WixDiscovery
    {
        public IEnumerable<Wix> Discover(string solutionAbsoluteFolder)
        {
            List<Wix> wixes = new List<Wix>();
            var allFiles = Directory.GetFiles(solutionAbsoluteFolder, "*.wxs", SearchOption.AllDirectories);
            foreach (var file in allFiles)
            {
                var parsedWix = ParseWix(file);
                wixes.Add(parsedWix);
            }

            return wixes;
        }

        private Wix ParseWix(string file)
        {
            XNamespace wixNamespace = "http://schemas.microsoft.com/wix/2006/wi";
            var fileReferences =
                (from el in XDocument.Load(file).Descendants(wixNamespace+"File")
                select new WixFileReference((string)el.Attribute("Source"))).ToArray();

            Trace.WriteLine($"Wix found with {fileReferences.Count()} file references");

            return new Wix(file, fileReferences);
        }
    }
}
