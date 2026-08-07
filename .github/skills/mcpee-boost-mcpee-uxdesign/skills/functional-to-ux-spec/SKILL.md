---
name: 'functional-to-ux-spec'
description: 'Transforma tareas funcionales o historias de usuario (de DevOps, Jira o copy-paste) en especificaciones UX/UI completas siguiendo el formato estándar de BoostDesign.'
---

# Skill: Functional-to-UX Spec

Este skill convierte cualquier requerimiento funcional — ya sea una historia de usuario de Azure DevOps, Jira o texto pegado directamente — en una especificación UX/UI completa, estructurada y lista para que el equipo de diseño y desarrollo trabajen sin ambigüedad.

## Referencias y Estándares

- **WCAG 2.1 AA**: https://www.w3.org/WAI/WCAG21/quickref/
- **WAI-ARIA 1.2**: https://www.w3.org/TR/wai-aria-1.2/
- **Nielsen Norman Group — User Stories for UX**: https://www.nngroup.com/
- **Material Design 3 — Patterns**: https://m3.material.io/patterns
- **JTBD (Jobs to Be Done)**: https://www.intercom.com/resources/books/intercom-jobs-to-be-done

## Capacidades

### 1. Análisis del Requerimiento Funcional
- Extrae la intención del usuario (quién, qué quiere, para qué)
- Identifica las acciones clave y los flujos principales
- Detecta entidades de datos relevantes (listados, filtros, formularios, acciones)
- Identifica restricciones, estados y casos especiales mencionados
- Señala supuestos implícitos y ambigüedades que requieren aclaración

### 2. Generación de Flujo de Usuario
- Mapea el flujo de navegación paso a paso
- Identifica bifurcaciones (éxito / error / confirmación)
- Define el punto de entrada y los puntos de salida
- Detecta flujos secundarios (acciones opcionales, cancelación, historial)

### 3. Arquitectura de Información
- Propone la estructura de secciones y jerarquía de contenido
- Define qué datos son visibles, cuáles son secundarios
- Establece agrupaciones lógicas y orden de presentación
- Sugiere patrones de componente adecuados (tabla, lista, tarjeta, formulario)

### 4. Wireframe Descriptivo
- Describe el layout en texto estructurado (sin necesidad de herramienta de diseño)
- Define zonas: cabecera, acciones principales, contenido, paginación, modales
- Especifica comportamiento de cada componente (expandible, modal, inline edit, etc.)
- Propone la posición de filtros, buscadores y exportaciones

### 5. Microcopy
- Genera títulos, subtítulos y descripciones para cada sección
- Define etiquetas de botones, CTAs y mensajes de confirmación
- Propone textos para estados vacíos, errores y mensajes de éxito
- Asegura tono coherente con el contexto del proyecto

### 6. Estados y Recuperación
- **Loading**: skeleton loaders, spinners, mensajes de espera
- **Empty**: mensaje descriptivo + acción sugerida
- **Error**: mensaje claro + causa + acción de recuperación
- **Success**: confirmación + siguiente paso sugerido
- **Confirmación**: para acciones destructivas o irreversibles (modal con motivo)

### 7. Accesibilidad Integrada
- ARIA roles y atributos para cada componente identificado
- Navegación por teclado (Tab, Enter, Escape, flechas)
- Semántica de tabla, formulario y controles interactivos
- Estados con color + icono + texto (nunca solo color)
- Touch targets mínimos (44×44px mobile)

### 8. Criterios de Aceptación UX
- Criterios testeables y observables (no subjetivos)
- Cobertura de flujo feliz + flujos alternativos
- Criterios de rendimiento percibido (tiempos de carga, feedback)
- Criterios de accesibilidad específicos al componente

## Comandos de Uso

```
# A partir de una tarea extraída con devops-connector
@functional-to-ux-spec interpreta tarea #12345

# Con texto pegado directamente
@functional-to-ux-spec interpreta este requerimiento:
"Como cliente quiero consultar mis pagos recurrentes..."

# Solo el flujo de usuario
@functional-to-ux-spec genera el flujo de usuario para esta historia

# Solo criterios de aceptación UX
@functional-to-ux-spec extrae los criterios de aceptación UX de esta tarea
```

## Plantilla de Salida Estándar

> La plantilla completa está en [`references/ux-spec-template.md`](references/ux-spec-template.md).  
> Copilot la usa como base para generar cada especificación. Puedes copiarla manualmente para rellenarla sin usar Copilot.

```markdown
## A) Intención
- Objetivo usuario: [qué quiere conseguir el usuario]
- Objetivo negocio: [qué valor aporta al producto/negocio]
- Supuestos: [qué se asume como verdadero para esta spec]
- Ambigüedades: [qué necesita aclararse antes de diseñar]

## B) Flujo de Usuario
[Punto de entrada] → [Paso 1] → [Decisión/Acción] → [Resultado]
↳ [Flujo alternativo: error / confirmación / cancelación]

## C) Arquitectura de Información
- Sección principal: [contenido, orden, jerarquía]
- Sección secundaria: [filtros, acciones, metadatos]
- Modales / overlays: [cuándo aparecen, qué contienen]

## D) Wireframe Descriptivo
[Descripción por zonas: cabecera, área principal, acciones, paginación]
[Comportamiento de cada componente clave]

## E) Microcopy
- Título de página: "..."
- CTA principal: "..."
- Estado vacío: "..."
- Confirmación de acción: "..."
- Error genérico: "..."

## F) Estados
- Loading: [descripción del comportamiento visual]
- Empty: [mensaje + acción sugerida]
- Error: [mensaje + causa + recuperación]
- Success: [confirmación + siguiente paso]
- Confirmación: [modal con texto + motivo si aplica]

## G) Accesibilidad
- [ ] Roles ARIA definidos para [componentes clave]
- [ ] Navegación por teclado: Tab / Enter / Escape / flechas
- [ ] Estados nunca solo por color
- [ ] Touch targets ≥ 44×44px
- [ ] Labels asociados a todos los inputs

## H) Criterios de Aceptación UX
- [ ] [Criterio 1 — observable y testeable]
- [ ] [Criterio 2]
- [ ] [Criterio N]

## I) Métrica / Hipótesis
- Hipótesis: Si [mejora UX], entonces [resultado esperado]
- Métrica: [qué medir y cómo]
```

## Integración con BoostDesign

- **Input desde**: `devops-connector` (automático) o texto directo del usuario (copy-paste)
- **Output hacia**: revisión del equipo UX → `docs/design-tokens/patterns.json` → equipo técnico
- **Compatible con**: `ux-audit`, `design-system-generator`, `aria-accessibility-patterns`

```
@devops-connector → tarea funcional → @functional-to-ux-spec → spec UX completa
                                                              ↓
                                              docs/design-tokens/ + criterios de aceptación
```
