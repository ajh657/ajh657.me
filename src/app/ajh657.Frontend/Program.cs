using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace ajh657.Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            var services = builder.Services;
            var rootComponent = builder.RootComponents;

            rootComponent.Add<App>("#app");
            rootComponent.Add<HeadOutlet>("head::after");

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            services.AddRadzenComponents();

            await builder.Build().RunAsync();
        }
    }
}
