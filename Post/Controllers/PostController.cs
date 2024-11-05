using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using global::Post.IRepository;
using global::Post.Model;
using Post.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Post.hub;
namespace Post.Controllers
{
	

	namespace Post.Controllers
	{
		[ApiController]
		[Route("api/[controller]")]
		public class PostController : ControllerBase
		{
			private readonly IPostRepository _postRepository;
			private readonly IHubContext<CommentHub> _commentHubContext;
			public PostController(IPostRepository postRepository, IHubContext<CommentHub> commentHubContext)
			{
				_postRepository = postRepository;
				_commentHubContext = commentHubContext;
			}

			// Lấy danh sách tất cả bài viết
			[HttpGet("getall")]
			public async Task<ActionResult<IEnumerable<postDTO>>> GetAllPosts()
			{
				var posts = await _postRepository.GetAllAsync();
				return Ok(posts);
			}

			// Lấy bài viết theo ID
			[HttpGet("{id}")]
			public async Task<ActionResult<postDTO>> GetPostById(string id)
			{
				var post = await _postRepository.GetByIdAsync(id);

				if (post == null)
				{
					return NotFound();
				}

				return Ok(post);
			}

			// Tạo bài viết mới
			[HttpPost]
			public async Task<ActionResult<Posts>> CreatePost([FromBody] Posts newPost)
			{
				if (newPost == null)
				{
					return BadRequest();
				}

				await _postRepository.CreateAsync(newPost);
				return CreatedAtAction(nameof(GetPostById), new { id = newPost.Id.ToString() }, newPost);
			}
			[HttpPost("createComment")]
			public async Task<ActionResult<Comment>> CreateCommentPost([FromBody] Comment newComment, string id)
			{
				if (newComment == null)
				{
					return BadRequest("Comment data is null.");
				}

				try
				{

					await _postRepository.CreateCommentAsync(id, newComment);

					return CreatedAtAction(nameof(GetPostById), new { id = newComment.Id }, newComment);
				}
				catch (Exception ex)
				{
					// Xử lý lỗi (nếu có)
					return StatusCode(500, $"Internal server error: {ex.Message}");
				}
			}


			// Cập nhật bài viết


			// Xóa bài viết
			[HttpDelete("{id}")]
			public async Task<IActionResult> DeletePost(string id)
			{
				var post = await _postRepository.GetByIdAsync(id);

				if (post == null)
				{
					return NotFound();
				}

				await _postRepository.DeleteAsync(id);

				return NoContent();
			}
		}
	}

}
