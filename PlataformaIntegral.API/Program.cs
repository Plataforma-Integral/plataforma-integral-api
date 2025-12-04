using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minio;
using PlataformaIntegral.API.Helpers;
using PlataformaIntegral.API.Models;
using PlataformaIntegral.API.Services;
using PlataformaIntegral.API.Services.Auth;
using System.Diagnostics;
using System.Text;
using Xabe.FFmpeg;

var builder = WebApplication.CreateBuilder(args);

try
{
    // -------------------- SERVICIOS BÁSICOS --------------------
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new NullableDateOnlyJsonConverter());
        });

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

    // -------------------- SERVICIOS PERSONALIZADOS --------------------
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ICrearCursoService, CrearCursoService>();
    builder.Services.AddScoped<IUsuarioService, UsuarioService>();
    builder.Services.AddScoped<ICursoService, CursoService>();
    builder.Services.AddScoped<ICatalogoService, CatalogoService>();
    builder.Services.AddScoped<MinioService>();

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
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // -------------------- CONFIGURAR MINIO --------------------
    try
    {
        string minioLocalEndpoint = builder.Configuration["Minio:Endpoint"] ?? "localhost:9000";
        string minioAccessKey = builder.Configuration["Minio:AccessKey"] ?? "AdminPI";
        string minioSecretKey = builder.Configuration["Minio:SecretKey"] ?? "CREDENCIAL_ELIMINADA";

        var minioClient = new MinioClient()
            .WithEndpoint(minioLocalEndpoint)
            .WithCredentials(minioAccessKey, minioSecretKey)
            .Build();

        builder.Services.AddSingleton<IMinioClient>(minioClient);
        Debug.WriteLine(" MinioClient configurado correctamente");
    }
    catch (Exception ex)
    {
        Debug.WriteLine(" Error configurando MinioClient: " + ex.Message);
    }

    // -------------------- CONFIGURAR FFMPEG --------------------
    try
    {
        var ffmpegPath = Path.Combine(builder.Environment.ContentRootPath, "ffmpeg", "bin");
        FFmpeg.SetExecutablesPath(ffmpegPath);
        Debug.WriteLine(" FFmpeg configurado correctamente en: " + ffmpegPath);
    }
    catch (Exception ex)
    {
        Debug.WriteLine(" Error configurando FFmpeg: " + ex.Message);
    }

    // -------------------- CONFIGURAR FORM OPTIONS --------------------
    builder.Services.Configure<FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = 1073741824; // 1 GB
    });

    // -------------------- CONFIGURAR KESTREL --------------------
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Limits.MaxRequestBodySize = 1073741824; // 1 GB
        options.ListenAnyIP(5020);
    });
}
catch (Exception ex)
{
    Debug.WriteLine(" Error en configuración de servicios: " + ex.Message);
    Debug.WriteLine(ex.StackTrace);
}

var app = builder.Build();

try
{
    // Swagger siempre activo
    //app.UseSwagger();
    //app.UseSwaggerUI();

    app.UseCors("AllowAll");
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // Test de Minio opcional
    using var scope = app.Services.CreateScope();
    try
    {
        var test = scope.ServiceProvider.GetRequiredService<MinioService>();
        Debug.WriteLine(" MinioService inyectado correctamente");
    }
    catch (Exception ex)
    {
        Debug.WriteLine(" Error inyectando MinioService: " + ex.Message);
    }

    app.Run();
}
catch (Exception ex)
{
    Debug.WriteLine(" Error al iniciar la aplicación: " + ex.Message);
    Debug.WriteLine(ex.StackTrace);
}
