namespace DirectoryScanner.Core.Model;

public class Node
{
    public Node(string path, string name)
    {
        Path = path;
        Name = name;
    }

    public Node(string path, string name, long size)
    {
        Path = path;
        Name = name;
        Size = size;
    }

    public string Path { get; }
    public string Name { get; }
    public long Size { get; set; }
    public List<Node>? Children { get; set; } = null;
}