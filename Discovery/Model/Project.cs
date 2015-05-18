namespace Devkoes.ReleaseManager.Discovery.Model
{
    public class Project : FileBase
    {
        public PackageConfig PackageConfig { get; private set; }

        public Nuspec Nuspec { get; private set; }

        public Project(
            string absolutePath, 
            Nuspec nuspec,
            PackageConfig packageConfig) : base(absolutePath)
        {
            this.Nuspec = nuspec;
            this.PackageConfig = packageConfig;
        }
    }
}
