using AutoMapper;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using Post.IRepository;
using Post.Model;

namespace Post.Repository
{
	public class PostRepository : IPostRepository
	{
		private readonly IMongoCollection<Posts> _posts;
		private readonly IMapper _mapper;

		public PostRepository(IMongoClient client, IConfiguration configuration,IMapper mapper)
		{
			var databaseName = configuration["ConnectionStrings:DatabaseName"];
			var collectionName = configuration["ConnectionStrings:CollectionName"];
			var database = client.GetDatabase(databaseName);
			_mapper = mapper;
			_posts = database.GetCollection<Posts>(collectionName);
		}

		public async Task<postDTO> GetByIdAsync(string id)
		{

			var post = await _posts.Find(post => post.Id.ToString() == id).FirstOrDefaultAsync();
			if (post == null) return null;

			var postDto = _mapper.Map<postDTO>(post);
			return postDto;
		}
		public async Task<Comment>CreateCommentAsync(string postId,Comment comment)
		{
			var objectId = new ObjectId(postId);

			ObjectId newObjectId = ObjectId.GenerateNewId();
			comment.Id = newObjectId;

			var updateDefinition = Builders<Posts>.Update.Push(p => p.Comments, comment);
			await _posts.UpdateOneAsync(p => p.Id == objectId, updateDefinition);
			return comment;
		}
		public async Task<IEnumerable<postDTO>> GetAllAsync()
		{
			try
			{

				var posts = await _posts.Find(post => true).ToListAsync();

				var postsDtos = _mapper.Map<List<postDTO>>(posts);
				return postsDtos;
			}catch(Exception ex)
			{
				throw new Exception("An error occurred while retrieving posts", ex);
			}
		}
		public async Task CreateAsync(Posts post)
		{
			await _posts.InsertOneAsync(post);
		}

		public async Task UpdateAsync(postDTO post)
		{
			var postmodel = _mapper.Map<Posts>(post);
			await _posts.ReplaceOneAsync(p => p.Id == postmodel.Id, postmodel);
		}

		public async Task DeleteAsync(string id)
		{
			var objectId = new ObjectId(id);
			await _posts.DeleteOneAsync(post => post.Id == objectId);
		}
	}
}
