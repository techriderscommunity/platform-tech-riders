# Checklist MVP - Cierre previo a publicación (2026-08-08)

## Objetivo

Checklist cerrada, requisito por requisito, para validar el MVP de intranet antes de publicar.

## Estado por requisito

| ID | Requisito | Estado | Evidencia | Gap / Riesgo |
| --- | --- | --- | --- | --- |
| MVP-AUTH-01 | Login local coherente sin depender de estado manual del navegador | CERRADO | Endpoint local `POST /api/auth/login` y emisión de JWT local con roles; fallback de usuarios de desarrollo y configuración en `appsettings.Development.json`. | Riesgo bajo: credenciales demo para desarrollo, no aptas para producción. |
| MVP-AUTH-02 | Backend acepta token local en rutas protegidas | CERRADO | Pipeline de auth híbrido en `Program.cs` con `PolicyScheme` (`AzureAd`/`LocalJwt`) y validación de issuer/audience/firmado. | Riesgo medio: coexistencia Azure AD + local requiere pruebas de regresión de tokens AAD reales. |
| MVP-SESS-01 | Workflow de sesiones gestionado en backend (no solo overrides MVP en cliente) | CERRADO | Nuevos endpoints `GET /api/sessions/workflow` y `PUT /api/sessions/{id}/workflow`; estado compartido central en store backend. | Riesgo bajo: store actual es en memoria (reinicio borra estado). |
| MVP-SESS-02 | Frontend de sesiones deja de depender de localStorage para overrides operativos | CERRADO | `sesiones.ts` elimina persistencia local y actualiza estado vía backend (`SesionesService.updateSessionWorkflow`). | Riesgo bajo: requiere backend disponible para cambios de estado. |
| MVP-SESS-03 | Compatibilidad temporal del endpoint legacy de session-actions | CERRADO | `IntranetController` mantiene endpoint, pero ahora usa clave global backend compartida. | Riesgo bajo: contrato legacy sigue presente. |
| MVP-VAL-01 | Build backend verde tras cambios | CERRADO | `dotnet build backend/TechRiders.API/TechRiders.Api.csproj` exitoso. | Sin gap técnico detectado. |
| MVP-VAL-02 | Build frontend verde tras cambios | CERRADO | `npm install --prefix techito` + `npm run --prefix techito build` exitoso. | Se reportan vulnerabilidades NPM pendientes de remediación. |
| MVP-SEC-01 | Login local limitado a desarrollo | CERRADO | `AuthController` devuelve 404 fuera de `Development` o si `LocalAuth:Enabled=false`. | Riesgo bajo. |
| MVP-SEC-02 | Contraseñas robustas y secreto JWT no hardcodeado para producción | CERRADO | Se retiraron `JwtKey` y `Users` de `appsettings.Development.json`; ahora se exigen `JWT_KEY` y `LOCAL_AUTH_USERS_JSON` vía entorno local. | Queda recomendación de rotación/secret store para entorno compartido. |
| MVP-GOV-01 | Trazabilidad boost-first y fallback de motor | CERRADO | Registro actualizado en `boost_trace_2026-08-07.md` (bloque 2026-08-08). | Riesgo bajo: GitNexus impact no operativo (storage version mismatch). |

## Resultado de readiness

- Requisitos cerrados: 10
- Requisitos abiertos: 0
- Readiness estimada para publicar MVP intranet local: LISTO (con hardening recomendado post-publicación).

## Hardening Recomendado

1. Ejecutar smoke E2E de login + navegación intranet por rol (admin, staff, embajador, junior).
2. Reducir vulnerabilidades reportadas por `npm audit` en `techito` antes de release pública.
3. Planificar persistencia duradera del workflow de sesiones (actualmente en memoria del backend).

## Estado de vulnerabilidades NPM (2026-08-08)

- Estado inicial tras instalación: 33 (2 low, 10 moderate, 21 high).
- Estado tras `npm audit fix`: 22 (1 low, 6 moderate, 15 high).
- Estado final tras upgrades de patch en Angular 20.x + DOMPurify: 10 (7 moderate, 3 high).

Vulnerabilidades residuales bloqueadas por salto mayor (`npm audit fix --force`):

- `@hono/node-server` (transitiva vía `@modelcontextprotocol/sdk` de `@angular/cli`) - requiere mover CLI/tooling a rama mayor.
- `image-size` / `less` / `webpack-dev-server` / `uuid` - requieren actualizar `@angular-devkit/build-angular` a `22.1.3` (breaking).

## Rutas clave afectadas

- `POST /api/auth/login`
- `GET /api/sessions/workflow`
- `PUT /api/sessions/{id}/workflow`
- `GET/PUT /api/intranet/session-actions` (compatibilidad)
