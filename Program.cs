using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace IncrementalSheep;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddSingleton<IToastService, ToastService>();
        builder.Services.AddSingleton(typeof(SaveGameProcessor));
        builder.Services.AddSingleton(typeof(GameEngine));

        await builder.Build().RunAsync();
    }
}
