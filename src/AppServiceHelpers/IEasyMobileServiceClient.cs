using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;

using AppServiceHelpers.Data.Tables;
using AppServiceHelpers.Data.Models;

namespace AppServiceHelpers
{
	public interface IEasyMobileServiceClient
	{
		IMobileServiceClient MobileService { get; set; }

		void Initialize(string url);
		void RegisterTable<A>() where A : EntityData;
		ITableDataStore<T> Table<T>() where T : EntityData;
		Task FinalizeSchema();
        Task<bool> LoginAsync(MobileServiceAuthenticationProvider provider);
	}
}

