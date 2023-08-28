using EdgeDB;
using FluentValidation;
using Reserve.Core.Features.Event;
using Reserve.Endpoints;
using Reserve.Helpers;
using Reserve.Repositories;
using Reserve.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddEdgeDB(EdgeDBConnection.FromInstanceName("reserve"), config =>
{
    config.SchemaNamingStrategy = INamingStrategy.SnakeCaseNamingStrategy;
});
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();
builder.Services.AddScoped<IValidator<CasualEvent>, CasualEventValidator>();
builder.Services.AddScoped<IValidator<CasualTicket>, CasualTicketValidator>();
builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapGroup("/").MapEventsApi();
app.Run();
