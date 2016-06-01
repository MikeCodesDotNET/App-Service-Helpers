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
		public string Url { get { return "http://easymobileapps.azurewebsites.net/"; } }

		public void Initialize()
		{
			if (initialized)
				return;

			store = new MobileServiceSQLiteStore("app.db");

			MobileService = new MobileServiceClient(Url, null)
			{
				SerializerSettings = new MobileServiceJsonSerializerSettings()
				{
					CamelCasePropertyNames = true
				}
			};

			initialized = true;
		}

		public void RegisterTable<A, B>() where A : EntityData where B : BaseTableDataStore<A>, new()
		{
			store.DefineTable<A>();
			ServiceLocator.Instance.Add<ITableDataStore<A>, B>();
		}

		public ITableDataStore<T> Table<T>() where T : EntityData
		{
			return ServiceLocator.Instance.Resolve<ITableDataStore<T>>();
		}

		public async Task FinalizeSchema()
		{
			await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
		}
	}
}