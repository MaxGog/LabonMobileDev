namespace Laboratory10.Interfaces;
public interface IApiService
{
    Task<List<Item>> GetItemsAsync(CancellationToken ct = default);

    Task<List<Item>> SearchItemsAsync(string query, CancellationToken ct = default);

    Task<Item> GetItemByIdAsync(int id, CancellationToken ct = default);
}