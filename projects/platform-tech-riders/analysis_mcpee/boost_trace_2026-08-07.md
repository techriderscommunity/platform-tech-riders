# Boost-First Traceability - 2026-08-07

## Contexto

- Proyecto: platform-tech-riders
- Objetivo actual: implementación funcional completa con Angular + C# DB-first + Azure, manteniendo branding y habilitando trabajo sin acceso a Azure SQL.

## Regla Operativa Aplicada

- No se usan todos los boosts a la vez.
- Se selecciona 1 boost principal por bloque de trabajo y, solo si hace falta, 1 complemento.
- Cada bloque deja evidencia de salida técnica.

## Bloques ejecutados y boost usado

### Bloque A - Descubrimiento funcional + branding

- Boost principal: `mcpee-frontend` (arquitectura funcional/UI)
- Complemento: `mcpee-uxdesign` (consistencia de identidad y tokens)
- Motor usado: Explore (read-only)
- Evidencia:
  - hallazgos en `/memories/session/techriders-exploration-findings.md`
  - plan en `/memories/session/plan.md`

### Bloque B - Estrategia backend DB-first sin acceso BBDD

- Boost principal: `mcpee-backend`
- Complemento: `mcpee-cloud` (alineación futura con Azure SQL)
- Motor usado: CodeGraph + edición directa
- Cambios técnicos:
  - `backend/TechRiders.Infrastructure/Extensions/InfrastructureServiceExtensions.cs`
  - `backend/TechRiders.Infrastructure/TechRiders.Infrastructure.csproj`
  - `backend/TechRiders.API/Program.cs`
  - `backend/TechRiders.API/appsettings.Development.json`
- Resultado:
  - fallback `Database:UseInMemory` habilitado para continuar desarrollo sin acceso a SQL.

### Bloque C - Runtime local sin Docker

- Boost principal: `mcpee-cloud` (container/runtime local policy)
- Complemento: `mcpee-backend` (compatibilidad API)
- Motor usado: edición + validación terminal
- Cambios técnicos:
  - `backend/TechRiders.API/Containerfile`
  - `techito/Containerfile`
  - `scripts/setup/podman-up.ps1`
  - `scripts/setup/podman-down.ps1`
  - `scripts/setup/PODMAN_LOCAL_RUN.md`
- Resultado:
  - ejecución local migrada a Podman-only.

## Gap detectado

- El índice GitNexus no estaba operativo para `impact()` (versión de storage incompatible). Se aplicó fallback con CodeGraph/lectura directa y se registró riesgo como `UNKNOWN` para ese control puntual.

## Criterio para próximos bloques

1. Azure IaC (Bicep): boost principal `mcpee-cloud`.
2. Endpoints y servicios de negocio: boost principal `mcpee-backend`.
3. Implementación de vistas Angular funcionales: boost principal `mcpee-frontend` + complemento `mcpee-uxdesign`.
4. QA y hardening: combinación controlada `mcpee-backend` + `mcpee-cloud` por capa.

## Bloque D - Cierre auth local + workflow de sesiones (2026-08-08)

- Boost principal: `mcpee-backend`
- Complemento: `mcpee-frontend`
- Motor usado: CodeGraph + edición directa
- Fallback:
  - GitNexus `impact()` no operativo por incompatibilidad de storage version (`lbug` versión 42 vs engine 41).
  - Se aplicó fallback con exploración estructural en CodeGraph y validación compilable.
- Cambios técnicos:
  - `backend/TechRiders.API/Program.cs`
  - `backend/TechRiders.API/Controllers/AuthController.cs`
  - `backend/TechRiders.API/Services/LocalAuthOptions.cs`
  - `backend/TechRiders.API/Controllers/SessionsController.cs`
  - `backend/TechRiders.API/Controllers/IntranetController.cs`
  - `backend/TechRiders.API/appsettings.Development.json`
  - `techito/src/app/features/intranet/fp-tour/services/sesiones.service.ts`
  - `techito/src/app/features/intranet/fp-tour/sesiones.ts`
  - `projects/platform-tech-riders/analysis_mcpee/mvp-checklist-2026-08-08.md`
- Validación:
  - backend OK: `dotnet build backend/TechRiders.API/TechRiders.Api.csproj`
  - frontend pendiente: `npm run --prefix techito build` falla por `ng` no disponible en entorno actual.

## Bloque E - Cierre de gaps MVP (2026-08-08)

- Boost principal: `mcpee-backend`
- Complemento: `mcpee-frontend`
- Motor usado: edición directa + validación terminal
- Cambios técnicos:
  - `backend/TechRiders.API/Controllers/AuthController.cs` (sin fallback de credenciales hardcodeadas)
  - `backend/TechRiders.API/Program.cs` (JWT local obligatorio por configuración)
  - `backend/TechRiders.API/appsettings.Development.json` (sin secretos/usuarios versionados)
  - `.env` (variables locales `JWT_KEY` y `LOCAL_AUTH_USERS_JSON`)
  - `projects/platform-tech-riders/analysis_mcpee/mvp-checklist-2026-08-08.md` (estado final cerrado)
- Validación:
  - frontend OK: `npm install --prefix techito` y `npm run --prefix techito build`.
  - backend OK: `dotnet build backend/TechRiders.API/TechRiders.Api.csproj`.

## Bloque F - Remediación de vulnerabilidades npm (2026-08-08)

- Boost principal: `mcpee-frontend`
- Complemento: `mcpee-backend`
- Motor usado: edición de dependencias + validación terminal
- Cambios técnicos:
  - `techito/package.json`
  - `techito/package-lock.json`
  - upgrades de patch Angular `20.3.27/20.3.33` y `dompurify@3.4.13`
- Validación:
  - build OK: `npm run --prefix techito build`.
  - auditoría: `npm audit --prefix techito` baja de `33` a `10` vulnerabilidades.
  - residual: 10 vulnerabilidades con fix condicionado a salto mayor (Angular toolchain 22.x).

## Nota

Este archivo existe para que la trazabilidad boost-first sea auditable y visible en cada iteración.
