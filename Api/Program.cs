using Asp.Versioning;
using Core.Extensions;
using Infrastructure.DatabaseContext;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Include API versions in response headers
    options.ApiVersionReader = new UrlSegmentApiVersionReader(); //Reads version number from request url at "apiVersion" constraint
    options.AssumeDefaultVersionWhenUnspecified = true; // Assume default version if none specified
    options.DefaultApiVersion = new ApiVersion(1, 0); // Set default API version
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; //v1
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Connection To SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
        .AddInterceptors(new SaveChangesInterceptor());
});

//allowed origins
builder.Services.AddCors(options => {

    options.AddDefaultPolicy(policyBuilder =>
    {
        string[] defaultOrigins = ["*"];
        policyBuilder
        .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? defaultOrigins)
        .WithMethods("GET", "POST", "PUT", "DELETE")
        .AllowCredentials();
    });
});

//Serilog
Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Services.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// handle all errors middleware
app.UseErrorHandlerMiddleware();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
