using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Copilot helped configure CORS to allow frontend requests from different ports
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientApp", policy =>
    {
        policy.WithOrigins("http://localhost:5267", "https://localhost:7157", "http://127.0.0.1:5267", "https://127.0.0.1:7157")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Copilot suggested using ConfigureHttpJsonOptions to maintain PascalCase property naming
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

// Copilot recommended adding in-memory caching to reduce server load on repeated requests
builder.Services.AddMemoryCache();

var app = builder.Build();

// Enable CORS
app.UseCors("AllowClientApp");

// Copilot suggested implementing server-side caching with IMemoryCache to minimize server load
app.MapGet("/api/productlist", (IMemoryCache cache) =>
{
    const string cacheKey = "productlist_cache";
    
    if (!cache.TryGetValue(cacheKey, out object? cachedProducts))
    {
        cachedProducts = new object[]
        {
            new
            {
                Id = 1,
                Name = "Laptop",
                Price = 1200.50M,
                Stock = 25,
                Category = new { Id = 101, Name = "Electronics" }
            },
            new
            {
                Id = 2,
                Name = "Headphones",
                Price = 50.00M,
                Stock = 100,
                Category = new { Id = 102, Name = "Accessories" }
            },
            new
            {
                Id = 3,
                Name = "USB-C Cable",
                Price = 15.99M,
                Stock = 250,
                Category = new { Id = 102, Name = "Accessories" }
            }
        };
        
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        cache.Set(cacheKey, cachedProducts, cacheOptions);
    }
    
    return cachedProducts;
});

app.Run();