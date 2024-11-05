
using Microsoft.Extensions.FileProviders;
using MongoDB.Driver;
using Post.hub;
using Post.IRepository;
using Post.Mapper;
using Post.Repository;


namespace Post
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddSignalR();
			var connectionString = builder.Configuration["ConnectionStrings:Connection"];
			var databaseName = builder.Configuration["ConnectionStrings:DatabaseName"];
			var collectionName = builder.Configuration["ConnectionStrings:CollectionName"];

			// Đăng ký MongoDB client
			builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(connectionString));
			builder.Services.AddControllers();		
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddScoped<IPostRepository, PostRepository>();
			builder.Services.AddScoped<IFileService, FileService>();
			//Add Cors
			builder.Services.AddAutoMapper(typeof(MappingProfile));
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin", policy =>
				{
					policy.WithOrigins("http://localhost:5173") // Thay thế bằng URL của bạn
						  .AllowAnyMethod()
						  .AllowAnyHeader()
						  .AllowCredentials(); // Cho phép gửi thông tin xác thực
				});
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

			app.UseAuthorization();

			app.UseCors("AllowSpecificOrigin");
			app.MapControllers();
			app.MapHub<CommentHub>("commentHub");
			app.Run();
		}
	}
}
