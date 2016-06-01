using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

using Azure.Mobile.Abstractions;
using Azure.Mobile.Models;
using Azure.Mobile.Tables;
using Azure.Mobile.Utils;

namespace Azure.Mobile.Abstractions
{
	public interface IEasyMobileServiceClient
	{
		MobileServiceClient MobileService { get; set; }

		void Initialize();
		void RegisterTable<A, B>() where A : EntityData where B : BaseTableDataStore<A>, new();
		ITableDataStore<T> Table<T>() where T : EntityData;
		Task FinalizeSchema();
	}
}

