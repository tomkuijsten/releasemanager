using System.Collections.Generic;
using System.Linq;

namespace Devkoes.ReleaseManager.Discovery.Model
{
    public class Solution : FileBase, IComparer<Solution>
    {
        public IList<Project> Projects { get; private set; }
        public string BuildScriptAbsolutePath { get; set; }
        public string AbsolutePathRelease { get; set; }
        public string AbsoluteFolderRelease { get; set; }
        public Wix Installer { get; set; }

        public Solution(
            string absolutePath, 
            string buildScriptAbsolutePath,
            string absolutePathRelease,
            string absoluteFolderRelease) : base(absolutePath)
        {
            BuildScriptAbsolutePath = buildScriptAbsolutePath;
            AbsolutePathRelease = absolutePathRelease;
            AbsoluteFolderRelease = absoluteFolderRelease;
            Projects = new List<Project>();
        }

        public int Compare(Solution x, Solution y)
        {
            return string.Compare(x?.Name, y?.Name);
        }
    }
}
