using System.Collections.Generic;
using System.Threading.Tasks;


namespace Azure.Mobile.Abstractions
{
    public interface ITableDataStore<T>
    {
        void Initialize();

        Task<IEnumerable<T>> GetItems();

        Task<T> GetItem(string id);

        Task<bool> Add(T item);

        Task<bool> Update(T item);

        Task<bool> Delete(T item);

        Task Sync();
    }
}

