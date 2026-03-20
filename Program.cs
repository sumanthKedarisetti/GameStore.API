using GameStore.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using GameStore.API.Entities;

var builder = WebApplication.CreateBuilder(args);
var keycloakSection = builder.Configuration.GetSection("Keycloak");
var authority = keycloakSection["Authority"]
    ?? throw new InvalidOperationException("Missing Keycloak:Authority in configuration.");
var audience = keycloakSection["Audience"] ?? keycloakSection["ClientId"]
    ?? throw new InvalidOperationException("Missing Keycloak:Audience (or Keycloak:ClientId) in configuration.");
var requireHttpsMetadata = keycloakSection.GetValue<bool?>("RequireHttpsMetadata")
    ?? authority.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

builder.Services.AddAuthentication(options=>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options=>
{
    options.Authority = authority;
    options.Audience = audience;
    options.RequireHttpsMetadata = requireHttpsMetadata;
});
builder.Services.AddAuthorization(options =>
{
    // Enforce authentication for all endpoints by default
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddControllers();

builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GameStoreDB")));

//building the app
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
// default root endpoint
app.MapGet("/", () => "Game Store API is running. Use a Bearer token to access protected endpoints.").AllowAnonymous();
// Run the application
app.Run();
 