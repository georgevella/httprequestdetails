using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RequestDetailsViewer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder()
                .UseKestrel(
                    (context, options) =>
                    {
                        options.ConfigureHttpsDefaults(adapterOptions =>
                            {
                                adapterOptions.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                            }                           
                        );
                        
                        options.ConfigureEndpointDefaults(listenOptions =>
                        {
                            
                        });
                    })
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
