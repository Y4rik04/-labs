using System;
using System.IO;

public class FileMonitorEventArgs : EventArgs
{
    public string FilePath { get; } //шлях до файлу
    public DateTime Timestamp { get; } //час створення і тд
    public long FileSize { get; } //розмір файлу

    public FileMonitorEventArgs(string filePath, DateTime timestamp, long fileSize) //конструктор
    {
        FilePath = filePath;
        Timestamp = timestamp;
        FileSize = fileSize;
    }
}

public delegate void FileMonitorDelegate(object sender, FileMonitorEventArgs e); //створення делегату

public class FileMonitor
{
    //події з різними файловими операціями
    public event FileMonitorDelegate OnFileCreated;
    public event FileMonitorDelegate OnFileDeleted;
    public event FileMonitorDelegate OnFileModified;
    public event FileMonitorDelegate OnFileRenamed;

    private readonly FileSystemWatcher fileSystemWatcher;  //відслідковування подій файлової системи

    // додавання фільтрів
    private string[] allowedFileTypes = { ".txt", ".docx" }; // приклад фільтрів
    private string[] ignoredFileTypes = { ".tmp" };

    public FileMonitor(string path) //конструктор
    {
        fileSystemWatcher = new FileSystemWatcher(path);
        //події
        fileSystemWatcher.Created += (sender, e) => NotifyEvent(OnFileCreated, e.FullPath, "Created");
        fileSystemWatcher.Deleted += (sender, e) => NotifyEvent(OnFileDeleted, e.FullPath, "Deleted");
        fileSystemWatcher.Changed += (sender, e) => NotifyEvent(OnFileModified, e.FullPath, "Modified");
        fileSystemWatcher.Renamed += (sender, e) => NotifyEvent(OnFileRenamed, e.FullPath, "Renamed");
    }

    private void NotifyEvent(FileMonitorDelegate handler, string filePath, string action) //метод сповіщення обраних делегатів та виведення повідомлень у консоль
    {
        if (IsFileTypeAllowed(filePath) && !IsFileTypeIgnored(filePath))
        {
            var eventArgs = new FileMonitorEventArgs(filePath, DateTime.Now, GetFileSize(filePath));
            handler?.Invoke(this, eventArgs);
            Console.WriteLine($"{action} file: {Path.GetFileName(filePath)}, Size: {eventArgs.FileSize} bytes, Timestamp: {eventArgs.Timestamp}");
        }
    }
    //перевірка, чи дозволений тип файлу
    private bool IsFileTypeAllowed(string filePath)
    {
        return allowedFileTypes.Any(extension => filePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase));
    }
    //перевірка, чи ігнорується тип файлу
    private bool IsFileTypeIgnored(string filePath)
    {
        return ignoredFileTypes.Any(extension => filePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase));
    }
    //отримання розміру файлу
    private long GetFileSize(string filePath)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }
        catch (Exception)
        {
            return -1; // Обробка винятку або повернення значення за замовчуванням
        }
    }

    public void StartMonitoring() //початок моніторингу
    {
        fileSystemWatcher.EnableRaisingEvents = true;
    }

    public void StopMonitoring() //стоп моніторингу
    {
        fileSystemWatcher.EnableRaisingEvents = false;
    }
}

class Program
{
    static void Main()
    {
        string pathToMonitor = @"D:\домашка\C#";  //папка в якій проводиться моніторинг
        FileMonitor fileMonitor = new FileMonitor(pathToMonitor); //створення екземпляру класу FileMonitor
        // подій з файлами використовуючи лямбда вирази
        fileMonitor.OnFileCreated += (sender, e) => Console.WriteLine($"File created: {Path.GetFileName(e.FilePath)}");
        fileMonitor.OnFileDeleted += (sender, e) => Console.WriteLine($"File deleted: {Path.GetFileName(e.FilePath)}");
        fileMonitor.OnFileModified += (sender, e) => Console.WriteLine($"File modified: {Path.GetFileName(e.FilePath)}");
        fileMonitor.OnFileRenamed += (sender, e) => Console.WriteLine($"File renamed: {Path.GetFileName(e.FilePath)}");
        //початок моніторингу
        fileMonitor.StartMonitoring();

        Console.WriteLine("Press any key to stop monitoring.");
        Console.ReadKey();
        //стоп моніторингу
        fileMonitor.StopMonitoring();
    }
}

