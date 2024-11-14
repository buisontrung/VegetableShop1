
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ProductAPI.Services
{
	public class BlobStorageService:IBlobStorageService
	{
		public async Task<CloudBlobContainer> GetCloudBlogContainer()
		{
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=nongsandemoblob;AccountKey=7oHN1fjwG3OeawWtNsCS1+l8Am87bdy5IB74gWvyM9HM52DXh57NN8EstIxKZPdesGO9dMkC/6TY+AStP+CEYg==;EndpointSuffix=core.windows.net");

			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			// Retrieve a reference to a container.
			CloudBlobContainer container = blobClient.GetContainerReference("pictures");

			await container.CreateIfNotExistsAsync();

			return container;
		}
	}

}
