using System.Collections.Generic;
using System.Linq;

namespace Devkoes.ReleaseManager.Discovery.Model
{
    public class Solution : FileBase, IComparer<Solution>
    {
        public IList<Project> Projects { get; private set; }
        public string BuildScriptAbsolutePath { get; set; }
        public Wix Installer { get; set; }

        public Solution(
            string absolutePath, 
            string buildScriptAbsolutePath) : this(absolutePath, buildScriptAbsolutePath, null, null)
        {

        }

        public Solution(
            string absolutePath,
            string buildScriptAbsolutePath,
            Wix installer) : this(absolutePath, buildScriptAbsolutePath, installer, null)
        {

        }

        public Solution(
            string absolutePath,
            string buildScriptAbsolutePath,
            IEnumerable<Project> projects) : this(absolutePath, buildScriptAbsolutePath, null, projects)
        {

        }

        public Solution(
            string absolutePath,
            string buildScriptAbsolutePath,
            Wix installer,
            IEnumerable<Project> projects) : base(absolutePath)
        {
            BuildScriptAbsolutePath = buildScriptAbsolutePath;
            Projects = new List<Project>(projects ?? Enumerable.Empty<Project>());
            Installer = installer;

        }

        public int Compare(Solution x, Solution y)
        {
            return string.Compare(x?.Name, y?.Name);
        }
    }
}
