using System.Collections.ObjectModel;

namespace Presentation.Models;

public class FilesystemTree
{
    public ObservableCollection<Node> Root { get; }

    public FilesystemTree(DirectoryScanner.Core.Model.Node root)
    {
        Root = new ObservableCollection<Node> { CreateDtoNode(root, root.Size) };
    }

    private static Node CreateDtoNode(DirectoryScanner.Core.Model.Node node, long parentSize)
    {
        var newNode = new Node(node.Name, node.Size);
        newNode.SizeInPercent = (double)node.Size / parentSize * 100;
        if (node.Children == null) 
            return newNode;
        
        newNode.Children = new ObservableCollection<Node>();
        foreach (var child in node.Children)
        {
            newNode.Children.Add(CreateDtoNode(child, node.Size));
        }
        return newNode;
    }
}