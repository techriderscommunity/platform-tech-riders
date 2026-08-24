# Code-First Setup - TechRiders Backend

## Diagnóstico

El backend canónico de la solución es `TechRiders.Api`.

El flujo correcto del proyecto es `code-first` con Entity Framework Core: el modelo C# es la fuente de verdad y la base de datos se materializa mediante migraciones.

## Contrato obligatorio

1. El modelo de dominio en C# define el esquema.
2. Los cambios de base de datos se realizan con `dotnet ef migrations add` y `dotnet ef database update`.
3. La base de datos local se crea o sincroniza desde las migraciones, no desde un reverse engineering.
4. El startup project para ejecución es `TechRiders.Api`.

## Base local activa

```text
Server=(localdb)\MSSQLLocalDB
Database=TechRidersDev
```

## Flujo code-first

### 1. Ejecutar la API canónica

```powershell
cd .\backend\TechRiders.Api
dotnet run
```

### 2. Revisar y aplicar migraciones

Desde la raíz del repositorio:

```powershell
cd .\backend

dotnet ef migrations add <NombreCambio> --project .\TechRiders.Infrastructure\TechRiders.Infrastructure.csproj --startup-project .\TechRiders.Api\TechRiders.Api.csproj
dotnet ef database update --project .\TechRiders.Infrastructure\TechRiders.Infrastructure.csproj --startup-project .\TechRiders.Api\TechRiders.Api.csproj
```

## Qué no hacer

- No usar reverse engineering desde la base de datos como flujo normal.
- No mantener documentación ni scripts de database-first en el backend ni en el flujo operativo.
- No crear snapshots generados por scaffolding del modelo como fuente de verdad.

## Verificación rápida

### Comprobar que LocalDB está disponible

```powershell
sqllocaldb info
```

### Comprobar que la BD existe

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT name FROM sys.databases ORDER BY name"
```

### Comprobar salud de la API

```text
GET /health
```

## Referencias

- Startup canónico: `backend/TechRiders.Api/Program.cs`
- Config local canónica: `backend/TechRiders.Api/appsettings.Development.json`
- DbContext: `backend/TechRiders.Infrastructure/Data/TechRidersDbContext.cs`
- Script de soporte: `backend/scaffold-code-first.ps1`

---
Generado: 2026-08-20
Status: Code-first activo y único flujo de persistencia
