using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVCSite.Features.Services;
using Weblab.Architecture.Configurations;
using Weblab.Architecture.Constants;
using Weblab.Architecture.Enums;
using Weblab.Architecture.Interfaces;
using Weblab.Modules.AuthorizationRequirement;
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
builder.Services.AddSingleton<IJwtConfig, JwtConfigService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(x =>
{
    x.Cookie.Name = CookieNames.Jwt;
})
.AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = configuration["Issuer"],
        ValidateAudience = true,
        ValidAudience = configuration["Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Key"]!)),
        ValidateIssuerSigningKey = true,
        NameClaimType = ClaimsIdentity.DefaultNameClaimType
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies[CookieNames.Jwt];
            if(!string.IsNullOrEmpty(token) && context != null)
            {
                context.Token = token;
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
    options.AddPolicy(PolicyNames.Admin, policy => policy.Requirements.Add(new RoleHierarсhyRequirement(Role.Admin)));
    options.AddPolicy(PolicyNames.User, policy => policy.Requirements.Add(new RoleHierarсhyRequirement(Role.User)));
});
builder.Services.AddScoped<IHash, HashService>();
builder.Services.AddScoped<IDbHome, DbManagerService>();
builder.Services.AddScoped<IDbAuth, DbManagerService>();
builder.Services.AddScoped<IDbShows, DbManagerService>();
builder.Services.AddScoped<IDbFeedback, DbManagerService>();
builder.Services.AddScoped<IDbFavorite, DbManagerService>();
builder.Services.AddScoped<IDbAuthRole, DbManagerService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddTransient<IAuthorizationHandler, RoleHierarсhyHandler>();
var app = builder.Build();

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
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy(new CookiePolicyOptions
{
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always
});

app.MapControllers();

app.Run();
