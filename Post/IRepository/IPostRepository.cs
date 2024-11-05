using Post.Model;

namespace Post.IRepository
{
	public interface IPostRepository
	{
		Task<postDTO> GetByIdAsync(string id);
		Task<IEnumerable<postDTO>> GetAllAsync();
		Task CreateAsync(Posts post);
		Task UpdateAsync(postDTO post);
		Task DeleteAsync(string id);
		Task<Comment> CreateCommentAsync(string postId, Comment newComment);
	}
}
