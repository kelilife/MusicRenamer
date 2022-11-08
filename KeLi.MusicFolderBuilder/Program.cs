using System.IO;
using System.Linq;
using System.Reflection;

using KeLi.MusicFolderBuilder.Properties;

namespace KeLi.MusicFolderBuilder
{
    internal class Program
    {
        private static void Main()
        {
            var appDirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var directoryInfo = new DirectoryInfo(appDirPath);
            var fileInfos = directoryInfo.GetFiles(Settings.Default.FilterType);

            foreach (var fileInfo in fileInfos)
            {
                var singerName = fileInfo.Name.Split('-').First().Trim();
                var singerDirPath = Path.Combine(appDirPath, singerName);

                if (!Directory.Exists(singerDirPath))
                    Directory.CreateDirectory(singerDirPath);

                File.Move(fileInfo.FullName, Path.Combine(singerDirPath, fileInfo.Name));
            }
        }
    }
}
