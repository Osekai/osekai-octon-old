
using System.Text;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IO;
using Osekai.Octon.WebServer;
using Osekai.Octon;
using Osekai.Octon.Caching;
using Osekai.Octon.Caching.Codecs.MsgPack;
using Osekai.Octon.Caching.Storages.MicrosoftInMemoryCache;
using Osekai.Octon.Domain.Aggregates;
using Osekai.Octon.Localization;
using Osekai.Octon.Localization.File;
using Osekai.Octon.OsuApi;
using Osekai.Octon.Persistence.EntityFramework.MySql;
using Osekai.Octon.Options;
using Osekai.Octon.OsuApi.Payloads;
using Osekai.Octon.Persistence;
using Osekai.Octon.Services;
using Osekai.Octon.WebServer.API.V1.Dtos.AppFaqController;
using Osekai.Octon.WebServer.API.V1.Dtos.UserController;
using Osekai.Octon.WebServer.Presentation;
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

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<RecyclableMemoryStreamManager>();

builder.Services.AddSingleton<ICache, ConfigurableCache>(p => 
    new ConfigurableCache(p.GetRequiredService<RecyclableMemoryStreamManager>(), 
        new MicrosoftInMemoryCacheStorage(p.GetRequiredService<IMemoryCache>()),
        new MsgPackCacheCodec()));

builder.Services.AddHttpClient();
builder.Services.AddScoped<OsuApiV2Interface>();
builder.Services.AddSingleton<IAuthenticatedOsuApiV2Interface, AuthenticatedOsuApiV2Interface>();
builder.Services.AddScoped<CachedAuthenticatedOsuApiV2Interface>();
builder.Services.AddScoped<AuthenticatedOsuApiV2Interface>();
builder.Services.AddScoped<CurrentSession>();
builder.Services.AddScoped<AppService>();
builder.Services.AddScoped<IOsuApiV2SessionProvider>(provider => provider.GetRequiredService<CurrentSession>());
builder.Services.AddScoped<ITokenGenerator, RandomBytes128BitTokenGenerator>();
builder.Services.AddSingleton<StaticUrlGenerator>();
builder.Services.AddScoped<IAdapter<App, AppBaseLayoutApp>, AppBaseLayoutAppFromAppAdapter>();
builder.Services.AddSingleton<IAdapter<Medal, AppBaseLayoutMedal>, AppBaseLayoutMedalFromMedalAdapter>();
builder.Services.AddSingleton<IAdapter<UserGroup, AppBaseLayoutUserGroup>, AppBaseLayoutUserGroupAdapter>();
builder.Services.AddScoped<IAdapter<App, AppWithFaqDto>, AppWithFaqDtoFromAppAdapter>();
builder.Services.AddSingleton<IAdapter<(OsuUser, IEnumerable<UserGroup>), UserDto>, UserDtoFromOsuUserAndAggregateAdapter>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<UserGroupService>();
builder.Services.AddScoped<MedalService>();
builder.Services.AddScoped<LocaleService>();
builder.Services.AddScoped<CurrentLocale>();

builder.Services.AddSingleton<ILocalizatorFactory, CachedLocalizatorFactory>(serviceProvider => 
    new CachedLocalizatorFactory(
        new FileLocalizatorFactory(serviceProvider.GetRequiredService<ObjectPool<StringBuilder>>(), "../OsekaiOld/global/lang/")));

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
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/global/fonts")),
    RequestPath = "/static/shared/fonts"
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

app.UseStaticFiles(new StaticFileOptions(new SharedOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/global/img")),
    RequestPath = "/global/img"
}));

app.UseStaticFiles(new StaticFileOptions(new SharedOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/global/js")),
    RequestPath = "/global/js"
}));

app.UseStaticFiles(new StaticFileOptions(new SharedOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/global/lang")),
    RequestPath = "/global/lang"
}));

app.UseStaticFiles(new StaticFileOptions(new SharedOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/global/lang")),
    RequestPath = "/global/lang"
}));

app.UseStaticFiles(new StaticFileOptions(new SharedOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/home/img")),
    RequestPath = "/home/img"
}));


app.UseStaticFiles(new StaticFileOptions(new SharedOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/home/js")),
    RequestPath = "/home/js"
}));

app.UseStaticFiles(new StaticFileOptions(new SharedOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "../OsekaiOld/home/css")),
    RequestPath = "/home/css"
}));

app.UseRouting();

app.MapRazorPages();
app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MySqlOsekaiDbContext>();
    await context.Database.MigrateAsync();
}

await app.RunAsync();