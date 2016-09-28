using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace AppServiceHelpers.Data.Tables
{
	public delegate ConflictResolutionStrategy ResolveConflictDelegate<T>(T localVersion, T serverVersion);

    public abstract class BaseTableDataStore { }

	public class BaseTableDataStore<T> : BaseTableDataStore, ITableDataStore<T> where T : Models.EntityData
    {
		IEasyMobileServiceClient serviceClient;
        readonly string identifier = typeof(T).Name;

        IMobileServiceSyncTable<T> table;
        protected IMobileServiceSyncTable<T> Table => table ?? (table = serviceClient.MobileService.GetSyncTable<T>());

        public ConflictResolutionStrategy ConflictResolutionStrategy { get; set; }
		public ResolveConflictDelegate<T> ConflictResolutionStrategyDelegate { get; set; }

		public virtual void Initialize(IEasyMobileServiceClient client)
		{
			serviceClient = client;
		}

        public virtual async Task Sync()
        {
            var connected = Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
            if (!connected)
                return;

			await serviceClient.MobileService.SyncContext.PushAsync();
			await Table.PullAsync($"all{identifier}", Table.CreateQuery());
        }

        public virtual async Task<bool> AddAsync(T item)
        {
			try
			{
				await Table.InsertAsync(item);
				await Sync();
			}
			catch (MobileServicePreconditionFailedException<T> ex)
			{
				var localVersion = item;
				var serverVersion = ex.Item;

				await Resolve(localVersion, serverVersion);
			}

            return true;
        }

        public virtual async Task<bool> UpdateAsync(T item)
        {
			try
			{
				await Table.UpdateAsync(item);
				await Sync();
			}
			catch (MobileServicePreconditionFailedException<T> ex)
			{
				var localVersion = item;
				var serverVersion = ex.Item;

				await Resolve(localVersion, serverVersion);
			}

            return true;
        }

        public virtual async Task<bool> DeleteAsync(T item)
        {
			try
			{
				await Table.DeleteAsync(item);
				await Sync();
			}
			catch (MobileServicePreconditionFailedException<T> ex)
			{
				var localVersion = item;
				var serverVersion = ex.Item;

				await Resolve(localVersion, serverVersion);
			}

            return true;
        }

        public virtual async Task<T> GetItemAsync(string id)
        {
            await Sync();

            var items = await Table.Where(s => s.Id == id).ToListAsync();

            if (items == null || items.Count == 0)
                return null;

            return items[0];
        }

        public virtual async Task<IEnumerable<T>> GetItemsAsync()
        {
            await Sync();

            return await Table.ToEnumerableAsync();
        }

        public virtual int Count()
        {
            var query = Table.CreateQuery();
            query.IncludeTotalCount();

            var results = query.ToListAsync().Result;
            return results.Count;
        }

		async Task<bool> Resolve(T localVersion, T serverVersion)
		{
			// Is anyone on the invocation list for the delegate? If so, user opted for custom
			// conflict handling instead of automatic.
			var strategy = ConflictResolutionStrategy;
			if (ConflictResolutionStrategyDelegate != null &&
				ConflictResolutionStrategyDelegate.GetInvocationList().Length > 0)
			{
				strategy = ConflictResolutionStrategyDelegate.Invoke(localVersion, serverVersion);
			}

			// Handle the actual conflict.
			var result = false;
			switch (strategy)
			{
				// Set local version to server version.
				case ConflictResolutionStrategy.ClientWins:
					{
						localVersion.Version = serverVersion.Version;
						await Sync();

						result = true;

						break;
					}
				// Copy entire record from server to local copy.
				case ConflictResolutionStrategy.ServerWins:
					{
						localVersion = serverVersion;
						await Sync();

						result = true;

						break;
					}
				// Which version was the latest write done?
				case ConflictResolutionStrategy.LatestWins:
					{
						localVersion.Version = serverVersion.Version;
						await Sync();

						result = true;

						break;
					}
			}

			return result;
		}
    }
}