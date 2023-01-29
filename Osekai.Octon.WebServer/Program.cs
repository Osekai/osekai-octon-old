
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IO;
using Osekai.Octon.WebServer;
using Osekai.Octon;
using Osekai.Octon.Caching.MsgPack;
using Osekai.Octon.OsuApi;
using Osekai.Octon.Persistence.EntityFramework.MySql;
using Osekai.Octon.Options;
using Osekai.Octon.Persistence;
using Osekai.Octon.Persistence.Aggregators;
using Osekai.Octon.Services;
using Osekai.Octon.WebServer.Presentation.AppBaseLayout;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OsuOAuthConfiguration>(builder.Configuration.GetSection("OsuOAuthConfiguration"));

builder.Services.AddDbContext<MySqlOsekaiDbContext>(optionsBuilder => optionsBuilder.UseMySql(
    builder.Configuration.GetConnectionString("MySql")!, MySqlServerVersion.LatestSupportedServerVersion,
    sqlOptions => sqlOptions.MigrationsAssembly("Osekai.Octon.Persistence.EntityFramework.MySql.Migrations").UseMicrosoftJson()));

builder.Services.AddScoped<IUnitOfWork, MySqlUnitOfWork>();

builder.Services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
builder.Services.AddSingleton<ObjectPool<StringBuilder>>(serviceProvider =>
{
    var provider = serviceProvider.GetRequiredService<ObjectPoolProvider>();
    var policy = new StringBuilderPooledObjectPolicy();
    return provider.Create(policy);
});

builder.Services.AddSingleton<RecyclableMemoryStreamManager>();
builder.Services.AddScoped<ICache, MsgPackDatabaseCache>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<OsuApiV2Interface>();
builder.Services.AddScoped<IAuthenticatedOsuApiV2Interface, AuthenticatedOsuApiV2Interface>();
builder.Services.AddScoped<CachedAuthenticatedOsuApiV2Interface>();
builder.Services.AddScoped<AuthenticatedOsuApiV2Interface>();
builder.Services.AddScoped<CurrentSession>();
builder.Services.AddScoped<AppService>();
builder.Services.AddScoped<IOsuApiV2SessionProvider>(provider => provider.GetRequiredService<CurrentSession>());
builder.Services.AddScoped<ITokenGenerator, RandomBytes128BitTokenGenerator>();
builder.Services.AddSingleton<StaticUrlGenerator>();
builder.Services.AddScoped<IMedalDataAggregator, MySqlEntityFrameworkMedalDataAggregator>();
builder.Services.AddScoped<IAppBaseLayoutMedalDataGenerator, AppBaseLayoutMedaDataGenerator>();
builder.Services.AddScoped<CachedAppBaseLayoutMedalDataGenerator>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<UserGroupService>();
builder.Services.AddScoped<IAppBaseLayoutUserGroupDataGenerator, AppBaseLayoutUserGroupDataGenerator>();

builder.Services.AddMemoryCache();

IMvcBuilder mvcBuilder = builder.Services.AddRazorPages();
#if DEBUG
    mvcBuilder.AddRazorRuntimeCompilation();
    builder.Services.AddScoped<ITestDataPopulator, MySqlTestDataPopulator>();
#endif

builder.Services.AddRouting();

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions(new SharedOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/global/css")),
    RequestPath = "/static/shared/css"
}));

app.UseStaticFiles(new StaticFileOptions(new SharedOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/global/js")),
    RequestPath = "/static/shared/js"
}));

app.UseStaticFiles(new StaticFileOptions(new SharedOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/home/js")),
    RequestPath = "/static/home/js"
}));


app.UseStaticFiles(new StaticFileOptions(new SharedOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/home/css")),
    RequestPath = "/static/home/css"
}));

app.UseRouting();

app.MapRazorPages();
app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MySqlOsekaiDbContext>()!;
    await context.Database.MigrateAsync();
}

await app.RunAsync();