using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

using AppServiceHelpers.Abstractions;
using AppServiceHelpers.Utils;

namespace AppServiceHelpers.Tables
{
	public abstract class BaseTableDataStore
	{

	}

	public class BaseTableDataStore<T> : BaseTableDataStore, ITableDataStore<T> where T : Models.EntityData
    {
		IEasyMobileServiceClient serviceClient;
        string identifier = typeof(T).Name;

        IMobileServiceSyncTable<T> table;
        protected IMobileServiceSyncTable<T> Table
        {
			get { return table ?? (table = serviceClient.MobileService.GetSyncTable<T>()); }
        }

		public ConflictResolutionStrategy ConflictResolutionStrategy { get; set; }
		public delegate ConflictResolutionStrategy ResolveConflict<T>(T localVersion, T serverVersion);

		public virtual void Initialize(IEasyMobileServiceClient client)
		{
			serviceClient = client;
		}

        public async virtual Task Sync()
        {
			await serviceClient.MobileService.SyncContext.PushAsync();
			await Table.PullAsync($"all{identifier}", Table.CreateQuery());
        }

        public async virtual Task<bool> AddAsync(T item)
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

				// Is anyone on the invocation list for the delegate?
					// Yes, this means that the user opted for custom data stores.
						// Call the subscribed delegate, grab the return value.
						// Pass return value to Resolve method to do heavy lifting.
					// No, this means the user is opting for default conflict handling.
						// Call Resolve method with ConflictResolutionHandler property as parameter.
			}

            return true;
        }

        public async virtual Task<bool> UpdateAsync(T item)
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

				// Is anyone on the invocation list for the delegate?
					// Yes, this means that the user opted for custom data stores.
						// Call the subscribed delegate, grab the return value.
						// Pass return value to Resolve method to do heavy lifting.
					// No, this means the user is opting for default conflict handling.
						// Call Resolve method with ConflictResolutionHandler property as parameter.
			}

            return true;
        }

        public async virtual Task<bool> DeleteAsync(T item)
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

				// Is anyone on the invocation list for the delegate?
				// Yes, this means that the user opted for custom data stores.
				// Call the subscribed delegate, grab the return value.
				// Pass return value to Resolve method to do heavy lifting.
				// No, this means the user is opting for default conflict handling.
				// Call Resolve method with ConflictResolutionHandler property as parameter.
			}

            return true;
        }

        public async virtual Task<T> GetItemAsync(string id)
        {
            await Sync();

            var items = await Table.Where(s => s.Id == id).ToListAsync();

            if (items == null || items.Count == 0)
                return null;

            return items[0];
        }

        public async virtual Task<IEnumerable<T>> GetItemsAsync()
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

		// Internal method for handling actual logic of conflict resolution strategy.
		bool Resolve<T>(T localVersion, T serverVersion)
		{
			// To resolve the conflict, update the version of the item being committed. Otherwise, you will keep
			// catching a MobileServicePreConditionFailedException.
			// localItem.Version = serverItem.Version;

			// Client wins
				// Update the version in our record to match version in server, then repush.
				// localVersion.Version = serverVersion.Version;
			// Server wins
				// Copy the entire record from server into list.
			// Latest wins
				// Which version was last write done?
	
			return true;
		}
    }
}