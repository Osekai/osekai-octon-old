
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
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
using Osekai.Octon.WebServer.API.V1.DataAdapter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OsuOAuthConfiguration>(builder.Configuration.GetSection("OsuOAuthConfiguration"));

builder.Services.AddDbContext<MySqlOsekaiDbContext>(optionsBuilder => optionsBuilder.UseMySql(
    builder.Configuration.GetConnectionString("MySql")!, MySqlServerVersion.LatestSupportedServerVersion,
    sqlOptions => sqlOptions.MigrationsAssembly("Osekai.Octon.Persistence.EntityFramework.MySql.Migrations").UseMicrosoftJson()));

builder.Services.AddScoped<IDatabaseUnitOfWork, MySqlDatabaseUnitOfWork>();

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
builder.Services.AddScoped<IOsekaiMedalDataGenerator, OsekaiMedalMedaDataGenerator>();
builder.Services.AddScoped<CachedOsekaiMedalDataGenerator>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<PermissionService>();

builder.Services.AddMemoryCache();

IMvcBuilder mvcBuilder = builder.Services.AddRazorPages();
#if DEBUG
    mvcBuilder.AddRazorRuntimeCompilation();
    builder.Services.AddScoped<ITestDataPopulator, MySqlTestDataPopulator>();
#endif

mvcBuilder.AddMvcOptions(
    options => options.Filters.Add(new SaveUnitOfWorkChangesPageFilter()));

builder.Services.AddControllers()
    .AddMvcOptions(options => options.Filters.Add(new SaveUnitOfWorkChangesControllerFilter()))
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddRouting();

var app = builder.Build();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MySqlOsekaiDbContext>()!;
    await context.Database.MigrateAsync();
}

await app.RunAsync();