using System.Diagnostics;
using DirectoryScanner.Core.Model;

namespace Tests;

public class Tests
{
    private readonly DirectoryScanner.Core.Services.DirectoryScanner _scanner = new();
    
    [Test]
    public void InvalidArgumentsTest()
    {
        Assert.Throws<ArgumentException>(() => _scanner.StartScan("invalid", 150),
            "Didn't throw exception about invalid directory name");
        Assert.Throws<ArgumentException>(() => _scanner.StartScan(@"./", 0),
            "Didn't throw exception about maxThreadCount being 0");
    }
    
    [Test]
    public void CorrectDirectoryTest()
    {
        var result = _scanner.StartScan("TestDir", 150);
        var root = result.Root;
        Assert.That(root.Children, Is.Not.Null);
        Assert.That(root.Children!, Has.Count.EqualTo(2));

        int aIndex = 0, fileIndex = 1;
        if (root.Children![0].Name != "A")
        {
            aIndex = 1;
            fileIndex = 0;
        }

        Assert.Multiple(() =>
        {
            Assert.That(root.Children![fileIndex].Name, Is.EqualTo("File.txt"));
            Assert.That(root.Children![fileIndex].Size, Is.EqualTo(3));
            Assert.That(root.Children![fileIndex].Children, Is.Null);
        });
            
        Assert.That(root.Children![aIndex].Children, Is.Not.Null);
        Assert.That(root.Children![aIndex].Children, Has.Count.EqualTo(2));
    }

    [Test]
    public void CancellationTest()
    {
        const string dirName = @"C:\";
        const int maxThreadCount = 150;
        
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var resultNoCancellation = _scanner.StartScan(dirName, maxThreadCount);
        stopwatch.Stop();
        var executionTimeNoCancellation = stopwatch.ElapsedMilliseconds;
        
        stopwatch.Reset();
        FilesystemTree resultWithCancellation = null!;
        var task = Task.Run(() =>
        {
            stopwatch.Start();
            resultWithCancellation = _scanner.StartScan(dirName, maxThreadCount);
            stopwatch.Stop();
        });
        Thread.Sleep(100); // wait for scan to start
        _scanner.StopScan();
        task.Wait();
        var executionTimeWithCancellation = stopwatch.ElapsedMilliseconds;
        
        Assert.Multiple(() =>
        {
            Assert.That(executionTimeWithCancellation, Is.LessThan(executionTimeNoCancellation));
            Assert.That(resultNoCancellation.Root.Size, Is.GreaterThanOrEqualTo(resultWithCancellation.Root.Size));
        });
    }
}