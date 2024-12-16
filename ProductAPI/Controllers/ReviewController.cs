using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.IRepository;
using ProductAPI.ModelDto;

namespace ProductAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewController : ControllerBase
	{
		private readonly IRepositoryReview _repositoryReview;

		public ReviewController(IRepositoryReview repositoryReview)
		{
			_repositoryReview = repositoryReview;
		}

		// GET: api/Review
		[HttpGet("getall")]
		public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetAll()
		{
			var reviews = await _repositoryReview.GetAllAsync();
			return Ok(reviews);
		}
		[HttpGet("countRatingProduct")]
		public async Task<ActionResult<IEnumerable<object>>> GetCountRating(int Id)
		{
			var reviews = await _repositoryReview.GetCountRatingStarAsync(Id);
			return Ok(reviews);
		}


		// GET: api/Review/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<ReviewDTO>> GetById(int id)
		{
			var review = await _repositoryReview.GetByIdAsync(id);
			if (review == null)
			{
				return NotFound();
			}
			return Ok(review);
		}

		// POST: api/Review
		[Authorize]
		[HttpPost("create")]
		public async Task<ActionResult> Add(ReviewDTO reviewDto)
		{
			await _repositoryReview.AddAsync(reviewDto, User);
			return CreatedAtAction(nameof(GetById), new { id = reviewDto.Id }, reviewDto);
		}

		// PUT: api/Review/{id}
		[HttpPut("{id}")]
		public async Task<ActionResult> Update(int id, ReviewDTO reviewDto)
		{
			if (id != reviewDto.Id)
			{
				return BadRequest("ID mismatch");
			}

			var existingReview = await _repositoryReview.GetByIdAsync(id);
			if (existingReview == null)
			{
				return NotFound();
			}

			await _repositoryReview.UpdateAsync(reviewDto);
			return NoContent();
		}

		// DELETE: api/Review/{id}
		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var review = await _repositoryReview.GetByIdAsync(id);
			if (review == null)
			{
				return NotFound();
			}

			await _repositoryReview.DeleteAsync(id);
			return NoContent();
		}

		// GET: api/Review/Product/{productId}
		[HttpGet("Product/{productId}")]
		public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetByProductId(int productId)
		{
			var reviews = await _repositoryReview.GetByProductIdAsync(productId);
			return Ok(reviews);
		}
	}
}
