using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devkoes.ReleaseManager.Model
{
    public class EnvironmentVariables
    {
        public static string ProjectSourcePath { get { return @"c:\_projects"; } }
        public static string ReleaseSourcePath { get { return @"c:\_releases"; } }
    }
}
