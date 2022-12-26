
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using Osekai.Octon;
using Osekai.Octon.Applications;
using Osekai.Octon.Applications.OsuApiV2;
using Osekai.Octon.Database;
using Osekai.Octon.Database.EntityFramework;
using Osekai.Octon.Database.EntityFramework.Repositories;
using Osekai.Octon.Database.Repositories;
using Osekai.Octon.Options;
using Osekai.Octon.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<OsuOAuthConfiguration>(builder.Configuration.GetSection("OsuOAuthConfiguration"));

builder.Services.AddDbContext<MySqlOsekaiDbContext>(options => 
    options.UseMySql(builder.Configuration.GetConnectionString("MySql")!, MySqlServerVersion.LatestSupportedServerVersion, 
        sqlOptions => sqlOptions.MigrationsAssembly("Osekai.Octon.Database.EntityFramework.MySql.Migrations")));

builder.Services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
builder.Services.AddSingleton<ObjectPool<StringBuilder>>(serviceProvider =>
{
    var provider = serviceProvider.GetRequiredService<ObjectPoolProvider>();
    var policy = new StringBuilderPooledObjectPolicy();
    return provider.Create(policy);
});

builder.Services.AddHttpClient();
builder.Services.AddSingleton<OsuApiTimeThrottler>();
builder.Services.AddScoped<OsuApiV2>();
builder.Services.AddScoped<AuthenticatedOsuApiV2>();
builder.Services.AddScoped<CurrentSession>();
builder.Services.AddScoped<IOsuApiV2TokenProvider, CurrentSession>();
builder.Services.AddScoped<DbContext, MySqlOsekaiDbContext>();
builder.Services.AddScoped<ITransactionProvider, EntityFrameworkTransactionProvider>();
builder.Services.AddTransient<IUnitOfWork, MySqlUnitOfWork>();
builder.Services.AddScoped<ITokenGenerator, RandomBytes128BitTokenGenerator>();
builder.Services.AddSingleton<StaticUrlGenerator>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddMemoryCache();

#if DEBUG
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddScoped<ITestDataPopulator, MySqlTestDataPopulator>();
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