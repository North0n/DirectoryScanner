using System.Collections.Concurrent;
using DirectoryScanner.Core.Interfaces;
using DirectoryScanner.Core.Model;

namespace DirectoryScanner.Core.Services;

public class DirectoryScanner : IDirectoryScanner
{
    private SemaphoreSlim _semaphore;
    private CancellationTokenSource _cancellationTokenSource;
    private ConcurrentQueue<Node> _queue;
    public bool IsScanning { get; private set; }

    public FilesystemTree StartScan(string path, ushort maxThreadCount)
    {
        if (File.Exists(path))
        {
            var fileInfo = new FileInfo(path);
            return new FilesystemTree(new Node(fileInfo.FullName, fileInfo.Name, fileInfo.Length));
        }
        if (!Directory.Exists(path))
        {
            throw new ArgumentException($"Directory {path} doesn't exist");
        }
        if (maxThreadCount == 0)
        {
            throw new ArgumentException("Max thread count can't be 0");
        }

        _semaphore = new SemaphoreSlim(maxThreadCount);
        _cancellationTokenSource = new CancellationTokenSource();
        _queue = new ConcurrentQueue<Node>();
        var token = _cancellationTokenSource.Token;
        IsScanning = true;

        var dirInfo = new DirectoryInfo(path);
        var root = new Node(dirInfo.FullName, dirInfo.Name);
        ScanDirectory(root, token);
        do
        {
            var result = _queue.TryDequeue(out var node);
            if (result)
            {
                try
                {
                    _semaphore.Wait(token);
                    Task.Run(() =>
                    {
                        ScanDirectory(node!, token);
                        _semaphore.Release();
                    }, token);
                }
                catch (Exception)
                {
                    // Exception is thrown in case if token was cancelled
                }
            }
        } while ((!_queue.IsEmpty || _semaphore.CurrentCount != maxThreadCount) &&
                 !token.IsCancellationRequested);

        IsScanning = false;
        return new FilesystemTree(root);
    }

    public void StopScan()
    {
        if (!IsScanning)
        {
            throw new Exception("Scanning is not started");
        }

        IsScanning = false;
        _cancellationTokenSource.Cancel();
    }

    private void ScanDirectory(Node node, CancellationToken token)
    {
        node.Children = new List<Node>();
        var dirInfo = new DirectoryInfo(node.Path);
        
        DirectoryInfo[] dirs;
        try
        {
            dirs = dirInfo.GetDirectories();
        }
        catch (Exception)
        {
            // Exception is thrown in case if user doesn't have access to directory
            return;
        }

        foreach (var info in dirs)
        {
            if (token.IsCancellationRequested)
                return;
            var childNode = new Node(info.FullName, info.Name);
            node.Children.Add(childNode);
            _queue.Enqueue(childNode);
        }

        var files = dirInfo.GetFiles();
        foreach (var info in files)
        {
            if (token.IsCancellationRequested)
                return;
            node.Children.Add(new Node(info.FullName, info.Name, info.Length));
        }
    }
}