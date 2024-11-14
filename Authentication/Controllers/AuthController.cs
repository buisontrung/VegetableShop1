using Authentication.Data;
using Authentication.Model;
using Authentication.ModelDto;
using Authentication.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly ApplicationDbContext _dbContext;
		private readonly IEmailService _emailService;
		private readonly IDistributedCache _cache;
		public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,ApplicationDbContext dbContext,IEmailService emailService, IDistributedCache cache)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_dbContext = dbContext;
			_emailService = emailService;
			_cache = cache;
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserById(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound("User not found");
			}

			var userDto = new UserDTO
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,

				Email = user.Email
			};

			return Ok(userDto);
		}
		[HttpGet("getuser")]
		
		public async Task<IActionResult> GetUserByUserName()
		{
			var username = User.Identity?.Name;
			if(username.IsNullOrEmpty())
			{
				return NotFound();
			}

			var user = await _userManager.FindByNameAsync(username);
			if (user == null)
			{
				return NotFound("User not found");
			}

			var userDto = new UserDTO
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				PhoneNumber = user.PhoneNumber,
				Email = user.Email
			};

			return Ok(userDto);
		}
		[HttpGet]
		[Route("users-by-role/{role}")]
		public async Task<IActionResult> GetUsersByRole(string role)
		{
			var users = await _userManager.Users.ToListAsync();
			var usersInRole = new List<UserDTO>();

			foreach (var user in users)
			{
				if (await _userManager.IsInRoleAsync(user, role))
				{
					usersInRole.Add(new UserDTO
					{
						Id = user.Id,
						FirstName = user.FirstName,
						LastName = user.LastName,
						Email = user.Email,

					});
				}
			}

			return Ok(usersInRole);
		}

		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDTO user)
		{
			var exituser = await _userManager.FindByNameAsync(user.UserName);
			if (exituser != null)
			{
				return BadRequest("Exit");
			}
			ApplicationUser newUser = new ApplicationUser()
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserName = user.UserName,
				Email = user.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
			};
			var createUserResult = await _userManager.CreateAsync(newUser, user.Password);
			
			if (!createUserResult.Succeeded)
			{
				var ErrorString = "User Create Error Because :";
				foreach (var error in createUserResult.Errors)
				{
					ErrorString += "#" + error;
				}
				return BadRequest(ErrorString);
			}
			var token1 = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
			await _userManager.ConfirmEmailAsync(newUser, token1);
			await _userManager.AddToRoleAsync(newUser, "user");
			string token = await GenerateNewJsonWebToken(newUser);
			string refreshtoken = GenerateRefreshToken(9);
			var refreshtokenModel = new RefreshToken()
			{
				Token = refreshtoken,
				ClientId = null,
				UserId = newUser.Id,
				Expiration = DateTime.Now.AddDays(30),
				IsRevoked = false,
				User=null
			};
			await _dbContext.RefreshTokens.AddAsync(refreshtokenModel);
			await _dbContext.SaveChangesAsync();
			var tokenmodel = new TokenModel()
			{
				Token = token,
				refreshToken = refreshtoken,
			};
			return Ok(tokenmodel);
		}
		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO user)
		{
			var exitUser = await _userManager.FindByNameAsync(user.UserName);
			if (exitUser is null)
			{
				return Unauthorized("NotFound UserName");
			}
			var checkPassword = await _userManager.CheckPasswordAsync(exitUser, user.Password);
			if (!checkPassword)
			{
				return Unauthorized("Password is incorrect");
			}
			var existingToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == exitUser.Id && !t.IsRevoked);
			if (existingToken != null)
			{
				existingToken.IsRevoked = true;
				_dbContext.RefreshTokens.Update(existingToken);
				await _dbContext.SaveChangesAsync();
			}
			string newRefreshToken = GenerateRefreshToken(9);

			var refreshTokenModel = new RefreshToken()
			{
				Token = newRefreshToken,
				UserId = exitUser.Id,
				Expiration = DateTime.Now.AddDays(30),
				IsRevoked = false,
			};

			await _dbContext.RefreshTokens.AddAsync(refreshTokenModel);
			await _dbContext.SaveChangesAsync();
			var token = await GenerateNewJsonWebToken(exitUser);
			var tokenresult = new TokenModel()
			{
				Token = token,
				refreshToken = newRefreshToken
			};
			return Ok(tokenresult);
		}

		[HttpPost("signin-google")]
		public async Task<IActionResult> SignInWithGoogle([FromBody] GoogleTokenDTO tokenDTO)
		{
			try
			{
				if (tokenDTO == null || string.IsNullOrEmpty(tokenDTO.credential))
				{
					return BadRequest(new { message = "Invalid token or data" });
				}


				// Validate the Google token
				var payload = await VerifyGoogleTokenAsync(tokenDTO.credential);

				// Check if the user already exists
				var existingUser = await _userManager.FindByEmailAsync(payload.Email);
				if (existingUser == null)
				{
					// Create a new user if not exists
					existingUser = new ApplicationUser
					{
						UserName = payload.Email,
						Email = payload.Email,
						FirstName = payload.GivenName,
						LastName = payload.FamilyName,
						SecurityStamp = Guid.NewGuid().ToString()
					};

					var createUserResult = await _userManager.CreateAsync(existingUser);
					if (!createUserResult.Succeeded)
					{
						return BadRequest("User creation failed");
					}

					await _userManager.AddToRoleAsync(existingUser, "user");
				}
				else
				{
					// Update existing user if needed
					existingUser.FirstName = payload.GivenName;
					existingUser.LastName = payload.FamilyName;
					await _userManager.UpdateAsync(existingUser);
				}

				// Generate JWT token for the user
				
				var token = GenerateNewJsonWebToken(existingUser);
				return Ok(token);
			}
			catch (Exception ex)
			{
				return BadRequest($"Google Sign-In failed: {ex.Message}");
			}
		}
		[HttpPost("signin-facebook")]
		public async Task<IActionResult> SignInWithFacebook([FromBody] string tokenres)
		{
			try
			{
				
				if (tokenres == null || string.IsNullOrEmpty(tokenres))
				{
					return BadRequest(new { message = "Invalid token or data" });
				}


				// Validate the Google token
				var payload = await VerifyGoogleTokenAsync(tokenres);

				// Check if the user already exists
				var existingUser = await _userManager.FindByEmailAsync(payload.Email);
				if (existingUser == null)
				{
					// Create a new user if not exists
					existingUser = new ApplicationUser
					{
						UserName = payload.Email,
						Email = payload.Email,
						FirstName = payload.GivenName,
						LastName = payload.FamilyName,
						SecurityStamp = Guid.NewGuid().ToString()
					};

					var createUserResult = await _userManager.CreateAsync(existingUser);
					if (!createUserResult.Succeeded)
					{
						return BadRequest("User creation failed");
					}

					await _userManager.AddToRoleAsync(existingUser, "user");
				}
				else
				{
					// Update existing user if needed
					existingUser.FirstName = payload.GivenName;
					existingUser.LastName = payload.FamilyName;
					await _userManager.UpdateAsync(existingUser);
				}

				// Generate JWT token for the user
				
				var token = GenerateNewJsonWebToken(existingUser);
				return Ok(token);
			}
			catch (Exception ex)
			{
				return BadRequest($"Google Sign-In failed: {ex.Message}");
			}
		}
		[HttpPost("VerifiedEmail")]
		public async Task<IActionResult> VerifiedEmail([FromBody] VerifyEmailDto request)
		{
			try
			{
				if(request.Email == null)
				{
					return BadRequest();
				}
				string otp = Generaterandomnumber();
				string otpbody =GenerateEmailBody(request.Username, otp);
				var Otp = new Otp()
				{
					Email = request.Email,
					Subject = "Cảm ơn vì đã sử dụng dịch vụ của chúng tôi",
					Emailbody = otpbody
				};
				 await _emailService.SendEmail(Otp);
				string otpKey = $"otp:{request.Email}";
				var cacheEntryOptions = new DistributedCacheEntryOptions()
				.SetAbsoluteExpiration(TimeSpan.FromMinutes(6));
				await _cache.SetStringAsync(otpKey, otp, cacheEntryOptions);
				return Ok("Gửi thành công");

			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpPost("VerifyOtp")]
		public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto request)
		{
			try
			{
				// Key để lấy OTP sẽ là "otp:email@example.com"
				string otpKey = $"otp:{request.Email}";

				// Lấy OTP từ Redis
				var storedOtp = await _cache.GetStringAsync(otpKey);

				// Kiểm tra OTP có tồn tại không
				if (string.IsNullOrEmpty(storedOtp))
				{
					return BadRequest(new { message = "OTP has expired or is invalid." });
				}

				// Kiểm tra mã OTP
				if (storedOtp != request.Otp)
				{
					return BadRequest(new { message = "Invalid OTP." });
				}
				await _cache.RemoveAsync(otpKey);
				// Nếu OTP hợp lệ, thực hiện logic đăng ký/tạo tài khoản
				return Ok(new { message = "OTP is valid. You can now proceed with registration." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while verifying the OTP.", details = ex.Message });
			}
		}
		private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string idToken)
		{
			var settings = new GoogleJsonWebSignature.ValidationSettings
			{
				Audience = new[] { "351636607233-d0cbhf3btn0mursiue86j09io0b0lka4.apps.googleusercontent.com" }
			};

			var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
			return payload;
		}
		[HttpPost("GenerateToken")]
		public async Task<IActionResult> GenerateAccessToken(string refreshToken)
		{
			try
			{
				var result = await  (from token in _dbContext.RefreshTokens 
							   join user in _dbContext.Users on token.UserId equals user.Id
							   where token.Token == refreshToken && token.IsRevoked == false
							   select new {user}).FirstOrDefaultAsync();
				if(result == null)
				{
					return BadRequest();
				}
				
					
				string accesstoken = await GenerateNewJsonWebToken(result.user);
				 return Ok(accesstoken);
			}catch(Exception ex)
			{
				return BadRequest();
			}
		}
		[HttpPut("RevokedToken")]
		public async Task<IActionResult> RevokedToken(string refreshToken)
		{
			try
			{
				var result = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
				if (result == null)
				{
					return BadRequest();
				}
				result.IsRevoked = true;
				_dbContext.RefreshTokens.Update(result);
				return Ok("revokedSuccess");
			}
			catch (Exception ex)
			{
				return BadRequest();
			}
		}
		private async Task<string> GenerateNewJsonWebToken(ApplicationUser user)
		{
			// Lấy danh sách các vai trò của người dùng
			var userRoles = await _userManager.GetRolesAsync(user);

			// Tạo danh sách các claim (thông tin xác thực) cho JWT
			var authClaims = new List<Claim>
			{
				new Claim("Id", user.Id),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
				new Claim("FirstName", user.FirstName),
				new Claim("LastName", user.LastName),
				new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddMinutes(30).ToString())
			};

			// Thêm các vai trò của người dùng vào claim
			foreach (var role in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, role));
			}

			// Lấy khóa bí mật từ cấu hình
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

			// Tạo JWT token
			var tokenObj = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Issuer"],
				expires: DateTime.UtcNow.AddHours(1), // Thời gian hết hạn token
				claims: authClaims, // Claims được gán vào token
				signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
			);

			// Viết token ra chuỗi
			string token = new JwtSecurityTokenHandler().WriteToken(tokenObj);

			// Trả về token
			return token;
		}
		public static string GenerateRefreshToken(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var random = new Random();
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
		[HttpPost]
		[Route("refresh-token")]
		public async Task<IActionResult> RefreshToken([FromBody]string Token)
		{
			// Tìm refresh token trong cơ sở dữ liệu
			var refreshToken = await _dbContext.RefreshTokens
				.Include(rt => rt.User) // Kết hợp với User nếu cần
				.FirstOrDefaultAsync(rt => rt.Token == Token && !rt.IsRevoked);

			// Kiểm tra tính hợp lệ của refresh token
			if (refreshToken == null || refreshToken.Expiration < DateTime.Now|| refreshToken.IsRevoked)
			{
				return BadRequest("Refresh token is invalid or has expired.");
			}

			// Tạo access token mới
			var user = await _userManager.FindByIdAsync(refreshToken.UserId);
			if (user == null)
			{
				return BadRequest("User not found.");
			}

			string newAccessToken = await GenerateNewJsonWebToken(user);
			string newRefreshToken = Token; 

			// Cập nhật refresh token mới vào cơ sở dữ liệu
			refreshToken.Token = newRefreshToken;
			refreshToken.Expiration = DateTime.Now.AddDays(30); // Thay đổi ngày hết hạn nếu cần
			await _dbContext.SaveChangesAsync();

			// Trả về token mới
			var tokenModel = new TokenModel
			{
				Token = newAccessToken,
				refreshToken = newRefreshToken
			};

			return Ok(tokenModel);
		}
		[HttpPost]
		[Route("add")]
		public async Task<IActionResult> AddRole()
		{
			string role = "user";
			if (!await _roleManager.RoleExistsAsync(role))
			{
				var result = await _roleManager.CreateAsync(new IdentityRole(role));
				if (result.Succeeded)
				{
					return Ok(new { Success = true, Message = "Role created successfully." });
				}
				else
				{
					return BadRequest(new { Success = false, Message = "Failed to create role.", Errors = result.Errors });
				}
			}
			return BadRequest(new { Success = false, Message = "Role already exists." });
		}
		private string Generaterandomnumber()
		{
			Random random = new Random();
			string randomno = random.Next(0, 1000000).ToString("D6");
			return randomno;
		}
		private string GenerateEmailBody(string name, string otptext)
		{
			string emailbody = "<div style='width:100%;background-color:grey'>";
			emailbody += "<h1>Hi " + name + ", Thanks for registering</h1>";
			emailbody += "<h2>Please enter OTP text and complete the registeration</h2>";
			emailbody += "<h2>OTP Text is :" + otptext + "</h2>";
			emailbody += "</div>";

			return emailbody;
		}
	}
}
