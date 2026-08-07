# WCAG 2.1 AA — Checklist de Auditoría

> Referencia operativa para usar durante una auditoría UX/UI con la skill `@ux-audit`.  
> Marca cada ítem como ✅ (pasa), ❌ (falla) o ⚠️ (revisar).

---

## 1. Perceptible

### 1.1 Alternativas de texto
- [ ] Todas las imágenes informativas tienen `alt` descriptivo
- [ ] Las imágenes decorativas tienen `alt=""` o `role="presentation"`
- [ ] Los iconos funcionales tienen `aria-label` o texto visible
- [ ] Los vídeos tienen subtítulos sincronizados
- [ ] Los audios tienen transcripción

### 1.2 Contraste de color
- [ ] Texto normal (< 18pt / < 14pt bold): ratio ≥ **4.5:1**
- [ ] Texto grande (≥ 18pt / ≥ 14pt bold): ratio ≥ **3:1**
- [ ] Componentes UI (bordes de input, iconos): ratio ≥ **3:1** contra fondo
- [ ] Estados focus, hover, active: ratio suficiente en todos los estados

### 1.3 Información no solo por color
- [ ] Los errores no se comunican únicamente con color rojo
- [ ] Los estados (éxito/error/warning) usan color + icono + texto
- [ ] Los gráficos y tablas no dependen solo del color para transmitir datos

### 1.4 Contenido redimensionable
- [ ] El texto se puede ampliar al 200% sin pérdida de contenido
- [ ] No hay texto en imágenes (salvo logos)
- [ ] El viewport no está bloqueado (`user-scalable=no` no permitido)

---

## 2. Operable

### 2.1 Teclado
- [ ] Todos los elementos interactivos son alcanzables con Tab
- [ ] El orden de Tab es lógico y coherente con el flujo visual
- [ ] No hay trampas de foco (salvo modales con focus trap intencional)
- [ ] Las acciones de ratón tienen equivalente de teclado
- [ ] Los shortcuts de teclado no sobreescriben los del sistema operativo

### 2.2 Foco visible
- [ ] El indicador de foco es claramente visible en todos los elementos
- [ ] Outline ≥ 2px, con offset, color de contraste suficiente
- [ ] No se usa `outline: none` sin reemplazo visual de foco

### 2.3 Tiempo suficiente
- [ ] Los timeouts se pueden desactivar, ajustar o extender
- [ ] Las animaciones automáticas se pueden pausar
- [ ] No hay contenido que parpadee más de 3 veces/segundo

### 2.4 Navegación
- [ ] La página tiene un `<title>` descriptivo y único
- [ ] Existe un enlace "Saltar al contenido principal" (skip link)
- [ ] Los headings forman una jerarquía lógica (H1 → H2 → H3)
- [ ] Los enlaces tienen texto descriptivo (no "clic aquí", "más info")
- [ ] Los breadcrumbs reflejan la posición en la jerarquía de navegación

---

## 3. Comprensible

### 3.1 Legibilidad
- [ ] El idioma principal de la página está definido (`lang="es"`)
- [ ] Los cambios de idioma inline están marcados (`lang="en"`)
- [ ] El nivel de lectura es adecuado para la audiencia

### 3.2 Predecibilidad
- [ ] El foco no provoca cambios de contexto inesperados
- [ ] Cambiar un input no envía formulario automáticamente
- [ ] La navegación es consistente entre páginas
- [ ] Los componentes similares tienen el mismo nombre en toda la aplicación

### 3.3 Asistencia de entrada
- [ ] Los errores de formulario se identifican claramente con texto
- [ ] Las etiquetas (`<label>`) están asociadas a cada input
- [ ] Los hints/instrucciones están disponibles antes de necesitarlos
- [ ] Para errores: se indica qué campo falla y cómo corregirlo
- [ ] Para acciones destructivas: se pide confirmación

---

## 4. Robusto

### 4.1 Compatibilidad
- [ ] El HTML es válido (sin atributos duplicados, elementos mal cerrados)
- [ ] Los roles ARIA son válidos y usados correctamente
- [ ] Los atributos ARIA requeridos están presentes (`aria-expanded`, `aria-selected`…)
- [ ] Los componentes custom tienen `name`, `role` y `value` accesibles
- [ ] El estado del componente se comunica al cambiar (`aria-live`, `aria-expanded`)

---

## 5. Componentes críticos — Checklist rápida

### Botones
- [ ] `<button>` nativo (no `<div role="button">`)
- [ ] `aria-label` si el texto visible no es suficiente
- [ ] Estado desactivado: `disabled` o `aria-disabled`
- [ ] Tamaño ≥ 44×44px en mobile

### Formularios
- [ ] Cada input tiene `<label>` asociado (`for`/`id` o aria-labelledby)
- [ ] Campos obligatorios marcados con `required` o `aria-required`
- [ ] Errores asociados con `aria-describedby`
- [ ] Grouping con `<fieldset>` + `<legend>`

### Modales / Dialogs
- [ ] `role="dialog"` con `aria-labelledby` y `aria-modal="true"`
- [ ] Focus trap activo mientras el modal está abierto
- [ ] Escape cierra el modal
- [ ] El foco vuelve al elemento que abrió el modal al cerrarlo

### Tablas
- [ ] `<caption>` descriptivo
- [ ] Cabeceras con `<th scope="col|row">`
- [ ] No usar tablas para layout

### Navegación
- [ ] `<nav>` con `aria-label` si hay más de una en la página
- [ ] `aria-current="page"` en el enlace activo

---

## Herramientas recomendadas

| Herramienta | Tipo | URL |
|-------------|------|-----|
| axe DevTools | Extensión navegador | https://www.deque.com/axe/ |
| WAVE | Extensión navegador | https://wave.webaim.org/ |
| Colour Contrast Analyser | Desktop | https://www.tpgi.com/color-contrast-checker/ |
| NVDA | Lector de pantalla (Windows) | https://www.nvaccess.org/ |
| VoiceOver | Lector de pantalla (macOS/iOS) | Integrado en macOS |
| Lighthouse | Integrado en DevTools | chrome://inspect |
