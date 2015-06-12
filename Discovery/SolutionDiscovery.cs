using Devkoes.ReleaseManager.Discovery.Model;
using Devkoes.ReleaseManager.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace Devkoes.ReleaseManager.Discovery
{
    public class SolutionDiscovery
    {
        private readonly Regex _projRegex = new Regex("^Project.*?,.*?\"(?<projectPath>.*?proj)\"", RegexOptions.Multiline);

        public static SolutionDiscovery Default { get; } = new SolutionDiscovery();
        private SolutionDiscovery() { }


        public IEnumerable<Solution> Discover(string path)
        {
            var absolutePath = Path.GetFullPath(path);

            return GetSolution(absolutePath);
        }

        public void DiscoverDetails(Solution sol)
        {
            if (sol == null)
            {
                return;
            }

            DiscoverSolutionDetails(sol);
            DiscoverProjectDetails(sol);
        }

        private void DiscoverSolutionDetails(Solution sol)
        {
            var wixDisc = new WixDiscovery();
            var installers = wixDisc.Discover(sol.AbsoluteFolder);
            if (!installers.Any())
            {
                return;
            }

            sol.Installer = installers.First();
        }

        private void DiscoverProjectDetails(Solution sol)
        {
            var projs = new List<Project>();
            var solutionContent = File.ReadAllText(sol.AbsolutePath);
            var matches = _projRegex.Matches(solutionContent);

            foreach (var match in matches.OfType<Match>())
            {
                var relProjPath = Path.Combine(sol.AbsoluteFolder, match.Groups["projectPath"].Value);
                var absoluteProjPath = Path.GetFullPath(relProjPath);

                sol.Projects.Add(ProjectDiscovery.Default.Discover(absoluteProjPath));
            }
        }

        private IEnumerable<Solution> GetSolution(string absolutePath)
        {
            var slns = new List<Solution>();
            foreach (var file in Directory.GetFiles(absolutePath, "*.sln"))
            {
                slns.Add(CreateSolution(file));
            }

            foreach (var dir in Directory.GetDirectories(absolutePath))
            {
                slns.AddRange(GetSolution(dir));
            }

            return slns;
        }

        private Solution CreateSolution(string absoluteFilePath)
        {
            Trace.WriteLine($"Solution found: {absoluteFilePath}");

            string buildScriptAbsolutePath = GetBuildScriptAbsolutePath(absoluteFilePath);
            string absolutePathRelease = GetReleaseCounterPart(absoluteFilePath);
            string absoluteFolderRelease = GetAbsoluteFolderRelease(absolutePathRelease);

            return new Solution(
                absoluteFilePath,
                buildScriptAbsolutePath,
                absolutePathRelease,
                absoluteFolderRelease);
        }

        private string GetAbsoluteFolderRelease(string absolutePathRelease)
        {
            if(string.IsNullOrWhiteSpace(absolutePathRelease))
            {
                return null;
            }

            return Path.GetDirectoryName(absolutePathRelease);
        }

        private string GetBuildScriptAbsolutePath(string absoluteSolutionFilePath)
        {
            string slnFileName = Path.GetFileNameWithoutExtension(absoluteSolutionFilePath);
            string buildConfig = string.Concat(slnFileName, ".build.xml");
            string buildConfigAbsolutePath = Path.Combine(Path.GetDirectoryName(absoluteSolutionFilePath), buildConfig);
            if (File.Exists(buildConfigAbsolutePath))
            {
                return buildConfigAbsolutePath;
            }

            return null;
        }

        private string GetReleaseCounterPart(string absoluteSolutionFilePath)
        {
            if (!absoluteSolutionFilePath.StartsWith(EnvironmentVariables.ProjectSourcePath))
            {
                return null;
            }

            var releasePath = absoluteSolutionFilePath.Replace(EnvironmentVariables.ProjectSourcePath, EnvironmentVariables.ReleaseSourcePath);
            if (File.Exists(releasePath))
            {
                return releasePath;
            }

            return null;
        }
    }
}
