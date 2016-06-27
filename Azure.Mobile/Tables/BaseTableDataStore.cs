using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

using Azure.Mobile.Abstractions;
using Azure.Mobile.Utils;

namespace Azure.Mobile.Tables
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

        public virtual void Initialize()
        {
            if (serviceClient == null)
				serviceClient = ServiceLocator.Instance.Resolve<IEasyMobileServiceClient>();
        }

		public virtual void Initialize(IEasyMobileServiceClient client)
		{
			serviceClient = client;
		}

        public async virtual Task Sync()
        {
            var connected = await Plugin.Connectivity.CrossConnectivity.Current.IsReachable("google.com");
            if (connected == false)
                return;

            try
            {
				await serviceClient.MobileService.SyncContext.PushAsync();
                await Table.PullAsync($"all{identifier}", Table.CreateQuery());

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to sync items, that is alright as we have offline capabilities: {ex.Message}");
            }
        }

        public async virtual Task<bool> Add(T item)
        {
            Initialize();

            await table.InsertAsync(item);
            await Sync();

            return true;
        }

        public async virtual Task<bool> Update(T item)
        {
            Initialize();

            await table.UpdateAsync(item);
            await Sync();

            return true;
        }

        public async virtual Task<bool> Delete(T item)
        {
            Initialize();

            await table.DeleteAsync(item);
            await Sync();

            return true;
        }

        public async virtual Task<T> GetItem(string id)
        {
            await Sync();

            var items = await table.Where(s => s.Id == id).ToListAsync();

            if (items == null || items.Count == 0)
                return null;

            return items[0];
        }

        public async virtual Task<IEnumerable<T>> GetItems()
        {
            Initialize();
            await Sync();

            return await table.ToEnumerableAsync();
        }

        public virtual int Count()
        {
            var query = Table.CreateQuery();
            query.IncludeTotalCount();

            var results = query.ToListAsync().Result;
            return results.Count;
        }
    }
}

