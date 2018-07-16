﻿using JKang.IpcServiceFramework;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace IpcServiceSample.WebServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost webHost = BuildWebHost(args);

            ThreadPool.QueueUserWorkItem(StartIpcService,
                webHost.Services.CreateScope().ServiceProvider);

            webHost.Run();
        }

        private static void StartIpcService(object state)
        {
            var serviceProvider = state as IServiceProvider;
            new IpcServiceHostBuilder(serviceProvider)
                .AddNamedPipeEndpoint("ep1", "pipeName")
                .Build()
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
