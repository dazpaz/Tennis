using DomainDesign.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentManagement.Application;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Data;
using TournamentManagement.Data.Repository;
using TournamentManagement.Domain.TournamentAggregate;

namespace TournamentManagement.WebApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var connectionString = Configuration["ConnectionString"];

			services.AddTransient<IUnitOfWork, UnitOfWork>();
			services.AddTransient(s => new TournamentManagementDbContext(connectionString, true));
			services.AddTransient<ICommandHandler<AddTournamentCommand, Guid>, AddTournamentCommandHandler>();
			services.AddTransient<ICommandHandler<AmendTournamentCommand>, AmendTournamentCommandHandler>();
			services.AddTransient<IQueryHandler<GetTournamentSummaryQuery, List<TournamentSummaryDto>>,
				GetTournamentSummaryQueryHandler>();
			services.AddSingleton<MessageDispatcher>();

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "TournamentManagement.WebApi", Version = "v1" });
			});


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TournamentManagement.WebApi v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
