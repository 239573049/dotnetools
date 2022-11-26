using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dotnetools.Cache;
using Dotnetools.WabiSabi.Models.Serialization;

namespace Dotnetools.Tests.UnitTests.WabiSabi.Integration;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		app.UseStaticFiles();
		app.UseRouting();
		app.UseEndpoints(endpoints => endpoints.MapControllers());
	}

	public void ConfigureServices(IServiceCollection services)
	{
		var backendAssembly = typeof(Dotnetools.Backend.Controllers.WabiSabiController).Assembly;
		services.AddSingleton<IdempotencyRequestCache>();
		services.AddMvc()
			.AddApplicationPart(backendAssembly)
			.AddControllersAsServices()
			.AddNewtonsoftJson(x => x.SerializerSettings.Converters = JsonSerializationOptions.Default.Settings.Converters);
	}
}
