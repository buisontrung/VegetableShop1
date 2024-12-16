using ProductAPI.ModelDto;
using System.Security.Claims;

namespace ProductAPI.IRepository
{
	public interface IRepositoryReview
	{
		Task<IEnumerable<ReviewDTO>> GetAllAsync();
		Task<ReviewDTO?> GetByIdAsync(int id);
		Task AddAsync(ReviewDTO reviewDto, ClaimsPrincipal user);
		Task<IEnumerable<object?>> GetCountRatingStarAsync(int id);
		Task UpdateAsync(ReviewDTO reviewDto);
		Task DeleteAsync(int id);
		Task<IEnumerable<ReviewDTO>> GetByProductIdAsync(int productId);

	}
}
