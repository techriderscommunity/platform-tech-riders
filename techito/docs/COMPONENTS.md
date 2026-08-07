# 🎨 TechRiders Component Library Guide

**Versión**: 1.0  
**Última actualización**: May 24, 2026

---

## Principios

✅ **Centralizado** — Todos los componentes definidos en `_components.scss`  
✅ **Reutilizable** — Importar y usar clases, no duplicar estilos  
✅ **Design-Token basado** — Nunca hardcodear colores, sombras, espacios, etc.  
✅ **Consistencia** — Mismo look & feel en toda la app  
✅ **Mantenible** — Un cambio = actualizar una sola clase  

---

## Componentes Disponibles

### 1. CARDS

```html
<!-- Card básica -->
<div class="card">
  <div class="card-header">
    <h2>Título</h2>
  </div>
  <div class="card-body">
    Contenido aquí
  </div>
  <div class="card-footer">
    Footer opcional
  </div>
</div>

<!-- Card elevada (hover effect) -->
<div class="card-elevated">
  Contenido...
</div>

<!-- Card secundaria (bg más oscuro) -->
<div class="card-secondary">
  Contenido...
</div>
```

### 2. BUTTONS

```html
<!-- Primary -->
<button class="btn btn-primary">Click me</button>

<!-- Secondary -->
<button class="btn btn-secondary">Secondary</button>

<!-- Outline -->
<button class="btn btn-outline">Outline</button>

<!-- Sizes -->
<button class="btn btn-sm">Small</button>
<button class="btn">Default</button>
<button class="btn btn-lg">Large</button>

<!-- Disabled -->
<button class="btn btn-primary" disabled>Disabled</button>
```

### 3. INPUTS & FORMS

```html
<!-- Input field -->
<div class="input-field">
  <label for="email">Email</label>
  <input type="email" id="email" placeholder="tu@email.com">
</div>

<!-- Small input -->
<div class="input-field input-sm">
  <label>Pequeño</label>
  <input type="text">
</div>

<!-- Textarea -->
<div class="input-field">
  <label>Mensaje</label>
  <textarea placeholder="Escribe aquí..."></textarea>
</div>

<!-- Select -->
<div class="input-field">
  <label>Opción</label>
  <select>
    <option>Opción 1</option>
    <option>Opción 2</option>
  </select>
</div>
```

### 4. BADGES

```html
<span class="badge badge-info">Info</span>
<span class="badge badge-success">Success</span>
<span class="badge badge-warning">Warning</span>
<span class="badge badge-error">Error</span>
```

### 5. TABLES

```html
<div class="table-responsive">
  <table class="table">
    <thead>
      <tr>
        <th>Header 1</th>
        <th>Header 2</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Dato 1</td>
        <td>Dato 2</td>
      </tr>
    </tbody>
  </table>
</div>
```

### 6. GRIDS

```html
<!-- Auto-fit (responde automáticamente) -->
<div class="grid-auto-fit">
  <div class="card">Card 1</div>
  <div class="card">Card 2</div>
  <div class="card">Card 3</div>
</div>

<!-- Grid de 2 columnas -->
<div class="grid-2">
  <div class="card">Col 1</div>
  <div class="card">Col 2</div>
</div>

<!-- Grid de 3 columnas -->
<div class="grid-3">
  <div class="card">Col 1</div>
  <div class="card">Col 2</div>
  <div class="card">Col 3</div>
</div>
```

### 7. CAROUSEL

```html
<div class="carousel">
  <button class="carousel-btn">❮</button>
  <div class="carousel-track">
    <div class="carousel-item">
      <img src="image1.jpg" alt="">
    </div>
    <div class="carousel-item">
      <img src="image2.jpg" alt="">
    </div>
  </div>
  <button class="carousel-btn">❯</button>
</div>
```

### 8. SECTION HEADERS

```html
<div class="section-header">
  <span class="section-label">NUEVA SECCIÓN</span>
  <h2 class="section-heading">Título Grande</h2>
  <p class="section-subheading">Descripción o subtítulo aquí</p>
</div>
```

### 9. STAT CARDS

```html
<div class="stat-card">
  <span class="stat-icon">📊</span>
  <div class="stat-value">1,234</div>
  <div class="stat-label">Total Usuarios</div>
</div>
```

### 10. FEATURE CARDS

```html
<div class="feature-card">
  <span class="feature-icon">✨</span>
  <h3 class="feature-title">Característica</h3>
  <p class="feature-desc">Descripción de la característica</p>
</div>
```

### 11. BADGES / TAGS / CHIPS

```html
<span class="tag">Tag 1</span>
<span class="tag active">Tag Activo</span>
<span class="chip">Skill Tag</span>
```

### 12. PAGINATION

