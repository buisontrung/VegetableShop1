
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Data;
using ProductAPI.IRepository;
using ProductAPI.Repository;
using ProductAPI.Services;
using System.Text;

namespace ProductAPI
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
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{


				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
				;
			});
			builder.Services.AddScoped<IProductCategoryRepository,ProductCategoryRepository>();
			builder.Services.AddScoped<IProductInventorySupplierRepository, ProductInventorySupplierRepository>();
			builder.Services.AddScoped<ISupplierRepository,SupplierRepository>();
			builder.Services.AddScoped<IInventoryRepository,InventoryRepository>();
			builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
			builder.Services.AddScoped<IProductRepository,ProductRepository>();
			builder.Services.AddScoped<IFileService, FileService>();
			builder.Services.AddScoped<IBlobStorageService,BlobStorageService>();
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
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Images")),
				RequestPath = "/images"
			});
			app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
