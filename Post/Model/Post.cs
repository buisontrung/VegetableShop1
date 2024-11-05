using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Post.Model
{
	public class Posts
	{
		[BsonId] // Đánh dấu thuộc tính này là ID của tài liệu
		public ObjectId Id { get; set; } // MongoDB sử dụng ObjectId làm ID mặc định

		[BsonElement("title")]
		public string Title { get; set; } // Tiêu đề của bài viết

		[BsonElement("content")]
		public List<Content> Contents { get; set; } // Nội dung của bài viết

		[BsonElement("createdAt")]
		public DateTime CreatedAt { get; set; } // Ngày giờ tạo bài viết

		[BsonElement("updatedAt")]
		public DateTime UpdatedAt { get; set; } // Ngày giờ cập nhật bài viết

		[BsonElement("author")]
		public Author Author { get; set; } // Thông tin về tác giả

		[BsonElement("images")]
		public List<Image> Images { get; set; } // Mảng chứa các thông tin về ảnh

		[BsonElement("comments")]
		public List<Comment> Comments { get; set; } // Mảng chứa các comment liên quan
	}

	// Lớp đại diện cho tác giả của bài viết
	public class Author
	{
		[BsonElement("id")]
		public string Id { get; set; } // ID của tác giả

		[BsonElement("firstName")]
		public string FirstName { get; set; } // Tên của tác giả

		[BsonElement("lastName")]
		public string LastName { get; set; } // Họ của tác giả
	}

	// Lớp đại diện cho một ảnh
	public class Image
	{
		[BsonElement("imageId")]
		public int ImageId { get; set; } // ID của ảnh

		[BsonElement("url")]
		public string Url { get; set; } // Đường dẫn đến ảnh

		[BsonElement("description")]
		public string Description { get; set; } // Mô tả ảnh
	}

	// Lớp đại diện cho một comment
	public class Comment
	{
		[BsonId]
		public ObjectId Id { get; set; }

		[BsonElement("author")]
		public CommentAuthor Author { get; set; } // Thông tin về tác giả bình luận

		[BsonElement("content")]
		public string Contents { get; set; } // Nội dung comment

		[BsonElement("createdAt")]
		public DateTime CreatedAt { get; set; } // Ngày giờ tạo comment

		[BsonElement("updatedAt")]
		public DateTime UpdatedAt { get; set; } // Ngày giờ cập nhật comment
	}

	// Lớp đại diện cho tác giả của comment
	public class CommentAuthor
	{
		[BsonElement("id")]
		public string Id { get; set; } // ID của tác giả bình luận

		[BsonElement("firstName")]
		public string FirstName { get; set; } // Tên của tác giả bình luận

		[BsonElement("lastName")]
		public string LastName { get; set; } // Họ của tác giả bình luận
	}
	public class Content {
		[BsonElement("title")]
		public string Title { get; set; }
		[BsonElement("value")]
		public string? Value { get; set; }
		[BsonElement("imageUrl")]
		public string? ImageUrl { get; set; }
	}
}
