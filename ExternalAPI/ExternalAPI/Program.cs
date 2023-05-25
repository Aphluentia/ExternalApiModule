using ExternalApi.Services;
using ExternalAPI.Configurations;
using ExternalAPI.Services;
using ExternalAPI.Services.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseApiConfigSection>(builder.Configuration.GetSection("DatabaseApiConfigSection"));
builder.Services.Configure<SessionConfigSection>(builder.Configuration.GetSection("SessionConfigSection"));
builder.Services.Configure<BrokerApiConfigSection>(builder.Configuration.GetSection("BrokerApiConfigSection")); 
builder.Services.AddSingleton<ISessionProvider, SessionProvider>();
builder.Services.AddSingleton<IBrokerProvider, BrokerProvider>();
builder.Services.AddSingleton<IDatabaseProvider, DatabaseProvider>();
builder.Services.AddSingleton<IServiceAggregator, ServiceAggregator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
