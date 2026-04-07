using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using QuanLyVanPhongCongChung.API.Authorization;
using QuanLyVanPhongCongChung.API.Middleware;
using QuanLyVanPhongCongChung.Application;
using QuanLyVanPhongCongChung.Infrastructure;
using QuanLyVanPhongCongChung.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Layer DI
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var jwtSection = builder.Configuration.GetSection("Jwt");
var issuer = jwtSection["Issuer"] ?? "QuanLyVanPhongCongChung";
var audience = jwtSection["Audience"] ?? "QuanLyVanPhongCongChung.Client";
var signingKey = jwtSection["SigningKey"] ?? "QuanLyVanPhongCongChung-Dev-Only-Key-Change-Me-2026";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization(options =>
{
    static bool HasAnyRole(ClaimsPrincipal user, params string[] roles)
        => roles.Any(role =>
            user.IsInRole(role) ||
            user.Claims.Any(c =>
                (c.Type == ClaimTypes.Role || c.Type == "role") &&
                string.Equals(c.Value, role, StringComparison.OrdinalIgnoreCase)));

    options.AddPolicy(ExportAuthorizationPolicies.Read, policy =>
        policy.RequireAuthenticatedUser());

    options.AddPolicy(ExportAuthorizationPolicies.GenerateSecureLink, policy =>
        policy.RequireAuthenticatedUser()
              .RequireAssertion(ctx => HasAnyRole(ctx.User, "Admin", "Notary", "LeadAuditor", "Compliance")));

    options.AddPolicy(ExportAuthorizationPolicies.AccessControlUpdate, policy =>
        policy.RequireAuthenticatedUser()
              .RequireAssertion(ctx => HasAnyRole(ctx.User, "Admin", "Notary", "Compliance")));

    options.AddPolicy(ExportAuthorizationPolicies.AuditReportPrint, policy =>
        policy.RequireAuthenticatedUser()
              .RequireAssertion(ctx => HasAnyRole(ctx.User, "Admin", "LeadAuditor", "Compliance")));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "QuanLyVanPhongCongChung API",
        Version = "v1"
    });
});

var app = builder.Build();

// Middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "QuanLyVanPhongCongChung API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
