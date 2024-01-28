namespace SiteGenerator;

public static class Extensions
{
    public static FileInfo ChildFile(this DirectoryInfo dir, string childName)
    {
        return new FileInfo(Path.Combine(dir.FullName, childName));
    }

    public static DirectoryInfo ChildDirectory(this DirectoryInfo dir, string childName)
    {
        return new DirectoryInfo(Path.Combine(dir.FullName, childName));
    }

    public static void CopyTo(this FileInfo file, FileInfo targetFile)
    {
        file.CopyTo(targetFile.FullName);
    }

    public static void CopyTo(this DirectoryInfo dir, string target_dir_path)
    {
        dir.CopyTo(new DirectoryInfo(target_dir_path));
    }

    public static void CopyTo(this DirectoryInfo dir, DirectoryInfo target, bool recursive = true)
    {
        string sourceDirPath = dir.FullName;
        string targetDirPath = target.FullName;

        target.Create();

        if (recursive)
        {
            foreach (var childDir in dir.EnumerateDirectories("*", SearchOption.AllDirectories))
                Directory.CreateDirectory(childDir.FullName.Replace(sourceDirPath, targetDirPath));

            foreach (var childFile in dir.EnumerateFiles("*", SearchOption.AllDirectories))
                childFile.CopyTo(childFile.FullName.Replace(sourceDirPath, targetDirPath));
        }
        else
        {
            foreach (var childFile in dir.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
                childFile.CopyTo(childFile.FullName.Replace(sourceDirPath, targetDirPath));
        }
    }

    public static string ReadAllText(this FileInfo file)
    {
        return File.ReadAllText(file.FullName);
    }

    public static void WriteAllText(this FileInfo file, string text)
    {
        File.WriteAllText(file.FullName, text);
    }
}
