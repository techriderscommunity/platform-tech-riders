# Patrones de Teclado — Referencia ARIA

> Referencia operativa basada en [WAI-ARIA Authoring Practices Guide](https://www.w3.org/WAI/ARIA/apg/patterns/).  
> Para cada componente: roles ARIA requeridos + interacción de teclado esperada.

---

## Botón (`button`)

**Roles:** `role="button"` (preferir `<button>` nativo)  
**Atributos:** `aria-label`, `aria-expanded` (si abre contenido), `aria-pressed` (toggle), `aria-disabled`

| Tecla | Acción |
|-------|--------|
| `Enter` | Activa el botón |
| `Space` | Activa el botón |

---

## Enlace (`link`)

**Roles:** `role="link"` (preferir `<a href>` nativo)

| Tecla | Acción |
|-------|--------|
| `Enter` | Activa el enlace |

---

## Checkbox

**Roles:** `role="checkbox"` (preferir `<input type="checkbox">`)  
**Atributos:** `aria-checked` (`true` / `false` / `mixed`)

| Tecla | Acción |
|-------|--------|
| `Space` | Cambia el estado (checked/unchecked) |

---

## Radio Group

**Roles:** `role="radiogroup"` + `role="radio"` (preferir `<input type="radio">`)  
**Atributos:** `aria-checked`, `aria-labelledby`

| Tecla | Acción |
|-------|--------|
| `Tab` | Mueve el foco al grupo; dentro del grupo, a la opción seleccionada |
| `↑` / `←` | Mueve a la opción anterior (con wrap) |
| `↓` / `→` | Mueve a la opción siguiente (con wrap) |
| `Space` | Selecciona la opción enfocada |

---

## Combobox / Select

**Roles:** `role="combobox"` + `role="listbox"` + `role="option"`  
**Atributos:** `aria-expanded`, `aria-haspopup="listbox"`, `aria-activedescendant`, `aria-selected`

| Tecla | Acción |
|-------|--------|
| `↓` | Abre el listbox si está cerrado; mueve al siguiente option |
| `↑` | Mueve al option anterior |
| `Enter` | Selecciona el option activo y cierra |
| `Escape` | Cierra el listbox sin seleccionar |
| `Home` / `End` | Mueve al primer / último option |
| Letras | Mueve al primer option que empieza con esa letra |

---

## Listbox

**Roles:** `role="listbox"` + `role="option"`  
**Atributos:** `aria-multiselectable`, `aria-selected`, `aria-activedescendant`

| Tecla | Acción |
|-------|--------|
| `↑` / `↓` | Mueve el foco entre opciones |
| `Home` / `End` | Primera / última opción |
| `Space` | Selecciona / deselecciona (multiselect) |
| `Shift + ↑/↓` | Extiende la selección (multiselect) |
| `Ctrl + A` | Selecciona todo (multiselect) |
| Letras | Mueve al primer option que empieza con esa letra |

---

## Menu / Menubar

**Roles:** `role="menubar"` / `role="menu"` + `role="menuitem"` / `role="menuitemcheckbox"` / `role="menuitemradio"`  
**Atributos:** `aria-haspopup`, `aria-expanded`

### Menubar (horizontal)
| Tecla | Acción |
|-------|--------|
| `←` / `→` | Mueve entre items del menubar |
| `↓` | Abre el submenu del item activo |
| `Enter` / `Space` | Activa el item o abre submenu |
| `Escape` | Cierra el submenu activo |

### Menu (vertical)
| Tecla | Acción |
|-------|--------|
| `↑` / `↓` | Mueve entre items del menú |
| `→` | Abre submenu si existe |
| `←` | Cierra submenu y vuelve al item padre |
| `Enter` / `Space` | Activa el item |
| `Escape` | Cierra el menú |
| `Home` / `End` | Primer / último item |
| Letras | Mueve al primer item que empieza con esa letra |

---

## Dialog / Modal

**Roles:** `role="dialog"` (no modal) / `role="alertdialog"` (requiere acción del usuario)  
**Atributos:** `aria-modal="true"`, `aria-labelledby`, `aria-describedby`

| Tecla | Acción |
|-------|--------|
| `Tab` | Mueve el foco al siguiente elemento (focus trap dentro del dialog) |
| `Shift + Tab` | Mueve el foco al elemento anterior |
| `Escape` | Cierra el dialog (si la acción es cancelable) |

**Importante:** Al abrir, el foco se mueve al primer elemento enfocable o al dialog mismo. Al cerrar, el foco vuelve al elemento que abrió el dialog.

---

## Tabs

**Roles:** `role="tablist"` + `role="tab"` + `role="tabpanel"`  
**Atributos:** `aria-selected`, `aria-controls`, `aria-labelledby`, `tabindex`

| Tecla | Acción |
|-------|--------|
| `←` / `→` | Mueve entre tabs (automáticamente activa el tab en algunos patrones) |
| `Home` / `End` | Primer / último tab |
| `Enter` / `Space` | Activa el tab (si no es activación automática) |
| `Tab` | Mueve el foco al tabpanel activo |

---

## Accordion

**Roles:** `role="button"` para el trigger (o `<button>` nativo)  
**Atributos:** `aria-expanded`, `aria-controls`, `id` en el panel

| Tecla | Acción |
|-------|--------|
| `Enter` / `Space` | Expande / colapsa el panel |
| `Tab` | Mueve al siguiente trigger (o elemento enfocable) |
| `Shift + Tab` | Mueve al trigger anterior |

---

## Tree View

**Roles:** `role="tree"` + `role="treeitem"` + `role="group"` (subnivel)  
**Atributos:** `aria-expanded`, `aria-selected`, `aria-level`, `aria-posinset`, `aria-setsize`

| Tecla | Acción |
|-------|--------|
| `↑` / `↓` | Mueve entre nodos visibles |
| `→` | Expande nodo colapsado / mueve al primer hijo si ya está expandido |
| `←` | Colapsa nodo expandido / mueve al nodo padre si ya está colapsado |
| `Home` / `End` | Primer / último nodo visible |
| `Enter` / `Space` | Selecciona / activa el nodo |
| Letras | Mueve al primer nodo visible que empieza con esa letra |

---

## Grid / Data Grid

**Roles:** `role="grid"` + `role="row"` + `role="gridcell"` / `role="rowheader"` / `role="columnheader"`  
**Atributos:** `aria-rowcount`, `aria-colcount`, `aria-rowindex`, `aria-colindex`

| Tecla | Acción |
|-------|--------|
| `↑` / `↓` / `←` / `→` | Mueve entre celdas |
| `Home` / `End` | Primera / última celda de la fila |
| `Ctrl + Home` / `Ctrl + End` | Primera / última celda del grid |
| `Page Up` / `Page Down` | Sube / baja una página de filas |
| `Enter` / `F2` | Activa el modo edición en la celda |
| `Escape` | Sale del modo edición |

---

## Slider

**Roles:** `role="slider"`  
**Atributos:** `aria-valuemin`, `aria-valuemax`, `aria-valuenow`, `aria-valuetext`

| Tecla | Acción |
|-------|--------|
| `←` / `↓` | Decrementa el valor |
| `→` / `↑` | Incrementa el valor |
| `Home` | Valor mínimo |
| `End` | Valor máximo |
| `Page Up` / `Page Down` | Incremento / decremento grande |

---

## Carousel / Slider de contenido

**Roles:** `role="region"` con `aria-roledescription="carousel"`, `aria-label`  
**Atributos:** `aria-live="off"` (mientras se controla), `aria-live="polite"` (si es automático)

| Tecla | Acción |
|-------|--------|
| Botón anterior/siguiente | Navegación entre slides (focus en el control) |
| `Tab` | Mueve el foco a los controles y contenido del slide activo |

---

## Breadcrumb

**Roles:** `<nav aria-label="Breadcrumb">` + lista `<ol>` + `<a>`  
**Atributos:** `aria-current="page"` en el último elemento

No requiere interacción de teclado especial — usa navegación nativa de enlaces.

---

## Live Regions (notificaciones dinámicas)

| Tipo | Role / Atributo | Cuándo usar |
|------|-----------------|-------------|
| Alertas urgentes | `role="alert"` o `aria-live="assertive"` | Errores críticos, acciones bloqueantes |
| Actualizaciones suaves | `aria-live="polite"` | Confirmaciones, mensajes de estado |
| Estado | `role="status"` | Loading, contadores, cambios de filtro |
| Log | `role="log"` | Chat, historial de actividad |
| Timer | `role="timer"` | Cuentas atrás |

**Regla:** Preferir `role="alert"` para errores; `aria-live="polite"` para el resto.
