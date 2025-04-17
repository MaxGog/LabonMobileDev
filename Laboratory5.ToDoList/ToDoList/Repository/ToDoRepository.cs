using System.Collections.ObjectModel;
using SQLite;

using ToDoList.Models;
using ToDoList.Constants;

namespace ToDoList.Repository;

public class ToDoRepository
{
    private SQLiteAsyncConnection _database;

    public ToDoRepository()
    {
        Initialize();
    }

    private async Task Initialize()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.Constants.DatabasePath, Constants.Constants.Flags);
        await _database.CreateTableAsync<ToDoModel>();
    }

    public async Task<ObservableCollection<ToDoModel>> GetAllItemsAsync()
    {
        await Initialize();
        var items = await _database.Table<ToDoModel>().ToListAsync();
        return new ObservableCollection<ToDoModel>(items);
    }

    public async Task<int> SaveItemAsync(ToDoModel item)
    {
        await Initialize();
        if (item.Id != 0)
        return await _database.UpdateAsync(item);
        else
        return await _database.InsertAsync(item);
    }

    public async Task<int> DeleteItemAsync(ToDoModel item)
    {
        await Initialize();
        return await _database.DeleteAsync(item);
    }
}