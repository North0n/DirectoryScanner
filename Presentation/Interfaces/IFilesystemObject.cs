namespace Presentation.Interfaces;

public interface IFilesystemObject
{
    public string Name { get; }
    public long Size { get; }
    public double SizeInPercent { get; }
}