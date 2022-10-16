namespace DirectoryScanner.Core.Model;

public class FilesystemTree
{
    public Node Root { get; }

    public FilesystemTree(Node node)
    {
        Root = node;
        CalculateDirectoriesSize(Root);
    }

    private static void CalculateDirectoriesSize(Node node)
    {
        if (node.Children == null)
        {
            return;
        }

        foreach (var child in node.Children)
        {
            CalculateDirectoriesSize(child);
            node.Size += child.Size;
        }
    }
}