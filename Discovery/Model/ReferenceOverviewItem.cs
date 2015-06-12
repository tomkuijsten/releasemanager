using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devkoes.ReleaseManager.Discovery.Model
{
    public class ReferenceOverviewItem
    {
        public string Name { get; set; }

        public string NugetPackageVersion { get; set; }
        public string NugetSpecVersion { get; set; }
        public string WixVersion { get; set; }
        public string AssemblyVersion { get; set; }
    }
}
