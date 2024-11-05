using Microsoft.WindowsAzure.Storage.Blob;

namespace ProductAPI.Services
{
	public interface IBlobStorageService
	{
		public  Task<CloudBlobContainer> GetCloudBlogContainer();
	}
}
