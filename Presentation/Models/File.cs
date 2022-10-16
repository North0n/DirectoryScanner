using Presentation.Interfaces;

namespace Presentation.Models;

public class File : IFilesystemObject
{
    public File(string name, long size)
    {
        Name = name;
        Size = size;
    }

    public string Name { get; }
    public long Size { get; }

    public double SizeInPercent { get; internal set; }
}