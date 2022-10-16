using System.Collections.ObjectModel;
using Presentation.Interfaces;

namespace Presentation.Models;

public class FilesystemTree
{
    public ObservableCollection<IFilesystemObject> Root { get; }

    public FilesystemTree(DirectoryScanner.Core.Model.Node root)
    {
        Root = new ObservableCollection<IFilesystemObject> { CreateDtoNode(root, root.Size) };
    }

    private static IFilesystemObject CreateDtoNode(DirectoryScanner.Core.Model.Node node, long parentSize)
    {
        if (node.Children == null)
        {
            var newNode = new File(node.Name, node.Size);
            newNode.SizeInPercent = (double)node.Size / parentSize * 100;
            return newNode;
        }
        else
        {
            var newNode = new Directory(node.Name, node.Size);
            newNode.SizeInPercent = (double)node.Size / parentSize * 100;
            foreach (var child in node.Children)
            {
                newNode.Children.Add(CreateDtoNode(child, node.Size));
            }
            return newNode;
        }
    }
}