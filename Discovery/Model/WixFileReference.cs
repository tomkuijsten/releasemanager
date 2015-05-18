using System.IO;

namespace Devkoes.ReleaseManager.Discovery.Model
{
    public class WixFileReference
    {
        public string FileNameWithExtension { get; private set; }
        public string Source { get; private set; }

        public WixFileReference(
            string source)
        {
            Source = source;
            FileNameWithExtension = Path.GetFileName(source);
        }
    }
}
