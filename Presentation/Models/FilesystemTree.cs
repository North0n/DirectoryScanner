using System;
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
        IFilesystemObject newNode;
        var sizeInPercent = (double)node.Size / parentSize * 100;
        sizeInPercent = double.IsNaN(sizeInPercent) ? 0 : sizeInPercent;
        if (node.Children == null)
        {
            newNode = new File(node.Name, node.Size, sizeInPercent);
        }
        else
        {
            newNode = new Directory(node.Name, node.Size, sizeInPercent);
            foreach (var child in node.Children)
            {
                ((Directory)newNode).Children.Add(CreateDtoNode(child, node.Size));
            }
        }

        return newNode;
    }
}