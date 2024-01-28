using System;
using ajh657.Backend.Cache;
using ajh657.Backend.Interop;
using Azure.Identity;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddAzureClients(builder =>
        {
            builder.AddBlobServiceClient(new Uri(hostContext.Configuration["StorageConnectionString"])).WithName("StorageClient");
            builder.UseCredential(new DefaultAzureCredential());
        });
        services.AddSingleton((s) =>
        {
            var endpoint = hostContext.Configuration["CosmosUrl"];
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException("Please specify a valid endpoint in the appSettings.json file or your Azure Functions Settings.");
            }

            var authKey = hostContext.Configuration["CosmosAuthorizationKey"];
            if (string.IsNullOrEmpty(authKey))
            {
                throw new ArgumentException("Please specify a valid AuthorizationKey in the appSettings.json file or your Azure Functions Settings.");
            }

            var configurationBuilder = new CosmosClientBuilder(endpoint, authKey);
            return configurationBuilder
                    .Build();
        });
        services.AddSingleton<ICosmosInterop, CosmosInterop>();
        services.AddSingleton<IStoryCache, StoryCache>();
    })
    .Build();

host.Run();
