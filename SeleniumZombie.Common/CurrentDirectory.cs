using System.IO;
using System.Reflection;

namespace SeleniumZombie.Common
{
    public class CurrentDirectory
    {
        public static string ActualPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.Replace("file:///", ""));

        public static string CreateFullFilePath(string filePath)
        {
            return Path.Combine(ActualPath, filePath);
        }
    }
}
