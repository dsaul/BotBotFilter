using Amazon.S3;
using DanSaul.SharedCode.CardDAV;
using DanSaul.SharedCode.Mongo;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;
using Serilog.Events;
using System.Net;

namespace TwilioCallScreening
{
	public class Program
	{
		public static WebApplication? Application { get; private set; }

		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
				.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Debug)
				.WriteTo.Console()
				.CreateLogger();



			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			builder.Host.UseSerilog((HostBuilderContext ctx, LoggerConfiguration lc) =>
			{
				lc.WriteTo.Console();
			});

			builder.Services.AddSingleton<CredentialCache>();
			builder.Services.AddSingleton<HttpClient>((serviceProvider) =>
			{
				CredentialCache creds = serviceProvider.GetRequiredService<CredentialCache>();
				return new(new HttpClientHandler { Credentials = creds });
			});
			builder.Services.AddSingleton<InMemoryCardDavController>((serviceProvider) =>
			{
				CredentialCache credentialCache = serviceProvider.GetRequiredService<CredentialCache>();

				List<CardDavSourceDocument> sources = new()
				{
					new()
					{
						URI = Env.CALDAV_URI,
						AuthType = Env.CALDAV_AUTH_TYPE,
						Password = Env.CALDAV_PASSWORD,
						UserName = Env.CALDAV_USER_NAME
					},
				};


				InMemoryCardDavController ret = new(credentialCache)
				{
					Sources = sources,
				};
				ret.Run();
				return ret;
			});


			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			Application = builder.Build();

			Application.UseSerilogRequestLogging();


			// Configure the HTTP request pipeline.
			if (Application.Environment.IsDevelopment())
			{
				Application.UseSwagger();
				Application.UseSwaggerUI();
			}

			//Application.UseHttpsRedirection();

			Application.UseAuthorization();


			Application.MapControllers();

			_ = Application.Services.GetRequiredService<InMemoryCardDavController>();


			Application.Run();
		}
	}
}//InMemoryCardDavController