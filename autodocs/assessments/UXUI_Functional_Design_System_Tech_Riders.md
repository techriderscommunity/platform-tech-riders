# UX/UI Functional Design System COMPLETO - Tech Riders

**Producto:** Plataforma Web Tech Riders  
**Tipo de documento:** UX/UI Functional Design System + Frontend Architecture Handbook  
**Stack objetivo:** Angular LTS · Standalone Components · Signals · Nx Monorepo · SCSS · Design Tokens  
**Versión:** 1.0 COMPLETA  
**Fecha:** 25/07/2026  
**Estado:** Documento maestro para agentes frontend, diseño UX/UI, arquitectura visual y composición de vistas  

---

## Índice

1. [Propósito del documento](#1-propósito-del-documento)
2. [Cómo deben usar este documento los agentes](#2-cómo-deben-usar-este-documento-los-agentes)
3. [Visión de producto Tech Riders](#3-visión-de-producto-tech-riders)
4. [Modelo conceptual de comunidad](#4-modelo-conceptual-de-comunidad)
5. [Principios UX/UI](#5-principios-uxui)
6. [Identidad visual oficial](#6-identidad-visual-oficial)
7. [Extracción de patrones visuales](#7-extracción-de-patrones-visuales)
8. [Arquitectura visual del sistema](#8-arquitectura-visual-del-sistema)
9. [Design Tokens](#9-design-tokens)
10. [Tokens CSS base](#10-tokens-css-base)
11. [Arquitectura Nx recomendada](#11-arquitectura-nx-recomendada)
12. [Reglas de boundaries Nx](#12-reglas-de-boundaries-nx)
13. [Capas frontend](#13-capas-frontend)
14. [Inventario completo de componentes](#14-inventario-completo-de-componentes)
15. [UI Primitives](#15-ui-primitives)
16. [UI Layout Components](#16-ui-layout-components)
17. [UI Patterns](#17-ui-patterns)
18. [Componentes de producto Tech Riders](#18-componentes-de-producto-tech-riders)
19. [Componentes de formularios](#19-componentes-de-formularios)
20. [Componentes de feedback y estado](#20-componentes-de-feedback-y-estado)
21. [Componentes de datos y administración](#21-componentes-de-datos-y-administración)
22. [Modelo de composición](#22-modelo-de-composición)
23. [Blueprints de vistas públicas](#23-blueprints-de-vistas-públicas)
24. [Blueprints de intranet](#24-blueprints-de-intranet)
25. [Blueprints de administración](#25-blueprints-de-administración)
26. [Sistema de navegación](#26-sistema-de-navegación)
27. [Sistema de cards](#27-sistema-de-cards)
28. [Sistema de perfiles](#28-sistema-de-perfiles)
29. [Sistema de sesiones](#29-sistema-de-sesiones)
30. [Sistema de contenidos](#30-sistema-de-contenidos)
31. [Sistema de métricas](#31-sistema-de-métricas)
32. [Sistema de filtros](#32-sistema-de-filtros)
33. [Sistema responsive](#33-sistema-responsive)
34. [Accesibilidad](#34-accesibilidad)
35. [Estados obligatorios](#35-estados-obligatorios)
36. [Microcopy y tono](#36-microcopy-y-tono)
37. [Reglas Angular](#37-reglas-angular)
38. [Patrones con Signals](#38-patrones-con-signals)
39. [Ejemplos de componentes Angular](#39-ejemplos-de-componentes-angular)
40. [Estructura de carpetas por componente](#40-estructura-de-carpetas-por-componente)
41. [Contratos de ViewModel](#41-contratos-de-viewmodel)
42. [Naming conventions](#42-naming-conventions)
43. [Anti-patrones prohibidos](#43-anti-patrones-prohibidos)
44. [Checklist para agentes](#44-checklist-para-agentes)
45. [Definition of Ready UX/UI](#45-definition-of-ready-uxui)
46. [Definition of Done UX/UI](#46-definition-of-done-uxui)
47. [Checklist de Pull Request](#47-checklist-de-pull-request)
48. [Roadmap de implementación del Design System](#48-roadmap-de-implementación-del-design-system)
49. [Backlog inicial de componentes](#49-backlog-inicial-de-componentes)
50. [Prompt maestro para agentes frontend](#50-prompt-maestro-para-agentes-frontend)
51. [Resumen ejecutivo para pegar en el repo](#51-resumen-ejecutivo-para-pegar-en-el-repo)

---

# 1. Propósito del documento

Este documento define el **sistema oficial UX/UI y frontend** para la Plataforma Web Tech Riders.

Su objetivo es evitar que cada agente, persona desarrolladora o vista cree su propio criterio visual, su propia estructura de componentes o su propia interpretación del producto.

Este documento debe actuar como:

- Fuente de verdad visual.
- Fuente de verdad de composición UX.
- Fuente de verdad para frontend Angular.
- Guía para agentes de generación de código.
- Contrato de calidad para pull requests.
- Base para construir el monorepo Nx.
- Sistema de diseño funcional y no solo estético.

## 1.1 Problema que resuelve

Sin un documento de este tipo, el riesgo es acabar con:

- Botones repetidos.
- Cards distintas en cada pantalla.
- Paletas de color inconsistentes.
- Formularios con UX diferente.
- Layouts imposibles de mantener.
- Componentes acoplados al dominio.
- Vistas que no reutilizan nada.
- Agentes generando código bonito pero incoherente.

## 1.2 Decisión principal

Las capturas e imágenes de referencia **no se entregan a los agentes como fuente directa de implementación**.

Las imágenes se usan únicamente para extraer:

- Intención visual.
- Patrones de layout.
- Jerarquía de información.
- Estilo de componentes.
- Comportamiento esperado de navegación.
- Distribución de dashboard, catálogo y home.

Los agentes deben trabajar con este documento y con los tokens/componentes definidos aquí.

---

# 2. Cómo deben usar este documento los agentes

Todo agente frontend debe leer este documento antes de generar código.

## 2.1 Orden obligatorio de lectura

1. Visión y principios de producto.
2. Identidad visual oficial.
3. Design tokens.
4. Arquitectura Nx.
5. Inventario de componentes.
6. Blueprints de vistas.
7. Reglas Angular.
8. Anti-patrones.
9. Checklist de PR.
10. Prompt maestro.

## 2.2 Regla crítica

Antes de crear cualquier componente nuevo, el agente debe responder internamente:

```text
¿Esto ya existe como primitive, pattern o componente de producto?
```

Si la respuesta es sí, debe reutilizarlo.

Si la respuesta es no, debe crearlo en la librería correcta, no dentro de una página.

## 2.3 Ninguna vista debe inventar sistema visual

Una página puede decidir composición y orden, pero no puede inventar:

- Botones.
- Cards.
- Badges.
- Espaciados.
- Sombras.
- Colores.
- Estados.
- Inputs.
- Layouts base.

---

# 3. Visión de producto Tech Riders

## 3.1 Qué es Tech Riders

Tech Riders es una comunidad tecnológica que conecta personas, conocimiento, sesiones, centros, empresas, oportunidades y experiencias reales del sector tech.

Tech Riders no se limita a una web informativa. La plataforma debe actuar como el **sistema operativo digital de la comunidad**.

## 3.2 Qué conecta la plataforma

- Miembros Tech Riders.
- Staff.
- Community Leaders.
- Ambassadors.
- Estudiantes.
- Profesores.
- Profesionales junior.
- Profesionales senior.
- Empresas.
- Centros formativos.
- Orientadores.
- Sesiones.
- Eventos.
- Formación.
- Banco de conocimiento.
- Recursos.
- Oportunidades.

## 3.3 Qué NO es la plataforma

La plataforma Tech Riders no debe sentirse como:

- Una web corporativa fría.
- Un LMS tradicional.
- Un portal institucional antiguo.
- Una intranet administrativa sin alma.
- Un simple directorio de speakers.
- Un catálogo de tutoriales.
- Un repositorio de enlaces.

## 3.4 Qué debe transmitir

- Comunidad.
- Movimiento.
- Tecnología real.
- Futuro.
- Aprendizaje práctico.
- Cercanía.
- Energía.
- Profesionalidad.
- Generosidad.
- Colaboración.

---

# 4. Modelo conceptual de comunidad

## 4.1 Tech Riders

Tech Riders es la comunidad completa.

No debe usarse “Tech Rider” como sinónimo de speaker.

Un Tech Rider es cualquier persona que se une a la comunidad.

## 4.2 Roles de comunidad

| Rol | Descripción | Implicación |
|---|---|---|
| Miembro | Persona que se une a Tech Riders | Base |
| Staff | Personas que coordinan, gobiernan y dan dirección | Alta |
| Community Leaders | Miembros que ayudan a construir e impulsar iniciativas | Media/Alta |
| Ambassador | Personas activas que participan en actividades y extienden comunidad | Alta |

## 4.3 Perfiles funcionales

Los roles de comunidad no sustituyen a los perfiles funcionales.

Una persona puede ser:

- Estudiante + Miembro.
- Profesor + Community Leaders.
- Profesional Senior + Ambassador.
- Staff + Ambassador.
- Empresa + Community Leaders.

## 4.4 Regla de producto

Separar siempre:

```text
Quién es la persona
Cómo participa en Tech Riders
Qué permisos tiene
```

---

# 5. Principios UX/UI

## 5.1 Community First

La comunidad es el centro.

Las personas, perfiles, sesiones y actividades deben estar más presentes que los bloques institucionales.

## 5.2 Content First

El contenido debe descubrirse rápido.

La plataforma debe facilitar encontrar:

- Sesiones.
- Eventos.
- Recursos.
- Tutoriales.
- Perfiles.
- Ambassadors.
- Oportunidades.

## 5.3 Role Aware UX

La experiencia debe adaptarse al rol activo:

- Miembro.
- Staff.
- Community Leaders.
- Ambassador.
- Profesor.
- Empresa.
- Centro.
- Orientador.

## 5.4 Composition First

Las páginas se construyen componiendo bloques existentes.

No se diseñan pantallas aisladas.

## 5.5 Token First

Todo valor visual debe venir de tokens.

## 5.6 Accessible by Default

Todos los componentes deben ser accesibles desde su diseño inicial.

## 5.7 Dark First

La identidad visual principal es dark-first.

El modo light no es la norma. Solo puede existir como variante de superficie si el producto lo exige, y siempre adaptado a la identidad Tech Riders.

---

# 6. Identidad visual oficial

## 6.1 Estética principal

```text
Dark-first neon-tech community UI
```

## 6.2 Rasgos visuales

- Fondo azul noche casi negro.
- Superficies elevadas en azul oscuro.
- Bordes cyan semitransparentes.
- Gradientes sutiles azul/cyan/violeta.
- Glow controlado.
- Iconografía lineal.
- Cards modulares.
- Dashboard denso pero ordenado.
- CTA claros y reconocibles.

## 6.3 Qué evitar

- Cyberpunk excesivo.
- Brillos saturados por todas partes.
- Estética gaming.
- Neones sin jerarquía.
- Gradientes agresivos.
- Fondos claros genéricos.
- Cards blancas sin adaptación.
- UI tipo SaaS corporativo sin personalidad.

## 6.4 Paleta conceptual

| Concepto | Color dominante |
|---|---|
| Acción primaria | Cyan / Blue |
| Comunidad | Violet |
| Profesor / conocimiento | Purple |
| Ambassador / confirmado | Green |
| Empresa / oportunidad | Amber |
| Women in Tech / orientación | Pink |
| Fondo | Ink / Navy |
| Panel | Dark Blue |

---

# 7. Extracción de patrones visuales

## 7.1 Home pública

La home pública debe usar:

- Topbar horizontal.
- Logo visible.
- Métricas destacadas.
- Grid de perfiles/audiencias.
- Cards con icono, título, texto y CTA.
- CTA final de comunidad.
- Fondo dark con elementos gráficos sutiles.

## 7.2 Landing de audiencia

Las páginas del tipo “Soy Profesor Tech” deben usar:

- Hero grande.
- Título con acento de color.
- Descripción clara.
- CTA principal y secundario.
- Panel lateral de beneficios.
- Cards por bloque funcional.
- CTA inferior de contacto/comunidad.

## 7.3 Dashboard/intranet

La intranet debe usar:

- Sidebar lateral.
- Topbar con buscador y acciones.
- Selector de rol activo.
- Hero contextual.
- Widgets en grid.
- Tablas compactas.
- Activity feed.
- Cards de perfil y preferencias.

## 7.4 Catálogo/tutoriales

La pantalla de tutoriales aporta layout, no estética.

Se debe conservar:

- Filtros laterales.
- Grid de tarjetas.
- Chips de filtros activos.
- Paginación.
- Cards de catálogo.

Pero debe adaptarse a identidad dark:

- Fondo dark.
- Cards dark glass.
- Bordes cyan.
- CTA cyan/blue/violet.
- Filtros con panel oscuro.

---

# 8. Arquitectura visual del sistema

```text
Design Tokens
  ↓
CSS Variables / SCSS Helpers
  ↓
UI Primitives
  ↓
UI Layout Components
  ↓
UI Patterns
  ↓
Domain UI Components
  ↓
Feature Components
  ↓
Pages
```

## 8.1 Responsabilidad por capa

| Capa | Responsabilidad |
|---|---|
| Tokens | Valores visuales base |
| CSS Variables | Exposición técnica de tokens |
| Primitives | Piezas mínimas reutilizables |
| Layout Components | Shells, grids, containers |
| Patterns | Composiciones comunes |
| Domain UI | Componentes específicos de Tech Riders |
| Features | Orquestación de caso de uso |
| Pages | Composición final |

## 8.2 Regla clave

Las páginas solo importan features, patterns y layout.

Las páginas no crean visuales propios.

---

# 9. Design Tokens

## 9.1 Categorías de tokens

- Color.
- Surface.
- Text.
- Border.
- State.
- Role.
- Spacing.
- Radius.
- Shadow.
- Gradient.
- Typography.
- Motion.
- Layout.
- Component.

## 9.2 Niveles de tokens

### Primitive tokens

Valores base:

```text
blue-500
cyan-400
space-4
radius-xl
```

### Semantic tokens

Valores con intención:

```text
color-background-page
color-text-primary
color-action-primary
```

### Component tokens

Valores asociados a componentes:

```text
button-height-md
card-padding
session-card-radius
```

## 9.3 Regla para añadir tokens

Solo se añade un token nuevo si:

- Se reutilizará en más de un componente.
- Tiene significado semántico.
- No existe ya un token equivalente.
- Mejora la coherencia del sistema.

No se añaden tokens para resolver una pantalla aislada.

---

# 10. Tokens CSS base

```css
:root {
  --tr-color-brand-ink: #020817;
  --tr-color-brand-ink-raised: #061226;
  --tr-color-brand-ink-soft: #0B1730;

  --tr-color-brand-cyan: #00D7FF;
  --tr-color-brand-blue: #0A84FF;
  --tr-color-brand-violet: #8B5CF6;
  --tr-color-brand-purple: #A855F7;
  --tr-color-brand-green: #39D353;
  --tr-color-brand-amber: #F59E0B;
  --tr-color-brand-pink: #EC4899;

  --tr-color-surface-page: #020817;
  --tr-color-surface-shell: #030B1C;
  --tr-color-surface-panel: #071426;
  --tr-color-surface-panel-alt: #0A1930;
  --tr-color-surface-panel-elevated: #0B1D35;

  --tr-color-text-primary: #F8FAFC;
  --tr-color-text-secondary: #CBD5E1;
  --tr-color-text-muted: #94A3B8;
  --tr-color-text-inverse: #071426;

  --tr-color-border-subtle: rgba(0, 215, 255, 0.12);
  --tr-color-border-default: rgba(0, 215, 255, 0.22);
  --tr-color-border-strong: rgba(0, 215, 255, 0.42);

  --tr-space-0: 0;
  --tr-space-1: 0.25rem;
  --tr-space-2: 0.5rem;
  --tr-space-3: 0.75rem;
  --tr-space-4: 1rem;
  --tr-space-5: 1.25rem;
  --tr-space-6: 1.5rem;
  --tr-space-8: 2rem;
  --tr-space-10: 2.5rem;
  --tr-space-12: 3rem;
  --tr-space-16: 4rem;
  --tr-space-20: 5rem;
  --tr-space-24: 6rem;

  --tr-radius-sm: 0.375rem;
  --tr-radius-md: 0.5rem;
  --tr-radius-lg: 0.75rem;
  --tr-radius-xl: 1rem;
  --tr-radius-2xl: 1.25rem;
  --tr-radius-3xl: 1.5rem;
  --tr-radius-full: 9999px;

  --tr-shadow-card: 0 16px 40px -20px rgba(0, 0, 0, 0.65);
  --tr-shadow-glow-cyan: 0 0 24px rgba(0, 215, 255, 0.25);
  --tr-shadow-glow-violet: 0 0 28px rgba(139, 92, 246, 0.28);
  --tr-shadow-focus: 0 0 0 4px rgba(0, 215, 255, 0.22);

  --tr-gradient-hero: linear-gradient(135deg, rgba(8, 31, 84, 0.92) 0%, rgba(33, 15, 82, 0.82) 52%, rgba(0, 150, 190, 0.34) 100%);
  --tr-gradient-cta: linear-gradient(90deg, #0A84FF 0%, #00D7FF 48%, #A855F7 100%);

  --tr-font-family-sans: Inter, Segoe UI, system-ui, sans-serif;
}
```

---

# 11. Arquitectura Nx recomendada

```text
apps/
  tech-riders-web/
  tech-riders-admin/

libs/
  design/
    tokens/
    theme/
    styles/

  shared/
    ui-primitives/
    ui-layout/
    ui-forms/
    ui-feedback/
    ui-data-display/
    util-a11y/
    util-formatters/
    util-testing/

  community/
    domain/
    data-access/
    ui-profile/
    ui-role-badge/
    feature-members/
    feature-ambassadors/
    feature-community-home/

  sessions/
    domain/
    data-access/
    ui-session-card/
    ui-session-status/
    feature-session-list/
    feature-session-detail/
    feature-session-request/
    feature-session-admin/

  activities/
    domain/
    data-access/
    ui-activity-card/
    feature-calendar/
    feature-activity-detail/

  content/
    domain/
    data-access/
    ui-knowledge-card/
    feature-knowledge-base/
    feature-tutorials-catalog/

  audiences/
    domain/
    ui-audience-card/
    feature-teachers/
    feature-students/
    feature-professionals/
    feature-companies/
    feature-orientators/
    feature-women-tech/
    feature-starters/

  admin/
    domain/
    data-access/
    feature-dashboard/
    feature-members-admin/
    feature-sessions-admin/
    feature-content-admin/
    feature-settings/
```

---

# 12. Reglas de boundaries Nx

## 12.1 Reglas generales

```text
apps -> puede importar feature libs
feature -> puede importar domain, data-access, ui, shared
ui -> puede importar shared/ui-primitives y design tokens
domain -> no importa Angular UI
data-access -> no importa componentes UI
design -> no importa nada
shared/ui-primitives -> no importa dominios
```

## 12.2 Reglas prohibidas

```text
community/feature-members -> sessions/feature-session-admin ❌
shared/ui-primitives -> community/domain ❌
design/tokens -> shared/ui-primitives ❌
page -> componente visual local duplicado ❌
```

## 12.3 Reglas recomendadas de tags Nx

```json
{
  "scope:shared": "shared reusable libraries",
  "scope:design": "design system foundation",
  "scope:community": "community domain",
  "scope:sessions": "sessions domain",
  "scope:content": "content domain",
  "scope:admin": "admin domain",
  "type:ui": "presentational UI",
  "type:feature": "feature orchestration",
  "type:domain": "domain models",
  "type:data-access": "API/state access",
  "type:util": "utilities"
}
```

---

# 13. Capas frontend

## 13.1 Domain

Define modelos y tipos:

- Member.
- CommunityRole.
- AmbassadorProfile.
- Session.
- Activity.
- ContentItem.
- Audience.

No contiene UI.

## 13.2 Data Access

Gestiona:

- HTTP.
- Adaptadores.
- Stores basados en signals.
- Mappers.
- Repositorios frontend.

## 13.3 UI

Componentes presentacionales reutilizables.

No hacen llamadas HTTP.

## 13.4 Feature

Orquesta:

- Estado.
- Carga de datos.
- Navegación.
- Formularios.
- Composición de UI.

## 13.5 App

Define shell, router y composición principal.

---

# 14. Inventario completo de componentes

## 14.1 P0 Foundation

- Button.
- IconButton.
- Link.
- Card.
- Badge.
- Tag.
- Avatar.
- AvatarGroup.
- Input.
- SearchBox.
- Select.
- Textarea.
- Checkbox.
- Radio.
- Switch.
- FormField.
- Spinner.
- Skeleton.
- EmptyState.
- Toast.
- Modal.
- Drawer.
- Tooltip.
- Tabs.
- Dropdown.

## 14.2 P1 Layout

- PublicShell.
- AppShell.
- AdminShell.
- Topbar.
- SidebarNav.
- PageContainer.
- Section.
- PageHeader.
- SectionHeader.
- Grid.
- Stack.
- SplitLayout.
- WidgetGrid.

## 14.3 P2 Patterns

- HeroCommunity.
- PageHero.
- MetricsStrip.
- CTASection.
- FeatureCardGrid.
- FilterPanel.
- FilterGroup.
- FilterChip.
- ActivityFeed.
- DataTable.
- Timeline.
- Stepper.
- Breadcrumbs.

## 14.4 P3 Product Components

- CommunityRoleBadge.
- MemberProfileCard.
- AmbassadorProfileCard.
- StaffProfileCard.
- CollaboratorProfileCard.
- SessionCard.
- SessionStatusBadge.
- SessionRequestCard.
- ActivityCard.
- KnowledgeCard.
- AudienceCard.
- MetricCard.
- InterestChipList.
- TopicChipList.
- RoleSelector.
- AmbassadorSelector.

---

# 15. UI Primitives

## 15.1 Button

### Uso

Acción principal o secundaria.

### Variantes

- primary.
- secondary.
- outline.
- ghost.
- danger.
- link.

### Tamaños

- sm.
- md.
- lg.

### Estados

- default.
- hover.
- active.
- focus-visible.
- disabled.
- loading.

### Regla

Nunca crear botones locales.

---

## 15.2 Card

### Uso

Contenedor visual reutilizable.

### Variantes

- default.
- interactive.
- elevated.
- highlight.
- glass.
- metric.
- catalog.

### Categorías visuales

- default.
- professor.
- student.
- professional.
- company.
- orientator.
- women-tech.
- ambassador.
- community.

---

## 15.3 Badge

### Uso

Estados, roles, categorías y metadatos.

### Variantes

- neutral.
- info.
- success.
- warning.
- danger.
- role.
- category.

---

## 15.4 Avatar

### Uso

Representación visual de personas o entidades.

### Tamaños

- xs.
- sm.
- md.
- lg.
- xl.

### Reglas

- Debe soportar imagen.
- Debe soportar iniciales.
- Debe soportar placeholder.
- No debe inferir género ni atributos personales.

---

# 16. UI Layout Components

## 16.1 PublicShell

Usado en:

- Home.
- Quiénes somos.
- Soy Profesor Tech.
- Audiencias.
- Eventos públicos.
- Catálogos.

Incluye:

- Topbar público.
- Contenedor principal.
- Footer.

## 16.2 AppShell

Usado en intranet.

Incluye:

- Sidebar.
- Topbar.
- Main content.
- Right rail opcional.

## 16.3 AdminShell

Usado en administración.

Incluye:

- Sidebar admin.
- Header contextual.
- Breadcrumbs.
- Actions bar.
- Main table/detail area.

---

# 17. UI Patterns

## 17.1 HeroCommunity

### Uso

Hero principal de home.

### Contenido

- Título.
- Subtítulo.
- CTA principal.
- CTA secundario.
- Imagen/ilustración opcional.
- Métrica o badge destacado.

## 17.2 MetricsStrip

### Uso

Mostrar impacto de comunidad.

### Ejemplos de métricas

- Años de comunidad.
- Tutoriales publicados.
- Centros inscritos.
- Alumnos impactados.
- Sesiones realizadas.
- Colaboraciones.
- Eventos propios.

## 17.3 FilterPanel

### Uso

Catálogos y listados.

### Debe soportar

- Grupos colapsables.
- Chips activos.
- Limpiar filtros.
- Checkbox/multiselect.
- Responsive drawer en mobile.

## 17.4 ActivityFeed

### Uso

Dashboard intranet.

### Items

- Nueva solicitud.
- Inscripción confirmada.
- Sesión confirmada.
- Contenido publicado.
- Ambassador aprobado.

---

# 18. Componentes de producto Tech Riders

## 18.1 CommunityRoleBadge

### Roles

```ts
type CommunityRole = 'member' | 'staff' | 'collaborator' | 'ambassador';
```

### Mapeo visual

| Rol | Color |
|---|---|
| member | cyan |
| staff | violet |
| collaborator | cyan dark |
| ambassador | green |

## 18.2 ProfileCard

### ViewModel

```ts
export interface ProfileCardVm {
  id: string;
  displayName: string;
  avatarUrl?: string;
  headline?: string;
  bio?: string;
  roles: CommunityRole[];
  topics?: string[];
  socialLinks?: Array<{ label: string; url: string }>;
  primaryAction?: {
    label: string;
    route?: string;
  };
}
```

## 18.3 SessionCard

### ViewModel

```ts
export interface SessionCardVm {
  id: string;
  title: string;
  description?: string;
  type: SessionType;
  status: SessionStatus;
  modality: ActivityModality;
  level?: ActivityLevel;
  date?: string;
  topics: string[];
  ambassadors?: ProfileSummaryVm[];
  primaryAction?: {
    label: string;
    route?: string;
  };
}
```

## 18.4 ActivityCard

Para eventos, formaciones, workshops, podcasts y mentorías.

## 18.5 AudienceCard

Para públicos:

- Docentes.
- Estudiantes.
- Profesionales.
- Empresas.
- Orientadores.
- Starters.
- Women in Tech.
- Conócenos.

---

# 19. Componentes de formularios

## 19.1 FormField

Debe envolver:

- Label.
- Control.
- Hint.
- Error.
- Required marker.

## 19.2 Inputs

Todos los inputs deben:

- Mostrar error.
- Soportar disabled.
- Soportar readonly.
- Tener focus visible.
- Estar asociados a label.

## 19.3 Wizards

Usados para:

- Alta miembro.
- Solicitud Ambassador.
- Solicitud de sesión.
- Propuesta de sesión.

---

# 20. Componentes de feedback y estado

## 20.1 EmptyState

Debe tener:

- Icono.
- Título.
- Descripción.
- CTA opcional.

## 20.2 Skeleton

Para cargas de:

- Cards.
- Tablas.
- Listados.
- Profile summary.

## 20.3 Toast

Tipos:

- success.
- info.
- warning.
- error.

---

# 21. Componentes de datos y administración

## 21.1 DataTable

Debe soportar:

- Columnas configurables.
- Acciones por fila.
- Badges de estado.
- Empty state.
- Loading state.
- Sorting.
- Filtros externos.

## 21.2 AdminActionBar

Debe soportar:

- Acción primaria.
- Acciones secundarias.
- Filtros.
- Exportación si aplica.

---

# 22. Modelo de composición

## 22.1 Regla general

Las páginas se escriben como composición de componentes.

Ejemplo correcto:

```html
<tr-public-shell>
  <tr-page-hero />
  <tr-metrics-strip />
  <tr-audience-grid />
  <tr-cta-section />
</tr-public-shell>
```

Ejemplo incorrecto:

```html
<div class="custom-home-card custom-blue-border">
  ...
</div>
```

## 22.2 Responsabilidad de una página

Una página decide:

- Qué bloques aparecen.
- En qué orden.
- Qué datos pasan a cada bloque.
- Qué rutas se activan.

Una página no decide:

- Cómo se pinta un botón.
- Cómo se pinta una card.
- Cómo se pinta un badge.
- Qué sombra usa un panel.

---

# 23. Blueprints de vistas públicas

## 23.1 Home pública

```text
PublicShell
  Topbar
  HeroCommunity
  MetricsStrip
  ProfileSelectorGrid
    AudienceCard: Docentes
    AudienceCard: Estudiantes
    AudienceCard: Profesionales
    AudienceCard: Empresas
    AudienceCard: Orientadores
    AudienceCard: Starters
    AudienceCard: Women in Tech
    AudienceCard: Conócenos
  FeaturedSessions
  KnowledgeHighlights
  CTASection
  Footer
```

## 23.2 Quiénes somos

```text
PublicShell
  PageHero
  MissionVisionSection
  ValuesGrid
  CommunityRolesSection
  Timeline
  CTASection
  Footer
```

## 23.3 Únete a Tech Riders

```text
PublicShell
  PageHero
  JoinBenefitsGrid
  RoleExplanationCards
  JoinFormWizard
  FAQSection
  CTASection
```

## 23.4 Soy Profesor Tech

```text
PublicShell
  AudienceHero
  BenefitPanel
  FeatureCardGrid
    Solicitar sesión
    Eventos Tech Riders
    Banco de conocimiento
    Recursos para docentes
    Forma parte de la comunidad
  ContactCTA
```

## 23.5 Catálogo de tutoriales

```text
PublicShell
  CatalogHeader
  CatalogLayout
    FilterPanel
    CatalogGrid
      KnowledgeCard
      KnowledgeCard
      KnowledgeCard
    Pagination
```

---

# 24. Blueprints de intranet

## 24.1 Dashboard miembro

```text
AppShell
  SidebarNav
  Topbar
  DashboardHero
  WidgetGrid
    UpcomingSessionsWidget
    RecommendedContentWidget
    MyInterestsWidget
    ActivityFeedWidget
```

## 24.2 Dashboard profesor

```text
AppShell
  RoleSwitcher
  DashboardHero
  QuickActions
    Solicitar sesión
    Explorar recursos
    Ver próximos eventos
  SessionsRequestsTable
  UpcomingEventsTable
  KnowledgeWidget
  ProfileSummary
  InterestsCard
```

## 24.3 Portal Ambassador

```text
AppShell
  AmbassadorHero
  ProfileCompletenessCard
  AvailabilityCard
  AssignedSessionsList
  CallForSessionsList
  ParticipationHistory
```

## 24.4 Portal Staff

```text
AppShell
  StaffHero
  MetricsStrip
  PendingSessionsTable
  MembersActivityFeed
  AmbassadorsPanel
  ContentReviewQueue
```

---

# 25. Blueprints de administración

## 25.1 Admin dashboard

```text
AdminShell
  AdminHeader
  AdminMetricsGrid
  PendingActionsPanel
  RecentActivityFeed
  HealthStatusCards
```

## 25.2 Gestión de miembros

```text
AdminShell
  PageHeader
  AdminActionBar
  FilterPanel
  DataTable
  MemberDetailDrawer
```

## 25.3 Gestión de sesiones

```text
AdminShell
  PageHeader
  StatusTabs
  FilterPanel
  SessionsDataTable
  AssignAmbassadorModal
  SessionDetailDrawer
```

---

# 26. Sistema de navegación

## 26.1 Navegación pública

Elementos base:

- Inicio.
- Únete.
- Quiénes somos.
- Tutoriales / Conocimiento.
- Eventos / Sesiones.
- OrientaTech.
- Contacto.
- Iniciar sesión.

## 26.2 Navegación por audiencias

- Docentes.
- Estudiantes.
- Profesionales.
- Empresas.
- Orientadores.
- Starters.
- Women in Tech.
- Conócenos.

## 26.3 Navegación intranet

- Inicio.
- Mi dashboard.
- Mi perfil.
- Mis sesiones.
- Recursos.
- Eventos.
- Comunidad.
- Configuración.

## 26.4 Navegación Staff/Admin

- Dashboard.
- Miembros.
- Ambassadors.
- Colaboradores.
- Sesiones.
- Eventos.
- Contenido.
- Métricas.
- Configuración.

---

# 27. Sistema de cards

## 27.1 Card base

Toda card debe derivar de `tr-card`.

## 27.2 Variantes

```ts
type CardVariant = 'default' | 'interactive' | 'elevated' | 'highlight' | 'metric' | 'catalog';
```

## 27.3 Categorías visuales

```ts
type CardCategory =
  | 'default'
  | 'professor'
  | 'student'
  | 'professional'
  | 'company'
  | 'orientator'
  | 'starter'
  | 'women-tech'
  | 'community'
  | 'ambassador'
  | 'resource';
```

## 27.4 Regla

No crear `ProfessorCard`, `CompanyCard`, `WomenTechCard` si solo cambia el color.

Crear `AudienceCard` con `category`.

---

# 28. Sistema de perfiles

## 28.1 ProfileCard única

Debe servir para:

- Miembro.
- Staff.
- Community Leaders.
- Ambassador.
- Profesor.
- Profesional.

## 28.2 Variantes

- compact.
- default.
- detailed.
- horizontal.
- admin.

## 28.3 Secciones posibles

- Avatar.
- Nombre visible.
- Headline.
- Roles.
- Temáticas.
- Bio corta.
- Acciones.
- Redes.

---

# 29. Sistema de sesiones

## 29.1 Sesión como concepto central

Usar siempre “sesión”.

No usar “charla”.

## 29.2 Tipos de sesión

```ts
export type SessionType =
  | 'technical'
  | 'orientation'
  | 'employment'
  | 'soft-skills'
  | 'community'
  | 'workshop'
  | 'podcast'
  | 'mentoring'
  | 'company-case'
  | 'women-tech';
```

## 29.3 Estados de sesión

```ts
export type SessionStatus =
  | 'draft'
  | 'requested'
  | 'in-review'
  | 'pending-ambassador'
  | 'ambassador-proposed'
  | 'accepted'
  | 'rejected'
  | 'pending-room'
  | 'confirmed'
  | 'done'
  | 'cancelled'
  | 'closed';
```

## 29.4 Componentes asociados

- SessionCard.
- SessionStatusBadge.
- SessionDetailHeader.
- SessionTimeline.
- SessionRequestWizard.
- SessionAssignmentPanel.
- SessionFeedbackPanel.

---

# 30. Sistema de contenidos

## 30.1 Tipos de contenido

- Tutorial.
- Talk.
- Podcast.
- Artículo.
- Guía.
- Caso real.
- Recurso descargable.
- Vídeo.

## 30.2 KnowledgeCard

Debe mostrar:

- Tipo.
- Título.
- Resumen.
- Temática.
- Nivel.
- Formato.
- Fecha.
- CTA.

---

# 31. Sistema de métricas

## 31.1 MetricCard

Debe mostrar:

- Icono.
- Valor.
- Label.
- Descripción opcional.
- Tendencia opcional.

## 31.2 Uso

- Home pública.
- Staff dashboard.
- Admin dashboard.
- Métricas de comunidad.

---

# 32. Sistema de filtros

## 32.1 FilterPanel

Debe permitir:

- Grupos colapsables.
- Multiselección.
- Chips activos.
- Limpiar todo.
- Buscar dentro de filtros.
- Modo drawer en mobile.

## 32.2 Filtros comunes

- Formato.
- Tema.
- Producto.
- Público.
- Modalidad.
- Nivel.
- Idioma.
- Fecha.
- Estado.

---

# 33. Sistema responsive

## 33.1 Breakpoints

```css
--tr-breakpoint-sm: 640px;
--tr-breakpoint-md: 768px;
--tr-breakpoint-lg: 1024px;
--tr-breakpoint-xl: 1280px;
--tr-breakpoint-2xl: 1536px;
```

## 33.2 Reglas

### Mobile

- Topbar compacta.
- Sidebar como drawer.
- Cards a una columna.
- Filtros como drawer.
- CTA full width.

### Tablet

- Cards a dos columnas.
- Sidebar opcional.
- Dashboard simplificado.

### Desktop

- Layout completo.
- Sidebar fija.
- Right rail opcional.
- Grids densos.

---

# 34. Accesibilidad

## 34.1 Requisitos mínimos

Todos los componentes deben tener:

- Focus visible.
- Navegación por teclado.
- Labels asociados.
- Roles ARIA cuando aplique.
- Contraste suficiente.
- Estado disabled reconocible.
- Estado error claro.

## 34.2 No permitido

- Controles solo por color.
- Iconos sin label si son interactivos.
- Modales sin gestión de foco.
- Toasts críticos sin alternativa persistente.

---

# 35. Estados obligatorios

Toda feature con datos debe implementar:

- Loading.
- Empty.
- Error.
- Success.
- Partial state si aplica.

Ejemplo:

```html
@if (vm().loading) {
  <tr-skeleton />
} @else if (vm().error) {
  <tr-empty-state variant="error" />
} @else if (vm().items.length === 0) {
  <tr-empty-state />
} @else {
  <tr-session-card />
}
```

---

# 36. Microcopy y tono

## 36.1 Tono

- Cercano.
- Claro.
- Motivador.
- Técnico cuando toca.
- Sin humo.

## 36.2 Terminología oficial

Usar:

- Sesión.
- Miembro.
- Staff.
- Community Leaders.
- Ambassador.
- Comunidad.
- Banco de conocimiento.
- Tech Riders.

No usar:

- Charla.
- Speaker como rol principal.
- Usuario cuando se habla de comunidad.
- Portal Tajamar como foco principal.

---

# 37. Reglas Angular

## 37.1 Obligatorio

- Standalone components.
- Signals.
- `input()`.
- `input.required()`.
- `output()`.
- `computed()`.
- `ChangeDetectionStrategy.OnPush`.
- Templates con control flow moderno si aplica.

## 37.2 Prohibido

- Crear NgModules nuevos.
- Usar `@Input()` en componentes nuevos si se ha adoptado `input()`.
- Lógica de negocio en UI primitives.
- HTTP directo en componentes presentacionales.
- CSS visual local en páginas.

---

# 38. Patrones con Signals

## 38.1 Component UI

```ts
@Component({
  selector: 'tr-example',
  standalone: true,
  templateUrl: './example.component.html',
  styleUrl: './example.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExampleComponent {
  title = input.required<string>();
  variant = input<'default' | 'highlight'>('default');

  classes = computed(() => [
    'tr-example',
    `tr-example--${this.variant()}`,
  ].join(' '));
}
```

## 38.2 Feature component

```ts
@Component({
  selector: 'tr-sessions-page',
  standalone: true,
  imports: [SessionCardComponent, EmptyStateComponent, SkeletonComponent],
  templateUrl: './sessions-page.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SessionsPageComponent {
  private readonly sessionsStore = inject(SessionsStore);

  vm = computed(() => ({
    loading: this.sessionsStore.loading(),
    error: this.sessionsStore.error(),
    sessions: this.sessionsStore.filteredSessions(),
  }));
}
```

---

# 39. Ejemplos de componentes Angular

## 39.1 Button

```ts
import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';

export type TrButtonVariant = 'primary' | 'secondary' | 'outline' | 'ghost' | 'danger' | 'link';
export type TrButtonSize = 'sm' | 'md' | 'lg';

@Component({
  selector: 'tr-button',
  standalone: true,
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TrButtonComponent {
  variant = input<TrButtonVariant>('primary');
  size = input<TrButtonSize>('md');
  disabled = input(false);
  loading = input(false);
  fullWidth = input(false);

  pressed = output<void>();

  classes = computed(() => [
    'tr-button',
    `tr-button--${this.variant()}`,
    `tr-button--${this.size()}`,
    this.fullWidth() ? 'tr-button--full' : '',
    this.loading() ? 'tr-button--loading' : '',
  ].filter(Boolean).join(' '));
}
```

```html
<button
  type="button"
  [class]="classes()"
  [disabled]="disabled() || loading()"
  (click)="pressed.emit()"
>
  @if (loading()) {
    <span class="tr-button__spinner" aria-hidden="true"></span>
  }
  <ng-content />
</button>
```

```scss
.tr-button {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: var(--tr-space-2);
  border: 1px solid transparent;
  border-radius: var(--tr-radius-lg);
  font-family: var(--tr-font-family-sans);
  font-weight: 700;
  cursor: pointer;
  transition: background-color 160ms ease, border-color 160ms ease, color 160ms ease, box-shadow 160ms ease;
}

.tr-button:focus-visible {
  outline: none;
  box-shadow: var(--tr-shadow-focus);
}

.tr-button--primary {
  background: var(--tr-gradient-cta);
  color: var(--tr-color-text-primary);
}

.tr-button--secondary {
  background: rgba(0, 215, 255, .12);
  color: var(--tr-color-brand-cyan);
  border-color: var(--tr-color-border-default);
}
```

## 39.2 Card

```ts
import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';

export type TrCardVariant = 'default' | 'interactive' | 'elevated' | 'highlight' | 'metric' | 'catalog';
export type TrCardCategory = 'default' | 'professor' | 'student' | 'professional' | 'company' | 'orientator' | 'women-tech' | 'community' | 'ambassador' | 'resource';

@Component({
  selector: 'tr-card',
  standalone: true,
  template: '<section [class]="classes()"><ng-content /></section>',
  styleUrl: './card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TrCardComponent {
  variant = input<TrCardVariant>('default');
  category = input<TrCardCategory>('default');

  classes = computed(() => [
    'tr-card',
    `tr-card--${this.variant()}`,
    `tr-card--${this.category()}`,
  ].join(' '));
}
```

## 39.3 SessionCard

```ts
@Component({
  selector: 'tr-session-card',
  standalone: true,
  imports: [TrCardComponent, TrBadgeComponent, TrButtonComponent, TrAvatarGroupComponent],
  templateUrl: './session-card.component.html',
  styleUrl: './session-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TrSessionCardComponent {
  session = input.required<SessionCardVm>();
  selected = output<string>();

  statusLabel = computed(() => mapSessionStatusToLabel(this.session().status));
}
```

---

# 40. Estructura de carpetas por componente

```text
libs/shared/ui-primitives/src/lib/button/
  button.component.ts
  button.component.html
  button.component.scss
  button.component.spec.ts
  button.types.ts
  README.md
  index.ts
```

## 40.1 README mínimo por componente

Debe incluir:

- Descripción.
- Cuándo usar.
- Cuándo no usar.
- Inputs.
- Outputs.
- Variantes.
- Ejemplo.
- Accesibilidad.

---

# 41. Contratos de ViewModel

## 41.1 CommunityRole

```ts
export type CommunityRole = 'member' | 'staff' | 'collaborator' | 'ambassador';
```

## 41.2 ActivityModality

```ts
export type ActivityModality = 'presential' | 'online' | 'hybrid';
```

## 41.3 ActivityLevel

```ts
export type ActivityLevel = 'intro' | 'basic' | 'intermediate' | 'advanced' | 'expert';
```

## 41.4 ProfileSummaryVm

```ts
export interface ProfileSummaryVm {
  id: string;
  displayName: string;
  avatarUrl?: string;
  headline?: string;
  roles: CommunityRole[];
}
```

---

# 42. Naming conventions

## 42.1 Selectores

```text
tr-button
tr-card
tr-badge
tr-profile-card
tr-session-card
tr-activity-card
tr-audience-card
```

## 42.2 Clases CSS

```text
.tr-button
.tr-button--primary
.tr-card
.tr-card--interactive
.tr-session-card
.tr-session-card__header
```

## 42.3 Librerías

```text
libs/shared/ui-primitives
libs/community/ui-profile
libs/sessions/ui-session-card
```

---

# 43. Anti-patrones prohibidos

## 43.1 Visuales

Prohibido:

```scss
color: #00D7FF;
margin: 17px;
border-radius: 13px;
box-shadow: 0 0 21px blue;
```

Correcto:

```scss
color: var(--tr-color-brand-cyan);
margin: var(--tr-space-4);
border-radius: var(--tr-radius-xl);
box-shadow: var(--tr-shadow-glow-cyan);
```

## 43.2 Componentes

Prohibido:

```html
<button class="my-custom-button">Enviar</button>
```

Correcto:

```html
<tr-button variant="primary">Enviar</tr-button>
```

## 43.3 Terminología

Prohibido:

```text
charla
speaker como rol principal
usuario Tech Rider como ponente
```

Correcto:

```text
sesión
Ambassador
Miembro Tech Riders
```

---

# 44. Checklist para agentes

Antes de generar código:

- [ ] He identificado si es primitive, pattern, domain UI, feature o page.
- [ ] He comprobado si ya existe el componente.
- [ ] He revisado tokens disponibles.
- [ ] He decidido la librería correcta.
- [ ] He evitado crear estilos locales.
- [ ] He mantenido terminología Tech Riders.

---

# 45. Definition of Ready UX/UI

Una issue frontend está lista si:

- Tiene objetivo de usuario claro.
- Indica vista o componente afectado.
- Indica rol/perfil afectado.
- Tiene criterios de aceptación.
- Tiene estados esperados.
- Indica datos necesarios.
- Indica permisos si aplica.
- Indica componentes reutilizables.
- Indica si requiere token nuevo.

---

# 46. Definition of Done UX/UI

Una issue está terminada si:

- Usa componentes compartidos.
- Usa tokens.
- No hardcodea visuales.
- Es responsive.
- Es accesible.
- Tiene loading/empty/error.
- Usa terminología correcta.
- Respeta Nx boundaries.
- Tiene tests básicos.
- Tiene README si es componente nuevo.

---

# 47. Checklist de Pull Request

```md
## UX/UI
- [ ] Respeta identidad dark Tech Riders.
- [ ] Usa tokens.
- [ ] No hardcodea colores.
- [ ] No hardcodea spacing.
- [ ] Reutiliza componentes.
- [ ] No crea variantes visuales innecesarias.

## Angular
- [ ] Es standalone.
- [ ] Usa signals/input/output/computed.
- [ ] Usa OnPush.
- [ ] No usa NgModules nuevos.
- [ ] No hace HTTP en UI primitives.

## Producto
- [ ] Usa “sesión”, no “charla”.
- [ ] Tech Riders se trata como comunidad.
- [ ] Roles correctos: Member, Staff, Community Leaders, Ambassador.

## Calidad
- [ ] Tiene tests.
- [ ] Tiene estados loading/empty/error.
- [ ] Es responsive.
- [ ] Es accesible.
```

---

# 48. Roadmap de implementación del Design System

## Fase 1 - Foundation

- Tokens.
- Theme CSS.
- Button.
- Card.
- Badge.
- Avatar.
- IconButton.
- FormField.

## Fase 2 - Layout

- PublicShell.
- AppShell.
- AdminShell.
- Topbar.
- SidebarNav.
- PageHeader.
- SectionHeader.
- Grid.

## Fase 3 - Patterns

- HeroCommunity.
- PageHero.
- MetricsStrip.
- CTASection.
- FilterPanel.
- ActivityFeed.
- DataTable.

## Fase 4 - Product UI

- CommunityRoleBadge.
- ProfileCard.
- SessionCard.
- ActivityCard.
- KnowledgeCard.
- AudienceCard.
- MetricCard.

## Fase 5 - Vistas

- Home.
- Quiénes somos.
- Soy Profesor Tech.
- Catálogo tutoriales.
- Dashboard miembro/profesor.
- Portal Ambassador.
- Portal Staff.

---

# 49. Backlog inicial de componentes

## EPIC-DS-01 Design Foundation

### DS-001 Tokens base

Crear librería `libs/design/tokens`.

Criterios:

- Exporta CSS variables.
- Exporta JSON tokens.
- Incluye colores, spacing, radius, shadows, typography.

### DS-002 Theme global

Crear `libs/design/theme`.

Criterios:

- Aplica dark theme.
- Define body background.
- Define surfaces.
- Permite extensión futura.

## EPIC-DS-02 Primitives

### DS-010 Button

Crear `tr-button`.

### DS-011 Card

Crear `tr-card`.

### DS-012 Badge

Crear `tr-badge`.

### DS-013 Avatar

Crear `tr-avatar` y `tr-avatar-group`.

## EPIC-DS-03 Product Components

### DS-030 CommunityRoleBadge

Crear badge de roles.

### DS-031 ProfileCard

Crear card de perfil reusable.

---

# 52. Estado de consolidación UX/UI (2026-08-09)

Este estado refleja la consolidación aplicada en el frontend público para alinear implementación real con este documento maestro.

## 52.1 Criterios de verificación usados

- Reutilización de bloques visuales desde `src/app/shared/ui`.
- Uso de tokens de diseño (sin hardcode visual nuevo).
- Sin creación de componentes visuales ad hoc dentro de páginas.
- Storybook consolidado al catálogo shared/ui.
- Build de Angular en verde tras cambios.

## 52.2 Matriz de cumplimiento por módulo

| Página | Shared/UI | Tokens | Storybook alineado | Estado |
|---|---|---|---|---|
| Home | Sí (metrics-strip, progress-cards, journey-steps) | Sí | Sí | Cumple |
| Únete | Sí (progress-cards, journey-steps) | Sí | Sí | Cumple |
| Eventos | Sí (metrics-strip, progress-cards) | Sí | Sí | Cumple |
| Quiénes somos | Sí (metrics-strip, profile-cards) | Sí | Sí | Cumple |
| OrientaTech | Sí (metrics-strip, feature-cards, progress-cards) | Sí | Sí | Cumple |
| Contacto | Sí (metrics-strip, journey-steps) | Sí | Sí | Cumple |
| Tutoriales | Sí (metrics-strip, feature-cards) | Sí | Sí | Cumple |
| Intranet · Admin Dashboard | Sí (metrics-strip, progress-cards) | Sí | N/A | Cumple |
| Intranet · Admin Configuración | Sí (metrics-strip + primitives de formulario) | Sí | N/A | Cumple |
| Intranet · Admin Auditoría | Sí (primitives/shared base) | Sí | N/A | Cumple |

## 52.3 Componentes shared/ui consolidados

- metrics-strip
- progress-cards
- journey-steps
- feature-cards
- profile-cards

Cada componente dispone de story dedicada en Storybook bajo `src/app/shared/ui/**`.

## 52.4 Decisiones de consolidación aplicadas

1. Storybook se limita a historias del catálogo shared/ui para evitar demos legacy desacopladas del sistema real.
2. La composición de páginas públicas se movió a bloques reutilizables con contratos de entrada claros (view models por página).
3. Intranet/admin incorporó el mismo enfoque en paneles de resumen y estado operativo (metrics/progress shared).
4. Se redujo CSS local duplicado en páginas migradas y se priorizó shell estructural tokenizado.
5. Tutoriales se realineó a patrón de catálogo funcional con panel lateral de filtros + tarjetas de recurso y CTA de registro/detalles, consumiendo datos reales del backend paginado.

## 52.5 Gaps pendientes

- Sin gaps críticos de consolidación UX/UI para el alcance actual.

## 52.6 Evidencia de validación técnica

- Diagnósticos editor: sin errores en archivos migrados.
- Build Angular: OK (`ng build`), salida generada en `dist/techito`.


### DS-032 SessionCard

Crear card de sesión.

### DS-033 AudienceCard

Crear card de audiencia.

## EPIC-DS-04 Layouts

### DS-050 PublicShell

### DS-051 AppShell

### DS-052 AdminShell

---

# 50. Prompt maestro para agentes frontend

```text
Eres un agente frontend del monorepo Nx de Tech Riders.

Tu objetivo es construir frontend Angular LTS de alta calidad, escalable y reutilizable.

Stack obligatorio:
- Angular LTS.
- Standalone Components.
- Signals.
- input(), input.required(), output(), computed().
- ChangeDetectionStrategy.OnPush.
- Nx Monorepo.
- SCSS con CSS Custom Properties.

Identidad de producto:
- Tech Riders es una comunidad tecnológica.
- Miembro Tech Riders es cualquier persona que se une.
- Staff y Community Leaders dan forma y operan la comunidad.
- Ambassador participa activamente en actividades y ayuda a extender comunidad.
- Usa siempre “sesión”, nunca “charla”.

Identidad visual:
- Dark-first.
- Fondo azul noche.
- Cards dark glass.
- Bordes cyan translúcidos.
- Gradientes azul/cyan/violeta.
- Acentos por categoría.
- Nada de estética gaming ni cyberpunk excesivo.

Reglas técnicas:
- No hardcodear colores.
- No hardcodear spacing.
- No hardcodear radius.
- No crear botones locales.
- No crear cards locales.
- No crear badges locales.
- Reutilizar libs/shared/ui-primitives.
- Usar tokens de libs/design/tokens.
- Respetar Nx boundaries.

Antes de crear algo:
1. Comprueba si ya existe un componente.
2. Decide si es primitive, pattern, domain UI, feature o page.
3. Ubícalo en la librería correcta.
4. Usa tokens.
5. Añade tests y README si es componente nuevo.

Toda entrega debe incluir:
- component.ts
- component.html
- component.scss
- component.spec.ts
- index.ts
- ejemplo de uso
- documentación de inputs/outputs
```

---

# 51. Resumen ejecutivo para pegar en el repo

Tech Riders utiliza un UX/UI Functional Design System dark-first basado en tokens, componentes reutilizables y composición por capas.

Las páginas no definen visuales propios. Las páginas componen componentes.

La arquitectura frontend se organiza en Nx con apps y libs separadas por design, shared, community, sessions, activities, content, audiences y admin.

El stack oficial es Angular LTS con standalone components, signals, input(), output(), computed() y ChangeDetectionStrategy.OnPush.

La terminología oficial de producto es:

- Tech Riders = comunidad completa.
- Miembro = persona que se une.
- Staff = coordinación y gobierno.
- Community Leaders = miembro que ayuda a construir iniciativas.
- Ambassador = persona activa que participa y extiende comunidad.
- Sesión = unidad principal de actividad.

Está prohibido:

- Usar “charla”.
- Crear botones locales.
- Crear cards locales.
- Hardcodear colores.
- Hardcodear spacing.
- Crear NgModules nuevos.
- Saltarse boundaries Nx.

La prioridad de implementación es:

1. Tokens.
2. Theme.
3. Primitives.
4. Layouts.
5. Patterns.
6. Product components.
7. Vistas finales.

---

# Cierre

Este documento es la fuente de verdad para construir la plataforma Tech Riders desde frontend.

Si una decisión visual, de estructura o de componente no está alineada con este documento, debe considerarse deuda de diseño o deuda técnica.

El objetivo no es construir pantallas bonitas de forma aislada.

El objetivo es construir un sistema frontend coherente, escalable, reutilizable y fiel a la identidad de Tech Riders.
