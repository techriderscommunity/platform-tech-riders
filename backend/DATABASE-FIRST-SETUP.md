# Database-First Setup - TechRiders Backend

## Diagnóstico

El backend canónico de la solución es `TechRiders.Api`.

La base local que existe en LocalDB es `TechRidersDev`.

El documento anterior mezclaba comandos de migraciones EF
(`migrations add` / `database update`) con el objetivo
de trabajar database-first.
Eso empuja a un flujo code-first y además contradice
la BD local real.

## Contrato obligatorio

1. La base de datos existente es la fuente de verdad.
2. Para desarrollo normal no se crean migraciones nuevas ni se recrea la BD local.
3. Cuando cambie el esquema, se hace reverse engineering desde la BD hacia el backend.
4. El startup project para scaffolding y ejecución es `TechRiders.Api`.

## Base local activa

```text
Server=(localdb)\MSSQLLocalDB
Database=TechRidersDev
```

Tablas detectadas en la BD local:

- `dbo.Ambassadors`
- `dbo.candidaturas`
- `dbo.Centers`
- `dbo.Events`
- `dbo.FPTours`
- `dbo.intranet_audit_logs`
- `dbo.intranet_settings`
- `dbo.intranet_user_categories`
- `dbo.MT_Categories`
- `dbo.ofertas`
- `dbo.Sessions`
- `dbo.tutoriales`

## Flujo database-first

### 1. Ejecutar la API canónica

```powershell
cd .\backend\TechRiders.Api
dotnet run
```

### 2. Regenerar snapshot EF desde la BD existente

Desde `projects/tetxito/backend`:

```powershell
.\scaffold-db-first.ps1 -Force
```

Salida generada:

- `TechRiders.Infrastructure/Data/DatabaseFirstSnapshot/`
- `TechRiders.Infrastructure/Data/DatabaseFirstSnapshot/Entities/`

Ese snapshot sirve para validar y sincronizar el
contrato real de SQL Server sin volver a declarar
el esquema a mano.

### 3. Usar Azure más adelante

Cuando cambies a Azure SQL, reutiliza el mismo flujo con otra connection string:

```powershell
.\scaffold-db-first.ps1 `
  -ConnectionString (
    "Server=tcp:<server>.database.windows.net,1433;" +
    "Initial Catalog=<database>;Persist Security Info=False;" +
    "User ID=<user>;Password=<password>;" +
    "MultipleActiveResultSets=True;Encrypt=True;" +
    "TrustServerCertificate=False;Connection Timeout=30;"
  ) `
  -Force
```

También puedes definir `TECHRIDERS_DB_CONNECTIONSTRING`
y ejecutar el script sin pasar la cadena por parámetro.

## Qué no hacer

- No usar `dotnet ef migrations add` como flujo normal de desarrollo.
- No usar `dotnet ef database update` para recrear la BD local ya provisionada.
- No usar rutas legacy del proyecto previo en scripts/documentación;
  el startup canónico es `TechRiders.Api`.

## Verificación rápida

### Comprobar que LocalDB está disponible

```powershell
sqllocaldb info
```

### Comprobar que la BD existe

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT name FROM sys.databases ORDER BY name"
```

### Comprobar el esquema actual

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" `
  -d "TechRidersDev" `
  -Q @"
SELECT s.name AS [schema], t.name AS [table]
FROM sys.tables t
INNER JOIN sys.schemas s ON s.schema_id = t.schema_id
ORDER BY s.name, t.name
"@
```

### Comprobar salud de la API

```text
GET /health
```

## Referencias

- Startup canónico: `backend/TechRiders.Api/Program.cs`
- Config local canónica: `backend/TechRiders.Api/appsettings.Development.json`
- Registro de infraestructura: `backend/TechRiders.Infrastructure/Extensions/InfrastructureServiceExtensions.cs`
- Script de reverse engineering: `backend/scaffold-db-first.ps1`

---
Generado: 2026-07-24
Status: Database-first alineado con la BD local existente
