using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using AppServiceHelpers.Abstractions;
using AppServiceHelpers.Helpers;
using AppServiceHelpers.Models;

namespace AppServiceHelpers
{
	public class ConnectedObservableCollection<T> : ObservableRangeCollection<T> where T : EntityData
	{
		ITableDataStore<T> table;

		public ConnectedObservableCollection(ITableDataStore<T> table)
		{
			this.table = table;
		}

		public new async Task Add(T item)
		{
			base.Add(item);

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T> { item }));

			await table.AddAsync(item);
		}

		public new async Task Insert(int index, T item)
		{
			base.Insert(index, item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T> { item }));

			await table.AddAsync(item);
		}

		public async Task Update(T item)
		{
			base.Remove(Items.FirstOrDefault((i) => i.Id == item.Id));
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T> { item }));
			base.Add(item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T> { item }));

			await table.UpdateAsync(item);
		}

		public new async Task Remove(T item)
		{
			base.Remove(item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T> { item }));

			await table.DeleteAsync(item);
		}

		public new async Task RemoveAt(int index)
		{
			var item = Items[index];
			base.RemoveAt(index);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T> { item }));

			await table.DeleteAsync(item);
		}

		public async Task Refresh()
		{
			var _items = await table.GetItemsAsync();
			Items.Clear();
			foreach (var item in _items)
			{
				Items.Add(item);
			}

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}