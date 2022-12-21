
using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MySqlOsekaiDbContext>(options => 
    options.UseMySql(builder.Configuration.GetConnectionString("MySql")!, MySqlServerVersion.LatestSupportedServerVersion));
builder.Services.AddMemoryCache();

#if DEBUG
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
#else
builder.Services.AddRazorPages();
#endif

builder.Services.AddRouting();

var app = builder.Build();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();

IServiceScope scope = app.Services.CreateScope();

await using (var context = scope.ServiceProvider.GetService<MySqlOsekaiDbContext>()!)
    await context.Database.MigrateAsync();

await app.RunAsync();