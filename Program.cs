using DotNetEnv;


public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {   
                webBuilder.UseStartup<Startup>();
            });
}

