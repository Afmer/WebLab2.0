using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Weblab.Architecture.Configurations;
using Weblab.Architecture.Interfaces;
using Weblab.Modules.DB;
using Weblab.Modules.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("jwtSettings.json", optional: true, reloadOnChange: true)
    .Build();
builder.Services.Configure<JwtConfiguration>(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder.WithOrigins("http://localhost:3000");
    }
));

builder.Services.AddDbContextPool<ApplicationContext>(options => options
        .UseMySql(
            builder.Configuration.GetConnectionString("MariaDbConnectionString"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MariaDbConnectionString"))
        )
);
builder.Services.AddScoped<IDbHome, DbManagerService>();
builder.Services.AddSingleton<IJwtConfig, JwtConfigService>();
var app = builder.Build();

var jwtConfig = app.Services.GetService<IJwtConfig>();
if(jwtConfig != null)
{
    var jwtOptions = app.Services.GetRequiredService<IOptions<JwtBearerOptions>>();
    jwtOptions.Value.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtConfig.Audience,
        ValidateLifetime = true,
        IssuerSigningKey = jwtConfig.SecurityKey,
        ValidateIssuerSigningKey = true
    };
}
else throw new Exception("jwt config not set");
using(var scope = app.Services.CreateScope())
{
    using(var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
    {
        try
        {
            dbContext.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
