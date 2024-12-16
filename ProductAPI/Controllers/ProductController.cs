using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ProductAPI.IRepository;
using ProductAPI.ModelDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductRepository _productRepository;
		private readonly IFileService _fileService;
		public ProductController(IProductRepository productRepository, IFileService fileService)
		{
			_productRepository = productRepository;
			_fileService = fileService;
		}
		[HttpGet("getproductnameall")]
		public async Task<ActionResult<IEnumerable<ResponseProductDialog>>> GetAllProducts(string categoryName)
		{
			var products = await _productRepository.GetAllProductsName(categoryName);
			return Ok(products);
		}
		[HttpGet("getvariant")]
		public async Task<ActionResult<IEnumerable<ProductDTO>>> GetVariant(string productname,string varriant)
		{
			var products = await _productRepository.GetAllProductVariantName(productname,varriant);
			return Ok(products);
		}

		[HttpGet("getall")]	
		public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
		{
			var products = await _productRepository.GetAllProductsAsync();
			return Ok(products);
		}
		[HttpGet("getcountproduct/${categoryId}")]
		public async Task<ActionResult<IEnumerable<ProductDTO>>> CountProduct(int categoryId)
		{
			var products = await _productRepository.CountProductByCategoryId(categoryId);
			return Ok(products);
		}

		[HttpGet("productscategoryid={categoryId}&getbyorder={orderType}&pz={pageSize}&pn={pageIndex}")]
		public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsbyOrder(string orderType, int pageSize, int pageIndex, int categoryId)
		{
			var products = await _productRepository.GetProductsByOrder(orderType,pageSize,pageIndex,categoryId);
			return Ok(products);
		}

		[HttpGet("id={id}")]
		public async Task<ActionResult<ProductDTO>> GetProductById(int id)
		{
			var product = await _productRepository.GetProductByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}
		[HttpGet("varianid={id}")]
		public async Task<ActionResult<ProductDTO>> GetProductByVarianId(int id)
		{
			var product = await _productRepository.GetProductByVarianIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}
		[HttpGet("getvarians")]
		public async Task<ActionResult<IEnumerable<ProductDTO?>>> GetAllVarianIds([FromQuery] List<int> Ids)
		{
			var products = await _productRepository.GetProductsByVariantIdsAsync(Ids);
			return Ok(products);
		}
		[HttpGet("productcategoryid={id}")]
		public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductByProductCategoryId(int id)
		{
			var product = await _productRepository.GetProductByCategoryIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}
		[HttpPost("add")]
		public async Task<ActionResult> AddProduct([FromForm] ProductDTO productDto, [FromForm] ICollection<IFormFile> images)
		{
			try
			{
				if (productDto.ImageFile?.Length > 1 * 1024 * 1024)
				{
					return StatusCode(StatusCodes.Status400BadRequest, "File size should not exceed 1 MB");
				}
				string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
				
				string createdImageName = await _fileService.SaveFileAsync(productDto.ImageFile, allowedFileExtentions);
				productDto.ImageUrl = createdImageName;

				var createdPost = await _productRepository.AddProductAsync(productDto,images);
				return Ok(createdPost);

			}
			catch (Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

			}
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductDTO productDto)
		{
			if (id != productDto.Id)
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await _productRepository.UpdateProductAsync(productDto);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteProduct(int id)
		{
			await _productRepository.DeleteProductAsync(id);
			return NoContent();
		}
	}
}
