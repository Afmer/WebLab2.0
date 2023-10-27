using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MVCSite.Features.Services;
using Weblab.Architecture.Configurations;
using Weblab.Architecture.Constants;
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
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options => {
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies[CookieNames.Jwt];
            if(!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                    if (jsonToken != null)
                    {
                        var usernameClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == JwtClaimsConstant.Login);
                        if (usernameClaim != null)
                        {
                            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usernameClaim.Value) }, context.Scheme.Name);
                            context.Principal = new ClaimsPrincipal(identity);
                            context.Success();
                        }
                    }
                }catch{}
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});
builder.Services.AddScoped<IHash, HashService>();
builder.Services.AddScoped<IDbHome, DbManagerService>();
builder.Services.AddScoped<IDbAuth, DbManagerService>();
builder.Services.AddScoped<IDbShows, DbManagerService>();
builder.Services.AddScoped<IImageService, ImageService>();
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
app.UseAuthentication();
app.UseCookiePolicy(new CookiePolicyOptions
{
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always
});

app.MapControllers();

app.Run();
