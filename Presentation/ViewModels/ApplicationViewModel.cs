using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using Presentation.Commands;
using Presentation.Models;

namespace Presentation.ViewModels;

public class ApplicationViewModel : INotifyPropertyChanged
{
    private const ushort MaxThreadCount = 150;  
    
    private readonly DirectoryScanner.Core.Services.DirectoryScanner _scanner = new();
    private string _filename;
    private FilesystemTree _root;

    private volatile bool _isScanning;

    public bool IsScanning
    {
        get => _isScanning;
        set
        {
            _isScanning = value;
            Notify(nameof(IsScanning));
        }
    }

    public ICommand SetDirectoryCommand { get; }
    public ICommand StartScanning { get; }
    public ICommand CancelScanning { get; }


    public ApplicationViewModel()
    {
        SetDirectoryCommand = new Command(_ =>
        {
            using var openFileDialog = new FolderBrowserDialog();
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            _filename = openFileDialog.SelectedPath;
        }, _ => true);

        StartScanning = new Command(_ =>
        {
            Task.Run(() =>
            {
                IsScanning = true;
                var result = _scanner.StartScan(_filename, MaxThreadCount);
                IsScanning = false;
                Root = new FilesystemTree(result.Root);
            });
        }, _ => _filename != null && !IsScanning);

        CancelScanning = new Command(_ =>
        {
            _scanner.StopScan();
            IsScanning = false;
        }, _ => IsScanning);
    }

    public FilesystemTree Root
    {
        get => _root;
        private set
        {
            _root = value;
            Notify(nameof(Root));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void Notify(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}