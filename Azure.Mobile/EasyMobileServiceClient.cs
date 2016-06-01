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
	public class EasyMobileServiceClient
	{
		bool initialized;
		string url;
		MobileServiceClient MobileService;
		MobileServiceSQLiteStore store;

		public EasyMobileServiceClient(string url)
		{
			this.url = url;
		}

		async Task Initialize()
		{
			if (initialized)
				return;
			
			store = new MobileServiceSQLiteStore("app.db");

			MobileService = new MobileServiceClient(url, null)
			{
				SerializerSettings = new MobileServiceJsonSerializerSettings()
				{
					CamelCasePropertyNames = true
				}
			};

			await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
		}

		public async Task RegisterTable<A, B>() where A : EntityData where B : BaseTableDataStore<A>, new()
		{
			await Initialize();

			ServiceLocator.Instance.Add<ITableDataStore<A>, B>();

			store.DefineTable<A>();
		}

		public ITableDataStore<T> Table<T>() where T : EntityData
		{
			return ServiceLocator.Instance.Resolve<ITableDataStore<T>>();
		}
	}
}