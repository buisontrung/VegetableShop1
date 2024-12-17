using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProductAPI.IRepository;
using ProductAPI.ModelDto;
using System;
using System.Security.Claims;
using System.Text;

namespace ProductAPI.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class DialogflowWebhookController : ControllerBase
	{
		private readonly IProductRepository _productRepository;
		private readonly IMemoryCache _memoryCahe;
		public DialogflowWebhookController(IProductRepository productRepository,IMemoryCache memoryCache) {
			_productRepository = productRepository;
			_memoryCahe = memoryCache;
		}
		[HttpPost]
		public async Task<IActionResult> HandleWebhook([FromBody] DialogflowRequest request)
		{

			string intentName = request.QueryResult.Intent.DisplayName;
			string userInput = request.QueryResult.QueryText;


			switch (intentName)
			{
				case "ChonDanhMuc":
					string category = request.QueryResult.Parameters["categories"]?.ToString();
					if (string.IsNullOrEmpty(category))
					{
						return Ok(new { fulfillmentText = "Vui lòng cung cấp danh mục." });
					}


					var userId2 = User?.FindFirst("Id")?.Value;
					var products = await _productRepository.GetAllProductsName(category);
					if (products == null || !products.Any())
					{
						return Ok(new { fulfillmentText = $"Danh mục {category} không có sản phẩm." });
					}
					
					string productList = string.Join(", ", products.Select(p => p.ProductName));
					return Ok(new { fulfillmentText = $"Danh sách sản phẩm trong danh mục {category}: {productList}" });

				case "SanPhamNoiBat":

					var productRating = await _productRepository.GetProductRatingMax(1,1);
					var productRating1 = productRating.First();
					return Ok(new
					{
						fulfillmentMessages = new[]
							{
								new
								{
									card = new
									{
										title = productRating1.ProductName,
										subtitle = "Mô tả: Sản phẩm chất lượng cao.",
										imageUri = productRating1.productImageDTOs.First().ImageUrl,
										buttons = new[]
										{
											new
											{
												text = "Mua ngay",
												postback =$"http://localhost:5173/#/san-pham/{productRating1.Id}"
											}
										}
									}
								}
						}
					});
				case "ChonSanPham":

					string product = request.QueryResult.Parameters["products"]?.ToString();
					if (string.IsNullOrEmpty(product))
					{
						return Ok(new { fulfillmentText = "Vui lòng cung cấp tên sản phẩm." });
					}

					var variant = await _productRepository.GetAllProductVariantsName(product);
					if (variant == null || !variant.Any())
					{
						return Ok(new { fulfillmentText = $"Không có biến thể nào cho sản phẩm {product}." });
					}
					

					
					string variantList = string.Join(", ", variant.Select(v => $"{v.VariantName} - {v.UnitPrice} VND"));
					;return Ok(new
					{
						fulfillmentMessages = new[]
							{
								new
								{
									card = new
									{
										title = "Sản phẩm 1",
										subtitle = "Mô tả: Sản phẩm chất lượng cao.",
										imageUri = "https://hoangphucphoto.com/wp-content/uploads/2024/04/anh-tcay-thumb.jpeg",
										buttons = new[]
										{
											new
											{
												text = "Mua ngay",
												postback = "https://example.com/san-pham/1"
											}
										}
									}
								}
						}
					});
				case "order - yes":
					return Ok(new
					{
						fulfillmentText = $"Đây là các biến thể của sản phẩm1"
					});
				case "order - no":
					
					// Lấy thông tin người dùng từ claims
					var userId = User?.FindFirst("Id")?.Value;
					var FirstName = User?.FindFirst("FirstName")?.Value;
					var LastName = User?.FindFirst("LastName")?.Value;
					var email = User?.FindFirst(ClaimTypes.Email)?.Value;



					// Lấy thông tin sản phẩm và phân loại từ parameters
					string product1 = request.QueryResult.OutputContexts[0].Parameters["products"]?.ToString();
					string cate = request.QueryResult.OutputContexts[0].Parameters["any"]?.ToString();

				




					var variant1 = await _productRepository.GetAllProductVariantName(product1, cate);

					
					// Tạo đối tượng OrderRequestDTO
					var orderRequest = new OrderRequestDTO()
					{
						Email = email,
						UserId = userId,
						PromotionId = null,
						Address = "số nhà 1",  // Bạn có thể thay đổi để lấy địa chỉ người dùng từ thông tin khác nếu cần
						CustomerName = FirstName + " " + LastName,
						PaymentMethodId = 2,
						IsPayment = true,
						Promotion = null,
						Phone = "0814656137",
						Status = 2,  // Trạng thái đơn hàng
					};

					// Thêm sản phẩm vào đơn hàng
					orderRequest.Items = new List<CartItemDto>
						{
							new CartItemDto() { ProductId = variant1.Id, Price =variant1.UnitPrice, Quantity = 1 }
						};

					// Tính toán tổng tiền cho đơn hàng
					orderRequest.CalculateTotals();

					// Chuyển đối tượng orderRequest thành JSON
					var orderRequestJson = JsonConvert.SerializeObject(orderRequest);

					// Tạo HttpClient và gửi POST request
					using (var client = new HttpClient())
					{
						var content = new StringContent(orderRequestJson, Encoding.UTF8, "application/json");

						try
						{
							// Gửi POST request tới URL
							var response = await client.PostAsync("https://localhost:7159/api/Order/vnpay", content);
							var responseContent = await response.Content.ReadAsStringAsync();
							var jsonResponse = JObject.Parse(responseContent);
							var url = jsonResponse["url"]?.ToString();
							if (response.IsSuccessStatusCode)
							{
								return Ok(new { fulfillmentText = responseContent });
							}
							else
							{
								return Ok(new { fulfillmentText = $"Có lỗi xảy ra khi gửi đơn hàng: {url}" });
							}
						}
						catch (Exception ex)
						{
							return Ok(new { fulfillmentText = $"Lỗi kết nối: {ex.Message}" });
						}
					}

				default:
					return Ok(new { fulfillmentText = "Xin lỗi, tôi không hiểu yêu cầu của bạn." });
			}
		}




	}
}
