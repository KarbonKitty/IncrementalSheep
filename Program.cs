using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using IncrementalSheep;

namespace Company.WebApplication1;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped<IToastService, ToastService>();
        builder.Services.AddSingleton(typeof(GameEngine));

        await builder.Build().RunAsync();
    }
}
