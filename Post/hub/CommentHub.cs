using Microsoft.AspNetCore.SignalR;
using Post.IRepository;
using Post.Model;

namespace Post.hub
{
	public class CommentHub : Hub
	{

		public CommentHub() { }
		public async Task SendComment(string postId, string token, string message)
		{
			using (HttpClient client = new HttpClient())
			{
				// Thêm header cho HttpClient
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

				// Gửi yêu cầu GET đến endpoint cụ thể (thay đổi đường dẫn nếu cần)
				HttpResponseMessage response = await client.GetAsync("https://localhost:7000/Auth/api/Auth/getuser"); // Cần có URL đầy đủ
				string userData = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					// Gửi thông báo đến nhóm liên quan đến postId
					await Clients.Group(postId).SendAsync("ReceiveComment", postId, userData, message);
				}
				else
				{
					// Xử lý lỗi nếu cần
					throw new Exception("Error fetching data: " + response.StatusCode);
				}
			}
		}
		public async Task JoinGroup(string postId)
		{
			// Tham gia nhóm có tên là postId
			await Groups.AddToGroupAsync(Context.ConnectionId, postId);
			// Có thể gửi một thông báo cho client để xác nhận tham gia nhóm
			await Clients.Caller.SendAsync("GroupJoined", postId);
		}
		

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			// Khi client ngắt kết nối, rời khỏi nhóm
			var postId = Context.GetHttpContext().Request.Query["postId"];
			if (!string.IsNullOrEmpty(postId))
			{
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, postId);
			}
			await base.OnDisconnectedAsync(exception);
		}
	}

}