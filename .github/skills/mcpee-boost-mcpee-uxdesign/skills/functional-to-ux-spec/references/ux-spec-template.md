# Especificación UX/UI — [Nombre de la Tarea / Historia]

> **Tarea origen:** [ID y título de la tarea — ej. #12345 · Gestión de pagos recurrentes]  
> **Generado por:** @functional-to-ux-spec  
> **Fecha:** [YYYY-MM-DD]  
> **Estado:** Borrador / En revisión / Aprobado

---

## A) Intención

- **Objetivo usuario:** [Qué quiere conseguir el usuario con esta funcionalidad]
- **Objetivo negocio:** [Qué valor aporta al producto o al negocio]
- **Supuestos:** [Qué se asume como verdadero — ej. usuario autenticado, datos ya cargados]
- **Ambigüedades:** [Qué necesita aclararse antes de diseñar — dejar vacío si ninguna]

---

## B) Flujo de Usuario

```
[Punto de entrada]
  ↓
[Paso 1 — descripción breve]
  ↓
[Paso 2 — decisión o acción]
  ├── Camino A: [éxito / acción completada]
  └── Camino B: [error / confirmación / cancelación]
        ↓
[Punto de salida / estado final]
```

**Flujos secundarios:**
- [Flujo alternativo 1 — ej. sin resultados, paginación, exportación]
- [Flujo alternativo 2 — ej. acción destructiva con confirmación]

---

## C) Arquitectura de Información

| Zona | Contenido | Jerarquía | Notas |
|------|-----------|-----------|-------|
| Cabecera | [título, breadcrumbs, acciones globales] | H1 | |
| Área principal | [listado / formulario / detalle] | H2 | |
| Filtros / Sidebar | [filtros, búsqueda, ordenación] | — | Colapsable en mobile |
| Acciones por fila | [ver detalle, editar, eliminar] | — | |
| Paginación / Exportación | [controles de navegación, descarga] | — | |
| Modales / Overlays | [confirmación, detalle, error] | — | Focus trap obligatorio |

---

## D) Wireframe Descriptivo

### Vista principal

```
┌─────────────────────────────────────────────┐
│ [Título de página]           [Acción global] │
├─────────────────────────────────────────────┤
│ [Filtros: campo1 · campo2 · campo3]  [Reset] │
├─────────────────────────────────────────────┤
│ [Tabla / Listado]                           │
│  Col1 | Col2 | Col3 | Col4 | Acciones       │
│  ···  | ···  | ···  | ···  | [Ver][Pausar]  │
│  ···  | ···  | ···  | ···  | [Ver][Cancelar]│
├─────────────────────────────────────────────┤
│ [Paginación: Anterior · 1 2 3 · Siguiente]  │
│ [Exportar a Excel]                          │
└─────────────────────────────────────────────┘
```

### Modal de confirmación (acción destructiva)

```
┌───────────────────────────────┐
│ [Título de la acción]         │
│                               │
│ [Descripción del efecto]      │
│                               │
│ [Motivo (texto libre)] ←opcional
│                               │
│ [Cancelar]      [Confirmar]   │
└───────────────────────────────┘
```

---

## E) Microcopy

| Elemento | Texto propuesto |
|----------|----------------|
| Título de página | "[Nombre de la sección]" |
| CTA principal | "[Verbo + objeto — ej. Exportar pagos]" |
| Estado vacío | "[Sin X todavía. Descripción breve de cuándo aparecerán.]" |
| Confirmación de acción | "¿Seguro que quieres [acción]? [Efecto explicado en 1 frase.]" |
| Éxito tras acción | "[Acción] realizado correctamente." |
| Error genérico | "No pudimos completar la acción. Inténtalo de nuevo." |
| Error con causa | "[Causa específica]. [Qué puede hacer el usuario.]" |
| Loading | "Cargando [nombre del contenido]…" |

---

## F) Estados

### Loading
- Comportamiento: skeleton loader en [zona afectada] mientras se obtienen datos
- Duración esperada: < [N]s
- Si supera: mostrar mensaje "Tardando más de lo habitual…" + opción de cancelar

### Empty (sin resultados)
- Mensaje: "[Texto del estado vacío definido en microcopy]"
- Acción sugerida: [CTA o enlace relevante]
- Visual: [icono / ilustración si aplica]

### Error
- Mensaje: "[Texto del error definido en microcopy]"
- Causa visible: [Sí / No — según si el sistema la conoce]
- Recuperación: [Botón reintentar / Contactar soporte / Volver atrás]

### Success
- Mensaje: "[Texto de confirmación]"
- Duración: toast [3s] / banner persistente / redirección
- Siguiente paso sugerido: [acción o navegación]

### Confirmación (acciones destructivas / irreversibles)
- Trigger: [qué acción lo lanza]
- Contenido del modal: [título + descripción de efectos + motivo opcional + botones]
- Foco inicial: [botón cancelar — recomendado para acciones destructivas]

---

## G) Accesibilidad

- [ ] Roles ARIA definidos: `[role="table/grid/dialog/alertdialog/...]` en [componentes clave]
- [ ] Cabeceras de tabla con `scope="col"` y `<caption>` descriptivo
- [ ] Acciones por fila con `aria-label` contextual — ej. `aria-label="Pausar Netflix"`
- [ ] Navegación por teclado: Tab · Shift+Tab · Enter · Escape · Flechas
- [ ] Estados nunca comunicados solo por color — siempre color + icono + texto
- [ ] Touch targets ≥ 44×44px en mobile para todos los elementos interactivos
- [ ] Labels asociados a todos los inputs del formulario/filtros
- [ ] Focus trap en modales: foco entra al abrir, vuelve al elemento origen al cerrar
- [ ] Orden de foco lógico y coherente con el flujo visual

---

## H) Criterios de Aceptación UX

- [ ] [Criterio 1 — ej. El listado carga en < 2s con skeleton visible durante la espera]
- [ ] [Criterio 2 — ej. Los filtros son combinables y reseteables con un solo click]
- [ ] [Criterio 3 — ej. Cada acción destructiva requiere confirmación explícita con modal]
- [ ] [Criterio 4 — ej. El mensaje de éxito/error es visible tras cada acción]
- [ ] [Criterio 5 — ej. Toda la tabla es navegable por teclado (Tab + Enter + flechas)]
- [ ] [Criterio 6 — ej. La exportación genera el fichero con el nombre de formato correcto]
- [ ] [Criterio de accesibilidad específico del componente]

---

## I) Métrica / Hipótesis

- **Hipótesis:** Si [mejora UX concreta], entonces [resultado esperado medible]
- **Métrica principal:** [qué medir — ej. tasa de uso de filtros, tiempo en tarea, errores por sesión]
- **Método de medición:** [cómo — ej. analytics de eventos, test de usabilidad, encuesta post-tarea]
