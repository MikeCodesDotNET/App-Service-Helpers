# DataStores & Tables

ASH works around the concept of having tables of objects. We register our tables when we initialize ASH and thereafter have access to CRUD operations. ASH Tables should inherit from BaseTableDataStore which itself provide functionality of Azures IMobileServiceSyncTable Interface.

Before we can register a table, we'll need to have created a Model which inherits from EntityData.

::: danger WARNING
Please make sure you're using the EntityData class provided by ASH and not from a EntityFramework dependancy.
:::

## Implements ITableDataStore

```csharp 
public interface ITableDataStore<T>
{
  void Initialize();

  Task<IEnumerable<T>> GetItemsAsync();

  Task<T> GetItemAsync(string id);

  Task<bool> AddAsync(T item);

  Task<bool> UpdateAsync(T item);

  Task<bool> DeleteAsync(T item);

  Task Sync();

  int Count();
}
```

## Registering a table 
The most basic way of using ASH is to register a table and use the table for all crud operations. When registering a table, ASH will deal with creating a DataStore for you. You can register as many tables as you wish but each registration should have a unique model. (It's not possible to register the same model multiple times).

```csharp 
client.RegisterTable<ToDo>();
```

## Fetching a registered table 
Once you've registered a table, you can get access to it by querying the ASH client. This may become static in the future but as of today, you'll need to keep an instance of the Client around to pass to your ViewModels.

```csharp 
var myTable = client.GetTable<ToDo>(); 
```

## Adding a new item 
```csharp 
await table.InsertAsync(new ToDo());
```