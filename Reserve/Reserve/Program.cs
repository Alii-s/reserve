using EdgeDB;
using FluentValidation;
using Reserve.Core.Features.Queue;
using Reserve.Core.Features.Event;
using Reserve.Core.Features.MailService;
using Reserve.Endpoints;
using Reserve.Helpers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
               _ => "This field is required.");
});
builder.Services.AddEdgeDB(EdgeDBConnection.FromInstanceName("reserve"), config =>
{
    config.SchemaNamingStrategy = INamingStrategy.SnakeCaseNamingStrategy;
});
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();
builder.Services.AddScoped<IValidator<QueueEvent>, QueueEventValidator>();
builder.Services.AddScoped<IValidator<QueueTicket>, QueueTicketValidator>();
builder.Services.AddScoped<IValidator<CasualEventInput>, CasualEventInputValidator>();
builder.Services.AddScoped<IValidator<CasualTicketInput>, CasualTicketInputValidator>();
builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();
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
