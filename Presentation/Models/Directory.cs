using System.Collections.ObjectModel;
using Presentation.Interfaces;

namespace Presentation.Models;

public class Directory : IFilesystemObject
{
    public Directory(string name, long size, double sizeInPercent)
    {
        Name = name;
        Size = size;
        SizeInPercent = sizeInPercent;
    }

    public string Name { get; }
    public long Size { get; }

    public double SizeInPercent { get; }
    public ObservableCollection<IFilesystemObject> Children { get; } = new();
}