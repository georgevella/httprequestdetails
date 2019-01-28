using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;

namespace RequestDetailsViewer
{
	public class Startup
	{
		private readonly IOptions<KestrelServerOptions> _kesterServerOptions;

		public Startup(IOptions<KestrelServerOptions> kesterServerOptions)
		{
			_kesterServerOptions = kesterServerOptions;
		}
		
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.Run(async (context) =>
			{
				context.Response.Headers["Content-Type"] = "application/json";

				var requestInfo = new
				{
					Request = new {
						context.Request.Scheme,
						context.Request.Method,
						Path = context.Request.Path.HasValue ? context.Request.Path.Value : "",
						QueryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : "",
						context.Request.Host,
						context.Request.Protocol,
					},
					Connection = new {
						Remote = new
						{
							Ip = context.Connection.RemoteIpAddress.ToString(),
							Port = context.Connection.RemotePort
						},
						ClientCertificate = context.Connection.ClientCertificate?.ToString(true )
					},
					Headers = context.Request.Headers.ToDictionary( x => x.Key, x => x.Value.ToString() ),
					Cookies = context.Request.Cookies.ToDictionary( x => x.Key, x => x.Value.ToString() ),
					Content = new {
						HasContent = context.Request.ContentLength != null, 
						context.Request.ContentLength,
						context.Request.ContentType,
					},
					context.TraceIdentifier,
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
