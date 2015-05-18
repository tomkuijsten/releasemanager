using System.IO;

namespace Devkoes.ReleaseManager.Discovery.Model
{
    public abstract class FileBase
    {
        public string Name { get; set; }
        public string AbsolutePath { get; set; }
        public string AbsoluteFolder { get; set; }

        public FileBase(string absolutePath)
        {
            this.Name = Path.GetFileNameWithoutExtension(absolutePath);
            this.AbsolutePath = absolutePath;
            this.AbsoluteFolder = Path.GetDirectoryName(absolutePath);
        }

        public override string ToString()
        {
            return $"{Name},{AbsoluteFolder}";
        }
    }
}
