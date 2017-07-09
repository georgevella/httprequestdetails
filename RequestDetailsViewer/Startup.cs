using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RequestDetailsViewer
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.Run(async (context) =>
			{
				context.Response.Headers["Content-Type"] = "application/json";

				var requestInfo = new
				{
					context.TraceIdentifier,
					RemoteConnectionInfo = new
					{
						Ip = context.Connection.RemoteIpAddress.ToString(),
						Port = context.Connection.RemotePort
					},
					Headers = context.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()),
					context.Request.ContentLength,
					context.Request.ContentType,
					context.Request.Method,
					Path = context.Request.Path.HasValue ? context.Request.Path.Value : "",
					QueryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : "",
					context.Request.Host,
					context.Request.Protocol,
					context.Request.Scheme,
				};

				using (var streamWriter = new StreamWriter(context.Response.Body))
				using (var jsonWriter = new JsonTextWriter(streamWriter))
				{
					Serializer.Json.Serialize(jsonWriter, requestInfo);
				}

				await Task.CompletedTask;
			});
		}
	}
}
