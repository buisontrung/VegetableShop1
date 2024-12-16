using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoppingCart.API.Repository;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.IRepository;
using ShoppingCartAPI.IServices;
using ShoppingCartAPI.Services;
using System.Text;
using ShoppingCartAPI.hub;

namespace ShoppingCartAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddMemoryCache();
			builder.Services.AddHttpClient<IProductService, ProductService>();

			builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
			builder.Services.AddSignalR();
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{


				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
				;
			});
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration["Jwt:Issuer"],
					ValidAudience = builder.Configuration["Jwt:Issuer"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
				};
			});
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin", policy =>
				{
					policy.WithOrigins("http://localhost:5173") // Thay th? b?ng URL c?a b?n
						  .AllowAnyMethod()
						  .AllowAnyHeader()
						  .AllowCredentials(); // Cho phép g?i thông tin xác th?c
				});
			});
			var app = builder.Build();
			app.MapHub<CartHub>("/cartHub");
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseCors("AllowSpecificOrigin");
			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
