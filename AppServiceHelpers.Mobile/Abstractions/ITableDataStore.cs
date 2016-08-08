using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace AppServiceHelpers.Abstractions
{
    public interface ITableDataStore<T>
    {
		ConflictResolutionStrategy ConflictResolutionStrategy { get; set; }

		void Initialize(IEasyMobileServiceClient client);

        Task<IEnumerable<T>> GetItemsAsync();

        Task<T> GetItemAsync(string id);

        Task<bool> AddAsync(T item);

        Task<bool> UpdateAsync(T item);

        Task<bool> DeleteAsync(T item);

        Task Sync();

        int Count();
    }
}

