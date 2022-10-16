using System.Collections.ObjectModel;
using Presentation.Interfaces;

namespace Presentation.Models;

public class Directory : IFilesystemObject
{
    public Directory(string name, long size)
    {
        Name = name;
        Size = size;
    }

    public string Name { get; }
    public long Size { get; }

    public double SizeInPercent { get; internal set; }
    public ObservableCollection<IFilesystemObject> Children { get; } = new();
}