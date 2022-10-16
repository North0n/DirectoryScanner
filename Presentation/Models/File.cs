using Presentation.Interfaces;

namespace Presentation.Models;

public class File : IFilesystemObject
{
    public File(string name, long size, double sizeInPercent)
    {
        Name = name;
        Size = size;
        SizeInPercent = sizeInPercent;
    }

    public string Name { get; }
    public long Size { get; }

    public double SizeInPercent { get; }
}