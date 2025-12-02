using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PlataformaIntegral.API.Models;
using PlataformaIntegral.API.Services;
using PlataformaIntegral.API.Services.Auth;
using System.Text;
using Minio;
using System.Diagnostics;

// -------------------- CONFIGURACIÓN INICIAL --------------------

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICIOS BÁSICOS --------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger con autenticación JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlataformaIntegral API", Version = "v1" });

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

// AutoMapper y DbContext
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<PlataformaIntegralContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlataformaIntegralDB")));

// Servicios personalizados
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICrearCursoService, CrearCursoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<MinioService>();
builder.Services.AddScoped<ICursoService, CursoService>();

// -------------------- JWT --------------------

var jwtKey = builder.Configuration["Jwt:Key"] ?? "quALEgRangrefULPAlMINGentIcHINFe";
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// -------------------- CORS --------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// -------------------- CONFIGURAR MINIO --------------------

string minioNetworkEndpoint = "minio:9000"; // red Docker
string minioLocalEndpoint = builder.Configuration["Minio:Endpoint"] ?? "localhost:9000"; // local
string minioAccessKey = builder.Configuration["Minio:AccessKey"] ?? "AdminPI";
string minioSecretKey = builder.Configuration["Minio:SecretKey"] ?? "CREDENCIAL_ELIMINADA";

IMinioClient? minioClient = null;

async Task<bool> TestMinioConnectionAsync(IMinioClient client)
{
    try
    {
        await client.ListBucketsAsync();
        return true;
    }
    catch (Exception ex)
    {
        return false;
    }
}

/* Probar conexión a MinIO en red Docker primero, si falla probar localhost*/
minioClient = new MinioClient()
        .WithEndpoint(minioLocalEndpoint) //Cambiar despues por minioNetworkEndpoint
        .WithCredentials(minioAccessKey, minioSecretKey)
        .Build();

// Registrar siempre
builder.Services.AddSingleton<IMinioClient>(minioClient);
builder.Services.AddScoped<MinioService>();

// Configurar Kestrel para escuchar en el puerto 5020
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5020); // puerto libre
});

// -------------------- CONSTRUIR APP --------------------

var app = builder.Build();

//Temporal
app.UseSwagger();
app.UseSwaggerUI();

/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
try
{
    var test = scope.ServiceProvider.GetRequiredService<MinioService>();
    Console.WriteLine("MinioService inyectado correctamente");
}
catch (Exception ex)
{
    Console.WriteLine("Error inyectando MinioService: " + ex.Message);
}

app.Run();