
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Order.Data;
using Order.IRepository;
using Order.IServices;
using Order.Repository;
using Order.Service;
using Order.Services;
using Order.Settings;
using System.Text;

namespace Order
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
			builder.Services.AddHttpClient();
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{


				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
				;
			});
			builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSetting"));
			builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddScoped<IOrderRepository, OrderRepository>();
			builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
			builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
			builder.Services.AddHttpClient<IProductService, ProductService>();
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
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAllOrigins", builder =>
					builder.AllowAnyOrigin()
						   .AllowAnyHeader()
						   .AllowAnyMethod());
			});
			builder.Services.AddDistributedMemoryCache();
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(15);  // Th?i gian h?t h?n c?a session
				options.Cookie.HttpOnly = true;  // ??m b?o cookie ch? có th? ???c truy c?p qua HTTP
				options.Cookie.IsEssential = true;  // Cookie s? c?n thi?t cho các yêu c?u
			});
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseSession();
			app.UseCors("AllowAllOrigins");
			app.MapControllers();

			app.Run();
		}
	}
}
