using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using KeLi.MusicRenamer.Properties;

namespace KeLi.MusicRenamer
{
    internal class Program
    {
        private static void Main()
        {
            var appDirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var directoryInfo = new DirectoryInfo(appDirPath);
            var fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);

            foreach (var fileInfo in fileInfos)
            {
                if (!Settings.Default.FilterTypes.Contains(fileInfo.Extension))
                    continue;

                var match = Regex.Match(Path.GetFileNameWithoutExtension(fileInfo.Name), Settings.Default.FilterRegex);
                var fileName = fileInfo.Name;

                if (match.Success)
                {
                    if (fileInfo.DirectoryName != appDirPath)
                        File.Move(fileInfo.FullName, Path.Combine(appDirPath, fileName));

                    continue;
                }

                if (fileName.Contains('_'))
                {
                    if (fileInfo.DirectoryName != appDirPath)
                        File.Move(fileInfo.FullName, Path.Combine(appDirPath, fileName));

                    continue;
                }

                if (fileName.Contains('('))
                {
                    if (fileInfo.DirectoryName != appDirPath)
                        File.Move(fileInfo.FullName, Path.Combine(appDirPath, fileName));

                    continue;
                }

                if (fileName.ToCharArray().Count(c => c == '.') != 1)
                {
                    if (fileInfo.DirectoryName != appDirPath)
                        File.Move(fileInfo.FullName, Path.Combine(appDirPath, fileName));

                    continue;
                }

                if (fileName.ToCharArray().Count(c => c == '-') != 1)
                {
                    if (fileInfo.DirectoryName != appDirPath)
                        File.Move(fileInfo.FullName, Path.Combine(appDirPath, fileName));

                    continue;
                }

                if (!fileName.Contains(" - "))
                    fileName = fileName.Replace("-", " - ");

                var singerName = fileName.Split('-').First().Trim();
                var singerDirPath = fileInfo.DirectoryName;

                if (fileInfo.Directory.Name != singerName)
                    singerDirPath = Path.Combine(fileInfo.DirectoryName, singerName);

                if (!Directory.Exists(singerDirPath))
                    Directory.CreateDirectory(singerDirPath);

                var targetFilePath = Path.Combine(singerDirPath, fileName);

                if (fileInfo.FullName == targetFilePath)
                    continue;

                if (File.Exists(targetFilePath))
                    File.Delete(targetFilePath);

                File.Move(fileInfo.FullName, targetFilePath);
            }
        }
    }
}