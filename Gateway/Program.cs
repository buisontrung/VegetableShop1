using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

namespace Gateway
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
			builder.Services.AddOcelot();
			
			// CORS configuration
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAllOrigins", builder => builder
					.AllowAnyOrigin()    // Allow all domains
					.AllowAnyMethod()    // Allow all HTTP methods
					.AllowAnyHeader());  // Allow all headers
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
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			app.UseHttpsRedirection();

			// Use CORS with defined policy
			app.UseCors("AllowAllOrigins");
			app.UseAuthentication();
			app.UseAuthorization();

			// Use Ocelot for routing
			app.UseOcelot().Wait();

			// Run the app
			app.Run();
		}
	}
}