```html
<div class="pagination">
  <button class="pagination-btn">❮</button>
  <button class="pagination-btn active">1</button>
  <button class="pagination-btn">2</button>
  <button class="pagination-btn">3</button>
  <button class="pagination-btn">❯</button>
</div>
```

### 13. PROGRESS BAR

```html
<div class="progress">
  <div class="progress-fill" style="width: 65%"></div>
</div>
```

### 14. EMPTY STATES

```html
<div class="empty-state">
  <span class="icon">📭</span>
  <h3>No hay datos</h3>
  <p>Aquí aparecerían los datos cuando existan</p>
</div>
```

### 15. ALERTS / TOASTS

```html
<div class="alert alert-info">
  Este es un mensaje de información
</div>

<div class="alert alert-success">
  Operación completada exitosamente
</div>

<div class="alert alert-warning">
  Advertencia: verifica esto
</div>

<div class="alert alert-error">
  Error: algo salió mal
</div>
```

---

## Cómo Refactorizar un Componente Existente

### ANTES (sin usar componentes):

```scss
// admin-dashboard.scss - DUPLICADO
.stat-card {
  background: white;
  border-radius: 0.75rem;
  padding: 1.5rem;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.5);
  border: 1px solid rgba(0, 174, 239, 0.14);
  text-align: center;
  transition: all 0.2s ease;
  
  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 40px rgba(0, 0, 0, 0.6);
  }
}
```

### DESPUÉS (usando componentes):

```scss
// admin-dashboard.scss - LIMPIO
// Solo importa los componentes y usa las clases
// No necesita redefinir .stat-card

// En el HTML:
<div class="stat-card">
  <span class="stat-icon">📊</span>
  <div class="stat-value">42</div>
  <div class="stat-label">Usuarios</div>
</div>
```

---

## Cómo Extender un Componente

Si necesitas variaciones específicas, extiende el componente sin duplicar:

```scss
// En tu página SCSS (ej: admin-dashboard.scss)

// ✅ Correcto: Extiende el componente base
.stat-card-special {
  @extend .stat-card;
  border-top: 4px solid var(--tr-blue);
}

// ❌ Incorrecto: No duplicar TODO el CSS
// .stat-card-special {
//   background: white;
//   border-radius: 0.75rem;
//   padding: 1.5rem;
//   box-shadow: ...
// }
```

---

## Variable / Token de Diseño

Todos los componentes usan `design-tokens.scss`. **Nunca hardcodear**:

| ❌ Incorrecto | ✅ Correcto |
|---|---|
| `background: white;` | `background: var(--bg-elevated);` |
| `color: #1976d2;` | `color: var(--tr-blue);` |
| `border: 1px solid #ddd;` | `border: 1px solid var(--border-default);` |
| `box-shadow: 0 2px 10px rgba(...);` | `box-shadow: var(--shadow-md);` |
| `padding: 16px;` | `padding: var(--space-4);` |
| `border-radius: 8px;` | `border-radius: var(--radius-md);` |
| `color: #888;` | `color: var(--text-secondary);` |

---

## Auditoría de Componentes

Para verificar que todo usa componentes centralizados:

1. **Grep por `background: white`** → Reemplazar con `.card`, `.card-elevated`, etc.
2. **Grep por hex colors** (`#fff`, `#000`, etc.) → Usar variables de `design-tokens`
3. **Grep por `.btn` duplicados** → Consolidar a `_components.scss`
4. **Grep por `.card` redefiniciones** → Eliminar y usar clase base

---

## Checklist para Nuevas Páginas

Cuando crees una nueva página/componente:

- [ ] ¿Usé `.card` o `.card-elevated` para contenedores?
- [ ] ¿Usé `.btn btn-primary`, `.btn btn-secondary`, etc. para botones?
- [ ] ¿Usé `.input-field` para formularios?
- [ ] ¿Usé `.badge-*` para estados?
- [ ] ¿Usé `.grid-auto-fit`, `.grid-2`, `.grid-3` para layouts?
- [ ] ¿Usé `.table` para tablas?
- [ ] ¿Todos los estilos vienen de `design-tokens` (vars)?
- [ ] ¿No hay colores hardcodeados (#xxx)?
- [ ] ¿No hay sombras hardcodeadas (box-shadow)`?
- [ ] ¿No hay espacios hardcodeados (padding/margin)?

---

## Beneficios

✅ **Mantenibilidad**: Cambiar color primario = 1 cambio en `design-tokens.scss`  
✅ **Consistencia**: Todas las tarjetas se ven igual  
✅ **Reutilización**: Copy-paste de HTML, no CSS  
✅ **Performance**: Menos CSS duplicado = archivo más pequeño  
✅ **Escalabilidad**: Fácil agregar nuevas páginas  
✅ **Accesibilidad**: Focus states, contraste, etc. garantizados  

---

**¿Preguntas?** Revisar `_components.scss` para implementación completa.
