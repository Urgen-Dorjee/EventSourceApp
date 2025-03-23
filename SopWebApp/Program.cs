using Application.AppDependencyInjection;
using Domain.Contacts;
using EventStore.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence.DataContext;
using Persistence.DependencyInjections;
using SopWebApp;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer Api", Version = "v1" });

    var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
    c.IncludeXmlComments(apiXmlPath);

    var applicationXmlFile = "Application.xml";
    var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, applicationXmlFile);
    c.IncludeXmlComments(applicationXmlPath);
});

builder.Services.AddPersistenceService();
builder.Services.AddApplicationService();

builder.Services.AddDbContext<CustomerDbContext>(optionsAction =>
    optionsAction.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var eventStoreSettings = builder.Configuration
    .GetSection("EventStore")
    .Get<EventStoreSettings>() ?? throw new InvalidOperationException("Missing EventStore configuration.");

builder.Services.AddSingleton(eventStoreSettings);

builder.Services.AddSingleton(sp =>
{
    var settings = EventStoreClientSettings.Create(eventStoreSettings.ConnectionString);
    return new EventStoreClient(settings);
});

builder.Services.AddScoped<IEventStore, EventStoreDbRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    context.Database.Migrate();
    await DbInitializer.SeedAsync(context);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();