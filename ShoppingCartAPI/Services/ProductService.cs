
using DTO.ModelDto;
using Newtonsoft.Json;
using ShoppingCartAPI.IServices;
using System;

namespace ShoppingCartAPI.Services
{
	public class ProductService:IProductService
	{
		private readonly HttpClient _httpClient;

		public ProductService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<ProductDTO?> GetProductByIdAsync(int productId)
		{
			try
			{
				var url = $"https://localhost:7001/api/Product/varianid={productId}";
				var response = await _httpClient.GetAsync(url);

				// Kiểm tra xem yêu cầu có thành công không
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					// Chuyển đổi chuỗi JSON thành đối tượng ProductDTO
					var product = JsonConvert.DeserializeObject<ProductDTO>(content);

					return product; // Trả về nội dung JSON dưới dạng chuỗi
				}
				else
				{
					// Nếu không thành công, ghi log lỗi và trả về null
					Console.WriteLine($"Error fetching product: {response.ReasonPhrase}");
					return null;
				}
			}
			catch (Exception ex)
			{
				// Ghi log lỗi
				Console.WriteLine($"Error fetching product: {ex.Message}");
				return null;
			}
		}

	}
}
