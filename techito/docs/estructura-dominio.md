# Estructura de Dominio — TechRiders

## Arquitectura: Monolito Modular por Dominios

```
src/app/
├── core/                  ← Singleton: auth, layout, config
│   ├── auth/              (guards, interceptors, auth.service)
│   ├── layout/            (header, footer)
│   └── services/          (config-loader, api-base)
│
├── shared/                ← Reutilizable sin lógica de negocio
│   ├── ui/                (botones, cards, modals, toasters)
│   ├── models/            (interfaces base compartidas)
│   └── pipes/             (formateo, fechas, etc.)
│
└── features/              ← Un módulo lazy-loaded por dominio
    ├── comunidad/         → home, quienes-somos, orienta-tech
    ├── contenido/         → conocimiento (tutoriales, vídeos, podcast, eventos)
    ├── captacion/         → candidato, solicita
    ├── contacto/          → formulario contacto + sugerencias
    ├── auth/              → login, perfil-usuario
    ├── sesiones/          → gestión de charlas (intranet)
    ├── embajadores/       → gestión de voluntarios (intranet)
    ├── empleo/            → empresa/ + junior/ (intranet)
    └── admin/             → dashboard, staff, colaboradores (intranet)
```

## Bounded Contexts (Dominios)

### Core Domains (valor diferencial)
| Dominio | Entidades principales | Rutas |
|---------|----------------------|-------|
| Sesiones | Sesión, Centro, Categoría, Asignación | `/intranet/sessions/*` |
| Embajadores | Embajador, Valoración, Historial | `/intranet/administration/ambassadors/*` |
| Empleo | Oferta, Candidatura, Empresa, Junior | `/intranet/company/*`, `/intranet/junior/*` |

### Supporting Domains
| Dominio | Entidades principales | Rutas |
|---------|----------------------|-------|
| Contenido | Tutorial, Evento, Vídeo, Episodio | `/conocimiento` |
| Identidad | Usuario, Rol, Sesión Auth | `/login`, `/perfil` |
| Comunidad | Página estática, FAQ, Staff | `/`, `/about-us`, `/orienta-tech` |

### Generic Domains
| Dominio | Responsabilidad |
|---------|----------------|
| Shell (core/layout) | Header, Footer, navegación |
| Shared | UI components, pipes, modelos base |

## Roles del sistema

| Rol | Acceso |
|-----|--------|
| **Público** | comunidad, contenido, captación, contacto |
| **Junior** | + dashboard junior, ofertas, cursos, perfil |
| **Empresa** | + dashboard empresa, gestionar ofertas, ver candidatos |
| **Admin** | + dashboard admin, staff, embajadores, colaboradores, sesiones |

## Archivos de infraestructura

- Bootstrap: `main.ts`, `main.server.ts`, `server.ts`
- Config: `app.config.ts`, `app.config.server.ts`
- Rutas: `app.routes.ts` (lazy loading por feature)
- Estilos: `design-tokens.scss` → `styles.scss`
- Contenido: `content-source/` (Markdown + config.js)
- Deploy: `staticwebapp.config.json`
