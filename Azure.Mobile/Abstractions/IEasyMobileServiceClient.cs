using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

using AppServiceHelpers.Abstractions;
using AppServiceHelpers.Models;
using AppServiceHelpers.Tables;
using AppServiceHelpers.Utils;

namespace AppServiceHelpers.Abstractions
{
	public interface IEasyMobileServiceClient
	{
		MobileServiceClient MobileService { get; set; }

		void Initialize(string url);
		void RegisterTable<A>() where A : EntityData;
		ITableDataStore<T> Table<T>() where T : EntityData;
		Task FinalizeSchema();
	}
}

