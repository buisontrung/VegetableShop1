using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.IRepository;
using ShoppingCartAPI.Model;

namespace ShoppingCartAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ShoppingCartController : ControllerBase
	{
		public IShoppingCartRepository _shoppingCartRepository;
		public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
		{
			_shoppingCartRepository = shoppingCartRepository;

		}
		[HttpGet("get")]
		public async Task<ActionResult<object?>> getall(int id)
		{
			return await _shoppingCartRepository.GetCartItemByIdAsync(id);
		}
		[HttpGet("userId={id}")]
		public async Task<ICollection<object?>> getbyuserId(string id)
		{
			return await _shoppingCartRepository.GetCartItemsbyUserIdAsync(id);
		}
		[HttpGet("user={id}&productVarianId={variantid}")]
		public async Task<ShoppingCarts?> getbyvarianuserId(string id,int variantid)
		{
			return await _shoppingCartRepository.GetCartItemByVarianIdUserIdAsync(variantid,id);
		}
		[HttpPost("add")]
		public async Task<ShoppingCarts?> Creat(ShoppingCarts a)
		{
			return await _shoppingCartRepository.AddToCartAsync(a);
		}
		[HttpDelete("Delete")]
		public async Task<IActionResult> Delete([FromBody]List<int> Ids)
		{
			 await _shoppingCartRepository.RemoveCartItemAsync(Ids);
			return Ok("delete thanh cong");
		}

	} 
}
	
