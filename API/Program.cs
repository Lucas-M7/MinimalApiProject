using MinimalApi;

IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(WebHostBuilder =>
    {
        WebHostBuilder.UseStartup<Startup>();
    });
}

CreateHostBuilder(args).Build().Run();