using System.Text;
using DotNetAssistant.Auth;
using DotNetAssistant.Core.Caching;
using DotNetAssistant.Data;
using DotNetAssistant.Entities;
using DotNetAssistant.Helpers;
using DotNetAssistant.Model;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
const string secretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
    
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSingleton<IJwtFactory, JwtFactory>();

var jwtAppSettingOptions = builder.Configuration.GetSection(nameof(JwtIssuerOptions));
builder.Services.Configure<JwtIssuerOptions>(options =>
{
    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
    options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
});

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

    ValidateAudience = true,
    ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,

    RequireExpirationTime = false,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddIdentityCore<AppUser>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(configureOptions =>
{
    configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
    configureOptions.TokenValidationParameters = tokenValidationParameters;
    configureOptions.SaveToken = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
});

builder.Services.AddCors(options => options.AddPolicy("defaultx",
    policyBuilder => 
        policyBuilder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRepository<Customer>, EntityRepository<Customer>>();
builder.Services.AddScoped<IRepository<Question>, EntityRepository<Question>>();
builder.Services.AddScoped<IValidator<Question>, QuestionValidator>();


var memoryCache = new MemoryCache(new MemoryCacheOptions());
builder.Services.AddSingleton<IMemoryCache>(memoryCache);
builder.Services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseCors("defaultx");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
;

app.Run();