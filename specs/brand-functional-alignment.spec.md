# Brand and Functional Alignment Spec

## Purpose
Define the implementation contract to deliver the full Tech Riders functional scope with modern Angular + C# Database First on Azure, without losing Tech Riders brand identity.

## Official Sources
- Functional scope: `autodocs/assessments/Funcional_Completo_Plataforma_Web_Tech_Riders.md`
- UX/UI scope: `autodocs/assessments/UXUI_Functional_Design_System_Tech_Riders.md`
- Brand core: `autodocs/branding/Brand Core.md`
- Badge catalog (future awards): `autodocs/branding/badges.md`

## Binding Decisions
1. Backend stack: C# with .NET 10, Database First against Azure SQL.
2. Frontend stack: Angular 20 modern architecture (standalone, lazy routes, signals).
3. Cloud runtime: Azure App Service.
4. Infrastructure as code: Bicep.
5. Badges are deferred to post-MVP and are not a blocker for the core functional rollout.

## Functional to Technical Mapping
| Functional Area | Primary Tech Layer | Mandatory Output |
|---|---|---|
| Public portal | Angular app | Public pages and discovery flows |
| Intranet and member operations | Angular + API | Authenticated role-based flows |
| Sessions/events lifecycle | API + Angular | End-to-end CRUD and scheduling |
| Community and ambassador operations | API + Angular | Membership and participation flows |
| Admin and reporting | API + Angular + telemetry | Dashboards and operational reporting |
| Security, RGPD and audit | API + Azure services | Auditable events and access control |

## Branding Guardrails
1. Keep brand tokens as source of truth in `techito/src/design-tokens.scss`.
2. Do not introduce hardcoded colors where a design token exists.
3. Keep logo and Tech Riders narrative aligned with Brand Core.
4. Do not couple MVP delivery to badges behavior; badges remain a future award layer.

## Delivery Phases
1. Source normalization and assessment manifest.
2. Backend Database First hardening for Azure SQL.
3. Frontend functional coverage and environment readiness.
4. Azure IaC and deployment pipelines.
5. Observability, security, and production readiness validation.
6. Post-MVP badge awards rollout.

## Verification Criteria
1. Every high-level functional area maps to concrete backend and frontend deliverables.
2. Database First pipeline is reproducible and Azure SQL compatible.
3. Angular application resolves all core routes and authenticated workflows.
4. Azure deployment is reproducible with Bicep and environment segregation.
5. Branding is preserved across UI surfaces while badges remain non-blocking.
