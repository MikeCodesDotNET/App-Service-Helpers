using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

public static class Storage
{
	public static async Task<string> Upload(string sas, Stream file)
	{
		string imageUrl;
		var container = new CloudBlobContainer(new Uri(sas));
		var date = DateTime.Now.ToString();

		try
		{
			var blob = container.GetBlockBlobReference("blob_" + date);
			await blob.UploadFromStreamAsync(file);

			imageUrl = blob.Uri.ToString();
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Azure.Mobile.Storage breakage: {ex.Message}");

			throw ex;
		}

		return imageUrl;
	}


	// Extension methods for UIImage, Android Bitmap, UWP image, and Xamarin.Forms impage
}
