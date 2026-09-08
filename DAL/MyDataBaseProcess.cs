using EventService.DAL;
using EventService.Model;
using System.Text.Json;

public class MyDataBaseProcess
{
    private readonly string _baseDirectory;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SemaphoreSlim _fileLock = new(1, 1);

    public MyDataBaseProcess(string? baseDirectory = null)
    {
        _baseDirectory = baseDirectory ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot");
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        Directory.CreateDirectory(_baseDirectory);
    }

    public void Initialize()
    {
        DataBase.Customers = LoadCollection<Customer>("Customers");
        DataBase.Stocks = LoadCollection<Stock>("Stocks");
        DataBase.Inventories = LoadCollection<Inventory>("Inventories");

        Console.WriteLine($"Loaded {DataBase.Customers.Count} customers, {DataBase.Inventories.Count} inventory records, and {DataBase.Stocks.Count} stock records from JSON.");
    }

    public void SaveCustomerChanges() => SaveCollection("Customers", DataBase.Customers);
    public void SaveInventoryChanges() => SaveCollection("Inventories", DataBase.Inventories);
    public void SaveStockChanges() => SaveCollection("Stocks", DataBase.Stocks);

    public void SaveAllChanges()
    {
        SaveCustomerChanges();
        SaveInventoryChanges();
        SaveStockChanges();
    }

    private List<T> LoadCollection<T>(string fileName)
    {
        string filePath = GetFilePath(fileName);

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"No existing {fileName} found.");
            SaveCollection(fileName, new List<T>());
            return new List<T>();
        }

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
    }

    private void SaveCollection<T>(string fileName, List<T> items)
    {
        _fileLock.Wait();
        try
        {
            string filePath = GetFilePath(fileName);
            string json = JsonSerializer.Serialize(items, _jsonOptions);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            throw new IOException($"Could not save {fileName} data to {_baseDirectory}.", ex);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(_baseDirectory, $"{fileName}.json");
    }
}