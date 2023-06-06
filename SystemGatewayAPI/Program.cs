using SystemGateway.Configurations;
using SystemGateway.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.Configure<DatabaseApiConfigSection>(builder.Configuration.GetSection("DatabaseApiConfigSection"));
builder.Services.Configure<SecurityManagerConfigSection>(builder.Configuration.GetSection("SecurityManagerConfigSection"));
builder.Services.Configure<OperationsApiConfigSection>(builder.Configuration.GetSection("OperationsApiConfigSection"));
builder.Services.AddSingleton<ISecurityManagerProvider, SecurityManagerProvider>();
builder.Services.AddSingleton<IOperationsProvider, OperationsProvider>();
builder.Services.AddSingleton<IDatabaseProvider, DatabaseProvider>();
builder.Services.AddSingleton<IServiceAggregator, ServiceAggregator>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
