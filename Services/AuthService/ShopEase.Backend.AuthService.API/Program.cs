using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Infrastructure;
using ShopEase.Backend.AuthService.Infrastructure.MailService;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Add services to the container.

// Adding DB Context
var connectionString = builder.Configuration.GetConnectionString("ShopEaseDB") ?? string.Empty;
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));

// Adding Repository
builder.Services.AddScoped<IAuthServiceRepository, AuthServiceRepository>();

// Adding Mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApiService).Assembly));
builder.Services.AddScoped<IApiService, ApiService>(c =>
{
    var mediator = c.GetRequiredService<IMediator>();
    return new ApiService(mediator);
});

// Adding Auth Helper
builder.Services.AddScoped<IAuthHelper, AuthHelper>();

// Adding Email Service
builder.Services.AddSingleton<IEmailService, EmailService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Secret").Value)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetSection("AppSettings:Issuer").Value,
            ValidateAudience = false
        });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "ShopEase AuthServices API",
        Description = "APIs for ShopEase AuthServices",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Kaustab Samanta",
            Email = "scriptsage001@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/kaustab-samanta-b513511a1")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    s.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopEase AuthServices API V1"));
//}

app.UseSwagger();
app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopEase AuthServices API V1"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
