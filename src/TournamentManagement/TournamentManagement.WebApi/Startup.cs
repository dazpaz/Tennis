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
using TournamentManagement.Application.Commands;
using TournamentManagement.Application.Decorators;
using TournamentManagement.Application.Queries;
using TournamentManagement.Application.Repository;
using TournamentManagement.Contract;
using TournamentManagement.Data;
using TournamentManagement.Data.Repository;

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
			services.AddTransient<ICommandHandler<AddTournamentCommand, Guid>>(provider =>
				new AuditDecorator<AddTournamentCommand, Guid>(
					new NullDecorator<AddTournamentCommand, Guid>(
						new AddTournamentCommandHandler(provider.GetService<IUnitOfWork>()))));
			services.AddTransient<ICommandHandler<AmendTournamentCommand>>(provider =>
				new AuditDecorator<AmendTournamentCommand> (
					new NullDecorator<AmendTournamentCommand>(
						new AmendTournamentCommandHandler(provider.GetService<IUnitOfWork>()))));
			services.AddTransient<ICommandHandler<AddEventCommand>>(provider =>
				new AuditDecorator<AddEventCommand>(
					new NullDecorator<AddEventCommand>(
						new AddEventCommandHandler(provider.GetService<IUnitOfWork>()))));
			services.AddTransient<ICommandHandler<AmendEventCommand>, AmendEventCommandHandler>();
			services.AddTransient<ICommandHandler<RemoveEventCommand>, RemoveEventCommandHandler>();
			services.AddTransient<ICommandHandler<OpenForEntriesCommand>, OpenForEntriesCommandHandler>();
			services.AddTransient<IQueryHandler<GetTournamentSummaryList, List<TournamentSummaryDto>>,
				GetTournamentSummaryListHandler>();
			services.AddTransient<IQueryHandler<GetTournamentDetailsList, List<TournamentDetailsDto>>,
				GetTournamentDetailsListHandler>();
			services.AddTransient<IQueryHandler<GetEvent, EventDto>, GetEventHandler>();
			services.AddTransient<IQueryHandler<GetTournamentDetails, TournamentDetailsDto>,
				GetTournamentDetailsHandler>();
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
