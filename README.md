# Plataforma Integral — API

Backend REST de la **Plataforma Integral**, sistema universitario de gestión educativa
desarrollado como proyecto final en la UNIVALLE. Gestiona cursos, inscripciones,
clases (presenciales y virtuales), profesores, estudiantes, pagos y certificados.

> **Nota:** Proyecto universitario archivado. Configurar credenciales propias en
> `appsettings.Development.json` para correrlo localmente (ver abajo).

## Stack

| Pieza | Tecnología |
|---|---|
| Runtime | ASP.NET Core / C# |
| Base de datos | SQL Server (Entity Framework Core) |
| Almacenamiento de archivos | MinIO (S3-compatible) |
| Auth | JWT Bearer |
| Documentación | OpenAPI |

## Módulos principales

- **Auth** — registro y login (roles: estudiante, profesor, administrador)
- **Cursos** — pregrabados, sincrónicos y presenciales; capítulos, clases, recursos
- **Compras y suscripciones** — pagos, recibos, métodos de pago
- **Progreso** — avance del estudiante, exámenes, cuestionarios
- **Certificados y medallas** — emisión al completar cursos
- **Archivos** — subida y descarga desde MinIO
- **Catálogos** — países, categorías, modalidades, roles, etc.

## Configuración local

Requiere .NET SDK, SQL Server y MinIO.

```bash
git clone https://github.com/Plataforma-Integral/plataforma-integral-api
cd plataforma-integral-api/PlataformaIntegral.API
```

Crear `appsettings.Development.json` (ignorado por git) con tus credenciales locales:

```json
{
  "ConnectionStrings": {
    "PlataformaIntegralDB": "Server=localhost,1433;Database=PlataformaIntegralDB;User Id=...;Password=...;TrustServerCertificate=True;"
  },
  "Jwt":   { "Key": "...", "Issuer": "PlataformaIntegralAPI", "Audience": "PlataformaIntegralApps" },
  "Minio": { "Endpoint": "localhost:9000", "AccessKey": "...", "SecretKey": "...", "Secure": false }
}
```

```bash
dotnet ef database update
dotnet run
```

La documentación OpenAPI queda disponible en `http://localhost:<puerto>/openapi`.
