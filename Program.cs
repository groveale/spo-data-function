using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using groveale.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

    // Register your custom service here
    services.AddSingleton<IStorageSnapshotService, StorageSnapshotService>();
    services.AddSingleton<IGraphService, GraphService>();
    services.AddSingleton<ICSVFileService, CSVFileService>();

    })
    .Build();


host.Run();
