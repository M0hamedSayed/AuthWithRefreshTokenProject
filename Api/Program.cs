using Asp.Versioning;
using Core.Extensions;
using Infrastructure.DatabaseContext;
using Infrastructure.Extensions;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Infrastructure.Seeding;
using Infrastructure.Repositories;
using Infrastructure.Interfaces;
using Service.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Core.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers( options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));

    //Authorization policy
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
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

//Connection To SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
        .AddInterceptors(new SaveChangesInterceptor());
});

// Dependency injections
builder.Services.AddCoreDependancies()
    .AddAuthConfiguration(builder.Configuration)
    .AddInfraDependancies()
    .AddServices();
builder.Services.AddTransient<AdminOnlyFilter>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//Swagger Gn
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth Project", Version = "v1" });

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        },
        Array.Empty<string>()
        }
    });

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

//sedding data
using (var scope = app.Services.CreateScope())
{
    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    await DataSeed.SeedAsync(unitOfWork);
}

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
