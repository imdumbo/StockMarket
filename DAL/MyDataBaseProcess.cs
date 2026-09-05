using EventService.DAL;
using EventService.Model;
using System.Text.Json;

public class MyDataBaseProcess
{
    private readonly string _baseDirectory;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SemaphoreSlim _fileLock = new(1, 1);

    public MyDataBaseProcess(string baseDirectory = null)
    {
        _baseDirectory = baseDirectory ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot");
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        Directory.CreateDirectory(_baseDirectory);
    }

    public async Task InitializeAsync()
    {
        DataBase.Customers = await LoadCollectionAsync<Customer>("Customers");
        DataBase.Inventories = await LoadCollectionAsync<Inventory>("Inventories");
        DataBase.Stocks = await LoadCollectionAsync<Stock>("Stocks");

        Console.WriteLine($"Loaded {DataBase.Customers.Count} customers, {DataBase.Inventories.Count} inventory records, and {DataBase.Stocks.Count} stock records from JSON.");
    }

    public async Task SaveCustomerChangesAsync() => await SaveCollectionAsync("Customers", DataBase.Customers);
    public async Task SaveInventoryChangesAsync() => await SaveCollectionAsync("Inventories", DataBase.Inventories);
    public async Task SaveStockChangesAsync() => await SaveCollectionAsync("Stocks", DataBase.Stocks);

    public async Task SaveAllChangesAsync()
    {
        await SaveCustomerChangesAsync();
        await SaveInventoryChangesAsync();
        await SaveStockChangesAsync();
    }

    private async Task<List<T>> LoadCollectionAsync<T>(string fileName)
    {
        string filePath = GetFilePath(fileName);

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"No existing JSON found for {fileName}. Starting fresh.");
            await SaveCollectionAsync(fileName, new List<T>());
            return new List<T>();
        }

        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
    }

    private async Task SaveCollectionAsync<T>(string fileName, List<T> items)
    {
        await _fileLock.WaitAsync();
        try
        {
            string filePath = GetFilePath(fileName);
            string json = JsonSerializer.Serialize(items, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);
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