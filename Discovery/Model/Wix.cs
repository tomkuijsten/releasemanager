using System.Collections.Generic;

namespace Devkoes.ReleaseManager.Discovery.Model
{
    public class Wix : FileBase
    {
        public IEnumerable<WixFileReference> FileReferences { get; private set; }

        public Wix(string fileAbsolutePath, IEnumerable<WixFileReference> fileReferences) : base(fileAbsolutePath)
        {
            FileReferences = fileReferences;
        }
    }
}
