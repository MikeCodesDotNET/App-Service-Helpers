using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

using AppServiceHelpers.Abstractions;
using AppServiceHelpers.Models;
using AppServiceHelpers.Tables;
using AppServiceHelpers.Utils;

namespace AppServiceHelpers
{
	public class EasyMobileServiceClient : IEasyMobileServiceClient
	{
		bool initialized;
		MobileServiceSQLiteStore store;

		public IMobileServiceClient MobileService { get; set; }
        public string Url { get; private set; }

		Dictionary<string, BaseTableDataStore> tables;

        static Lazy<IEasyMobileServiceClient> Implementation = new Lazy<IEasyMobileServiceClient>(() => new EasyMobileServiceClient(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IEasyMobileServiceClient Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }


        /// <summary>
        /// Initializes the EasyMobileServiceClient to work with an Azure Mobile App endpoint.
        /// </summary>
        /// <param name="url">The url of your Azure Mobile App.</param>
        public void Initialize(string url)
		{
			if (initialized)
				return;

			Url = url;

			store = new MobileServiceSQLiteStore("app.db");

			var authenticationHandler = new AuthenticationHandler();
			MobileService = new MobileServiceClient(Url, authenticationHandler)
			{
				SerializerSettings = new MobileServiceJsonSerializerSettings()
				{
					CamelCasePropertyNames = true
				}
			};
			authenticationHandler.Client = MobileService;

			tables = new Dictionary<string, BaseTableDataStore>();
			LoadCachedUserCredentials();

			initialized = true;
		}

		/// <summary>
		/// Initializes the EasyMobileServiceClient with a preconfigured IMobileServiceClient for custom scenarios.
		/// </summary>
		/// <param name="client">Preconfigured IMobileServiceClient.</param>
		public void Initialize(IMobileServiceClient client)
		{
			if (initialized)
				return;

			MobileService = client;
			tables = new Dictionary<string, BaseTableDataStore>();
			LoadCachedUserCredentials();

			initialized = true;
		}

		/// <summary>
		/// Register a data model with EasyMobileServiceClient to create a table.
		/// </summary>
		/// <typeparam name="A">The data model used to create table schema.</typeparam>
		public void RegisterTable<A>() where A : EntityData
		{
			store.DefineTable<A>();

			var table = new BaseTableDataStore<A>();
			table.Initialize(this);
			tables.Add(typeof(A).Name, table);
		}

		/// <summary>
		/// Register a custom data store with EasyMobileServiceClient to create a table.
		/// </summary>
		/// <returns>The table.</returns>
		/// <typeparam name="A">The data model used to create table schema.</typeparam>
		/// <typeparam name="B">A custom implementation of BaseTableDataStore. For default behavior, use RegisterTable<A>.</typeparam>
		public void RegisterTable<A, B>() where A : EntityData where B : BaseTableDataStore<A>, new()
		{
			store.DefineTable<A>();

			var table = new B();
			table.Initialize(this);
			tables.Add(typeof(A).Name, table);
		}

		/// <summary>
		/// Creates a backing SQLite database for the tables registered. Tables cannot be registered after FinalizeSchema is called.
		/// </summary>
		public async Task FinalizeSchema()
		{
			await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
		}

		/// <summary>
		/// Fetch a data store for a data model, allowing operations to be done on the table.
		/// </summary>
		/// <returns>An ITableDataStore for CRUD operations and syncing.</returns>
		/// <typeparam name="T">The data model for the table.</typeparam>
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

		#region Authentication
		public async Task<bool> LoginAsync(MobileServiceAuthenticationProvider provider)
		{
			var authenticator = Platform.Instance.Authenticator;

			return await authenticator.LoginAsync(MobileService, provider);
		}
		#endregion

		void LoadCachedUserCredentials()
		{
			var authenticator = Platform.Instance.Authenticator;
			var credentials = authenticator.LoadCachedUserCredentials();

			if (credentials != null 
			    && credentials.ContainsKey("userId") 
			    && credentials.ContainsKey("authenticationToken"))
			{
				MobileService.CurrentUser.UserId = credentials["userId"];
				MobileService.CurrentUser.MobileServiceAuthenticationToken = credentials["authenticationToken"];
			}
		}
	}
}