using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PlataformaIntegral.API.Models;
using PlataformaIntegral.API.Services;
using PlataformaIntegral.API.Services.Auth;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger para soportar autenticación con JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlataformaIntegral API", Version = "v1" });

    // Esquema para incluir el JWT en Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa: Bearer <tu-token>"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configurar AutoMapper detectando todos los Profiles del proyecto
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Agregar EF Core
builder.Services.AddDbContext<PlataformaIntegralContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlataformaIntegralDB")));

// Inyectar servicios personalizados
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();


// Configurar JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "quALEgRangrefULPAlMINGentIcHINFe"; // Contraseña por defecto
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
    // indican que la autenticación será por Bearer JWT.
})
.AddJwtBearer("Bearer", options =>
{
    // parámetros que la API usa para validar los tokens recibidos
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // Puedes activarlo luego si tienes dominio fijo
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Inyectar servicios personalizados - Pipeline de servicios
var app = builder.Build();

// Habilitar autenticación antes de la autorización
app.UseAuthentication();

// Luego habilitar autorización
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Autorizar después de autenticar
app.UseAuthorization();

app.MapControllers();

app.Run();