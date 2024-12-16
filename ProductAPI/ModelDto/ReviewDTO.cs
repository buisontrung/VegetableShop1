namespace ProductAPI.ModelDto
{
	public class ReviewDTO
	{
		public int Id { get; set; }
		public int? ProductId { get; set; }
		public string? UserId { get; set; }
		public string? UserName { get; set; }
		public string? Content { get; set; }
		public int Rating { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime UpdateAt { get; set; }
	}
}
