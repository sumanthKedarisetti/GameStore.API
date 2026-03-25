using GameStore.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using GameStore.API.Entities;
using GameStore.API.Interfaces.IServices;
using GameStore.API.Services;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;


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
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Paste your Keycloak JWT token here (without the 'Bearer' prefix)"
        });
        document.SecurityRequirements.Add(new OpenApiSecurityRequirement
        {
            [
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                }
            ] = Array.Empty<string>()
        });
        return Task.CompletedTask;
    });
});
// Registering the GameService with the dependency injection container
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GameStoreDB")));

//building the app
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
// Enable middleware to serve generated OpenAPI as a JSON endpoint and the Swagger UI
app.MapOpenApi().AllowAnonymous();
// Enable middleware to serve generated Swagger as a JSON endpoint and the Swagger UI
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "GameStore API v1");
    options.EnablePersistAuthorization();
});

app.MapControllers().RequireAuthorization();
// default root endpoint
app.MapGet("/", () => "Game Store API is running. Use a Bearer token to access protected endpoints.").AllowAnonymous();
// Run the application
app.Run();
 