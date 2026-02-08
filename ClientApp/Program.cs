using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClientApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Point to ServerApp instead of ClientApp origin
var httpClientHandler = new HttpClientHandler();
builder.Services.AddScoped(sp => new HttpClient(httpClientHandler) 
{ 
    BaseAddress = new Uri("http://127.0.0.1:5002") 
});

await builder.Build().RunAsync();
