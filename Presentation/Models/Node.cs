using System.Collections.ObjectModel;

namespace Presentation.Models;

public class Node
{
    public Node(string name, long size)
    {
        Name = name;
        Size = size;
    }

    public string Name { get; }
    public long Size { get; }
    public double SizeInPercent { get; internal set; }
    public ObservableCollection<Node> Children { get; set; }
}