using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
