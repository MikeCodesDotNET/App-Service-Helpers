using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

using Azure.Mobile.Abstractions;
using Azure.Mobile.Models;
using Azure.Mobile.Tables;
using Azure.Mobile.Utils;

namespace Azure.Mobile
{
	public class EasyMobileServiceClient : IEasyMobileServiceClient
	{
		bool initialized;
		MobileServiceSQLiteStore store;

		public MobileServiceClient MobileService { get; set; }
        public string Url { get; private set; }

		Dictionary<string, BaseTableDataStore> tables;

		public static IEasyMobileServiceClient Create()
		{
			ServiceLocator.Instance.Add<IEasyMobileServiceClient, EasyMobileServiceClient>();

			return ServiceLocator.Instance.Resolve<IEasyMobileServiceClient>();
		}

		public void Initialize(string url)
		{
			if (initialized)
				return;

			Url = url;

			store = new MobileServiceSQLiteStore("app.db");

			MobileService = new MobileServiceClient(Url, null)
			{
				SerializerSettings = new MobileServiceJsonSerializerSettings()
				{
					CamelCasePropertyNames = true
				}
			};

			tables = new Dictionary<string, BaseTableDataStore>();

			initialized = true;
		}

		public void RegisterTable<A>() where A : EntityData
		{
			store.DefineTable<A>();

			var table = new BaseTableDataStore<A>();
			table.Initialize(this);
			tables.Add(typeof(A).Name, table);
		}

		public void RegisterTable<A, B>() where A : EntityData where B : BaseTableDataStore<A>, new()
		{
			store.DefineTable<A>();
			ServiceLocator.Instance.Add<ITableDataStore<A>, B>();
		}

		public async Task FinalizeSchema()
		{
			await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
		}

		#region Tables
		public ITableDataStore<T> Table<T>() where T : EntityData
		{
			var instance = ServiceLocator.Instance.Resolve<ITableDataStore<T>>();
			if (instance != null)
			{
				return instance;
			}
			else
			{
				return tables[typeof(T).Name] as BaseTableDataStore<T>;
			}
		}

		public async Task Sync<T>(IMobileServiceSyncTable<T> table)
		{
			var connected = await Plugin.Connectivity.CrossConnectivity.Current.IsReachable("google.com");
			if (connected == false)
				return;

			try
			{
				var identifier = typeof(T).Name;
				await MobileService.SyncContext.PushAsync();
				await table.PullAsync($"all{identifier}", table.CreateQuery());
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Unable to sync items, that is alright as we have offline capabilities: {ex.Message}");
			}
		}

		public async Task<bool> Add<T>(IMobileServiceSyncTable<T> table, T item)
		{
			await table.InsertAsync(item);
			await Sync(table);

			return true;
		}

		public async Task<bool> Update<T>(IMobileServiceSyncTable<T> table, T item)
		{
			await table.UpdateAsync(item);
			await Sync(table);

			return true;
		}

		public async Task<bool> Delete<T>(IMobileServiceSyncTable<T> table, T item)
		{
			await table.DeleteAsync(item);
			await Sync(table);

			return true;
		}

		public async Task<T> GetItem<T>(IMobileServiceSyncTable<T> table, string id) where T : EntityData
		{
			await Sync(table);

			var items = await table.Where(s => s.Id == id).ToListAsync();

			if (items == null || items.Count == 0)
				return null;
			
			return items[0];
		}

		public async Task<IEnumerable<T>> GetItems<T>(IMobileServiceSyncTable<T> table)
		{
			await Sync(table);

			return await table.ToEnumerableAsync();
		}
		#endregion
	}
}