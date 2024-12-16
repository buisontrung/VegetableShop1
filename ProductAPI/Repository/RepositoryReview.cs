using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.IRepository;
using ProductAPI.Model;
using ProductAPI.ModelDto;
using System;
using System.Security.Claims;

namespace ProductAPI.Repository
{
	public class RepositoryReview : IRepositoryReview
	{
		private readonly ApplicationDbContext _context;

		public RepositoryReview(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ReviewDTO>> GetAllAsync()
		{
			return await _context.Reviews.Select(r => new ReviewDTO
			{
				Id = r.Id,
				ProductId = r.ProductId,
				UserId = r.UserId,
				UserName = r.UserName,
				Content = r.Content,
				Rating = r.Rating,
				CreateAt = r.CreateAt,
				UpdateAt = r.UpdateAt
			}).ToListAsync();
		}

		public async Task<ReviewDTO?> GetByIdAsync(int id)
		{
			return await _context.Reviews
				.Where(r => r.Id == id)
				.Select(r => new ReviewDTO
				{
					Id = r.Id,
					ProductId = r.ProductId,
					UserId = r.UserId,
					UserName = r.UserName,
					Content = r.Content,
					Rating = r.Rating,
					CreateAt = r.CreateAt,
					UpdateAt = r.UpdateAt
				})
				.FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<object?>> GetCountRatingStarAsync(int productId)
		{
			// Tạo một danh sách các sao từ 1 đến 5
			var allRatings = new[] { 1, 2, 3, 4, 5 };

			// Truy vấn để lấy số lượng đánh giá cho từng sao
			var ratingCounts = await _context.Reviews
				.Where(r => r.ProductId == productId)
				.GroupBy(r => r.Rating)
				.Select(g => new { Rating = g.Key, Count = g.Count() })
				.ToListAsync();

			// Danh sách kết quả
			var result = new List<object>();

			// Dùng foreach để duyệt tất cả các sao từ 1 đến 5
			foreach (var rating in allRatings)
			{
				var count = ratingCounts.FirstOrDefault(rc => rc.Rating == rating)?.Count ?? 0;
				result.Add(new { Rating = rating, Count = count });
			}

			return result;
		}


		public async Task AddAsync(ReviewDTO reviewDto, ClaimsPrincipal user)
		{
			var UserId = user.FindFirst("Id")?.Value;
			var FirstName = user.FindFirst("FirstName")?.Value;
			var LastName = user.FindFirst("LastName")?.Value;
			var review = new Review
			{
				ProductId = reviewDto.ProductId,
				UserId = UserId,
				UserName = FirstName +" "+LastName,
				Content = reviewDto.Content,
				Rating = reviewDto.Rating,
				CreateAt = reviewDto.CreateAt,
				UpdateAt = reviewDto.UpdateAt
			};

			await _context.Reviews.AddAsync(review);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(ReviewDTO reviewDto)
		{
			var review = await _context.Reviews.FindAsync(reviewDto.Id);
			if (review != null)
			{
				review.ProductId = reviewDto.ProductId;
				review.UserId = reviewDto.UserId;
				review.UserName = reviewDto.UserName;
				review.Content = reviewDto.Content;
				review.Rating = reviewDto.Rating;
				review.UpdateAt = reviewDto.UpdateAt;

				_context.Reviews.Update(review);
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(int id)
		{
			var review = await _context.Reviews.FindAsync(id);
			if (review != null)
			{
				_context.Reviews.Remove(review);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<ReviewDTO>> GetByProductIdAsync(int productId)
		{
			return await _context.Reviews
				.Where(r => r.ProductId == productId)
				.Select(r => new ReviewDTO
				{
					Id = r.Id,
					ProductId = r.ProductId,
					UserId = r.UserId,
					Content = r.Content,
					UserName = r.UserName,
					Rating = r.Rating,
					CreateAt = r.CreateAt,
					UpdateAt = r.UpdateAt
				})
				.ToListAsync();
		}
	}
}
