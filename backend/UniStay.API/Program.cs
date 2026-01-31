/*using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniStay.API.Data;
using UniStay.API.Helper;
using UniStay.API.Helper.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;
using UniStay.API.Services;
using System.Security.Cryptography;

var config = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", false)
.Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("db1")));

builder.Services.AddScoped<IMyAuthService, MyAuthService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllpwFrontend",
        policy =>
        {
            policy.WithOrigins("http:localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(x => x.OperationFilter<MyAuthorizationSwaggerHeader>());
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "UniStay.API", Version = "v1" });

    // AUTH
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddFluentValidationAutoValidation();


var app = builder.Build();

app.UseCors("AllowFrontend");


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(
    options => options
        .SetIsOriginAllowed(x => _ = true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
); //This needs to set everything allowed


app.UseAuthorization();

app.MapControllers();

app.Run();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

app.UseCors("AllowLocalhost");*/




using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using UniStay.API.Data;
using UniStay.API.Helper;
using UniStay.API.Helper.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;
using UniStay.API.Services;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UniStay.API.Endpoints.UserEndpoints;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;   
using System.Text;
using Stripe;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .Build();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// ✅ Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("db1")));

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
// ✅ Auth
builder.Services.AddScoped<IMyAuthService, MyAuthService>();
builder.Services.AddScoped<ITwoFactorService, TwoFactorService>();

builder.Services.AddValidatorsFromAssemblyContaining<FaultCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateValidator>();

// Stripe service 
builder.Services.AddScoped<StripeService>();
// ✅ Halls service (za CRUD)
builder.Services.AddScoped<HallsService>();

// ✅ HttpContext
builder.Services.AddHttpContextAccessor();

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ✅ Fluent Validation
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            // Provjeri da se putanja točno podudara s onom u MapHub
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/hubs/chat"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// ✅ AutoMapper
builder.Services.AddAutoMapper(typeof(Program));




builder.Services.AddScoped<ITwoFactorService, TwoFactorService>();

// ✅ Controllers i Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "UniStay.API", Version = "v1" });

    // JWT Auth u Swaggeru
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();


builder.Services.AddScoped< AnalyticsService>();
builder.Services.AddHostedService<AnalyticsBackgroundService>();
builder.Services.AddScoped<AnalyticsBroadcaster>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("db1"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddHttpContextAccessor(); // registruje IHttpContextAccessor

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //await db.Database.MigrateAsync();               // ako koristiš migracije
    await DynamicDataSeeder.SeedAsync(db);
}


// ✅ Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/hubs/chat");
app.MapControllers();

app.MapHub<AnalyticsHub>("/hubs/analytics");

// (kasnije ćemo dodati) app.MapHallEndpoints();

app.Run();