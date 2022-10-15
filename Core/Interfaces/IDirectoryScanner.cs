using DirectoryScanner.Core.Model;

namespace DirectoryScanner.Core.Interfaces;

public interface IDirectoryScanner
{
     FilesystemTree StartScan(string path, ushort maxThreadCount);
     
     void StopScan();
     
     public bool IsScanning { get; }
}