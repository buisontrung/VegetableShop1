namespace Post.Model
{
	public class postDTO
	{
		public string Id { get; set; } 
		public string Title { get; set; }
		public List<Content> Contents { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public Author? Author { get; set; }
		public List<Image> Images { get; set; }
		public List<Comment> Comments { get; set; }
	}

	

	
}

