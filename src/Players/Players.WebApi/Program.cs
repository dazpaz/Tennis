using Cqrs.Common.Application;
using Cqrs.Common.Data;
using Players.Application.Repository;
using Players.Data;
using Players.Data.Repository;
using Players.WebApi.Utilities;

namespace Players.WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			var commandConnectionString = new ConnectionString(builder.Configuration["CommandConnectionString"]);
			builder.Services.AddSingleton(commandConnectionString);

			builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
			builder.Services.AddTransient(s => new PlayersDbContext(commandConnectionString, true));

			builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
			builder.Services.AddTransient(s => new PlayersDbContext(commandConnectionString, true));

			builder.Services.AddSingleton<IMessageDispatcher, MessageDispatcher>();
			builder.Services.AddHandlers();

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();

				using var context = new PlayersDbContext(commandConnectionString, false);
				context.Database.EnsureCreated();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}