using System.ComponentModel.DataAnnotations;

namespace Authentication.ModelDto
{
	public class RegisterDTO
	{
		[Required(ErrorMessage = "First is required")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "Last is required")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "UserName is required")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; }
		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }


	}
}
