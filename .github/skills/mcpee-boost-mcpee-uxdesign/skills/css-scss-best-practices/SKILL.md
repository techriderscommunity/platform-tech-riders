---
name: css-scss-best-practices
description: 'Domina la organización CSS y SCSS, patrones de arquitectura (BEM, SMACSS), design tokens, mixins, funciones y diseño responsivo. Incluye mejores prácticas de MDN y documentación Sass con patrones listos para producción.'
---

# Mejores Prácticas CSS y SCSS

Construye hojas de estilo mantenibles y escalables con patrones estándar de la industria. Este skill cubre fundamentos CSS, características avanzadas de SCSS y enfoques arquitectónicos siguiendo https://sass-lang.com/documentation y https://developer.mozilla.org/en-US/docs/Web/CSS.

## Fundamentos CSS

### 1. **Cascada CSS y Especificidad**

**Entendiendo la Cascada:**
```css
/* Good: Lower specificity first, then build up */
button {
  padding: 8px 16px;
  border-radius: 4px;
}

button:hover {
  background-color: var(--color-primary-hover);
}

button.btn--large {
  padding: 12px 24px;
  font-size: 18px;
}

button:disabled {
  opacity: 0.5;
}
```

**Jerarquía de Especificidad:**
```
Inline styles        1000
IDs (#id)            100
Classes (.class)     10
Elements (div)       1
```

**Mejores Prácticas:**
```css
/* ✅ Good: Use classes for styling, low specificity */
.button { }
.button:hover { }
.button--primary { }

/* ❌ Bad: High specificity, harder to override */
div#main .container button.btn { }
button#submit { }
```

### 2. **Variables CSS (Custom Properties)**

**Definición y Uso:**
```css
:root {
  /* Colors */
  --color-primary: #0066cc;
  --color-on-primary: #ffffff;

  /* Spacing (8px grid) */
  --spacing-xs: 4px;
  --spacing-sm: 8px;
  --spacing-md: 16px;
  --spacing-lg: 24px;
  --spacing-xl: 32px;

  /* Typography */
  --font-size-sm: 14px;
  --font-size-base: 16px;
  --font-size-lg: 18px;

  --line-height-tight: 1.2;
  --line-height-normal: 1.5;

  /* Shadows */
  --shadow-sm: 0 1px 2px rgba(0, 0, 0, 0.05);
  --shadow-md: 0 4px 6px rgba(0, 0, 0, 0.1);
  --shadow-lg: 0 10px 15px rgba(0, 0, 0, 0.1);

  /* Border Radius */
  --radius-sm: 4px;
  --radius-md: 8px;
  --radius-lg: 12px;
}

/* Dark mode theme */
[data-theme="dark"] {
  --color-primary: #3399ff;
  --color-on-primary: #000000;
}

/* Component usage */
.button {
  padding: var(--spacing-sm) var(--spacing-md);
  border-radius: var(--radius-md);
  font-size: var(--font-size-base);
  line-height: var(--line-height-normal);
  box-shadow: var(--shadow-md);
}
```

**Valores de Fallback:**
```css
.button {
  /* Fallback if variable not supported (old browsers) */
  padding: 8px 16px;
  padding: var(--spacing-md, 16px);
}
```

### 3. **Patrones de Diseño Responsivo**

**Enfoque Mobile-First (Recomendado):**
```css
/* Mobile (small screens) - default */
.container {
  padding: var(--spacing-md);
  font-size: var(--font-size-base);
  grid-template-columns: 1fr;
}

/* Tablet */
@media (min-width: 640px) {
  .container {
    grid-template-columns: 1fr 1fr;
  }
}

/* Desktop */
@media (min-width: 1024px) {
  .container {
    grid-template-columns: 1fr 1fr 1fr;
    padding: var(--spacing-lg);
  }
}
```

**Breakpoints (Estándar):**
```css
/* Common breakpoints */
@media (min-width: 320px)  { } /* xs: Mobile */
@media (min-width: 640px)  { } /* sm: Small tablet */
@media (min-width: 1024px) { } /* md: Large tablet */
@media (min-width: 1280px) { } /* lg: Desktop */
@media (min-width: 1536px) { } /* xl: Large desktop */
```

**Consultas de Orientación y Características:**
```css
/* Dark mode preference */
@media (prefers-color-scheme: dark) {
  :root {
    --color-bg: #1a1a1a;
    --color-text: #ffffff;
  }
}

/* Reduced motion preference */
@media (prefers-reduced-motion: reduce) {
  * {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}

/* Touch devices */
@media (hover: none) and (pointer: coarse) {
  button {
    min-height: 48px; /* Touch target */
  }
}
```

## Características SCSS

### 1. **Variables y Anidamiento**

```scss
// Variables
$primary-color: #0066cc;
$spacing-unit: 8px;
$base-font-size: 16px;

// Nesting
.button {
  padding: $spacing-unit * 2 $spacing-unit * 3;
  border-radius: 4px;

  &:hover {
    background-color: darken($primary-color, 10%);
  }

  &:disabled {
    opacity: 0.5;
  }

  &--primary {
    background-color: $primary-color;
    color: white;
  }

  &--secondary {
    background-color: lighten($primary-color, 20%);
  }
}
```

### 2. **Mixins (Bloques Reutilizables)**

```scss
// Mixin with parameters
@mixin flex-center($direction: row) {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: $direction;
}

@mixin button-variant($bg-color, $text-color) {
  background-color: $bg-color;
  color: $text-color;

  &:hover:not(:disabled) {
    background-color: darken($bg-color, 10%);
  }
}

@mixin responsive-font($mobile, $desktop) {
  font-size: $mobile;

  @media (min-width: 1024px) {
    font-size: $desktop;
  }
}

@mixin focus-visible {
  &:focus-visible {
    outline: 2px solid var(--color-focus);
    outline-offset: 2px;
  }
}

// Usage
.button {
  @include flex-center(row);
  @include button-variant(#0066cc, white);
  @include focus-visible;
  @include responsive-font(14px, 16px);
}
```

### 3. **Funciones**

```scss
// Color functions
@function get-contrast-color($bg-color) {
  @if (lightness($bg-color) > 50%) {
    @return #000000;
  } @else {
    @return #ffffff;
  }
}

// Spacing function
@function spacing($multiplier: 1) {
  @return 8px * $multiplier;
}

// Usage
.button {
  padding: spacing(1) spacing(2);
  background-color: #0066cc;
  color: get-contrast-color(#0066cc); // #ffffff
}
```

### 4. **Extends (Herencia)**

```scss
// Base placeholder
%button-base {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-height: 44px;
  padding: 8px 16px;
  border-radius: 4px;
  border: none;
  cursor: pointer;
  transition: all 200ms ease;
  font-weight: 500;
}

%form-element {
  font-size: 16px;
  font-family: inherit;
  border: 1px solid var(--color-border);
}

// Usage
.button {
  @extend %button-base;
  @extend %form-element;
  background-color: var(--color-primary);
}

.link-button {
  @extend %button-base;
  background-color: transparent;
  text-decoration: underline;
}
```

## Architecture Patterns

### 1. **BEM (Block Element Modifier)**

**Naming Convention:**
```scss
.block { }
.block__element { }
.block--modifier { }
.block__element--modifier { }

// Example
.button { }
.button__icon { }
.button__text { }
.button--primary { }
.button--large { }
.button__icon--large { }
```

**Implementation:**
```scss
.card {
  padding: var(--spacing-md);
  border-radius: var(--radius-md);
  box-shadow: var(--shadow-md);

  &__header {
    margin-bottom: var(--spacing-md);
    border-bottom: 1px solid var(--color-border);
  }

  &__title {
    font-size: 20px;
    font-weight: 600;
  }

  &__content {
    margin: var(--spacing-md) 0;
  }

  &--highlighted {
    border: 2px solid var(--color-primary);
  }

  &--compact {
    padding: var(--spacing-sm);
  }
}

// HTML
<div class="card card--highlighted">
  <div class="card__header">
    <h2 class="card__title">Title</h2>
  </div>
  <div class="card__content">Content</div>
</div>
```

### 2. **SMACSS (Scalable and Modular Architecture)**

**Structure:**
```
styles/
├── base/           # HTML elements, resets
├── layout/         # Major layout components
├── modules/        # Reusable components
├── state/          # State classes (.is-active, .is-hidden)
├── theme/          # Theme variables
└── utilities/      # Utility classes (.u-hide, .u-center)
```

**File Organization:**
```scss
// styles/base/_reset.scss
* {
  box-sizing: border-box;
}

body {
  margin: 0;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto;
}

// styles/theme/_variables.scss
:root {
  --color-primary: #0066cc;
  --spacing-md: 16px;
}

// styles/modules/_button.scss
.button {
  display: inline-flex;
  padding: var(--spacing-sm) var(--spacing-md);
}

// styles/state/_states.scss
.is-active {
  border-bottom: 2px solid var(--color-primary);
}

.is-hidden {
  display: none;
}

.is-disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

// styles/utilities/_utilities.scss
.u-center {
  text-align: center;
}

.u-flex-center {
  display: flex;
  align-items: center;
  justify-content: center;
}
```

### 3. **Design Tokens System**

```scss
// styles/tokens/_colors.scss
$colors: (
  'primary': #0066cc,
  'secondary': #f0f0f0,
  'success': #28a745,
  'danger': #dc3545,
  'warning': #ffc107,
);

@function color($name) {
  @return map-get($colors, $name);
}

// styles/tokens/_spacing.scss
$spacing-unit: 8px;

$spacing: (
  'xs': $spacing-unit * 0.5,
  'sm': $spacing-unit,
  'md': $spacing-unit * 2,
  'lg': $spacing-unit * 3,
  'xl': $spacing-unit * 4,
);

@function spacing($size) {
  @return map-get($spacing, $size);
}

// styles/tokens/_typography.scss
$typography: (
  'xs': (
    'size': 12px,
    'weight': 400,
    'line-height': 1.4,
  ),
  'sm': (
    'size': 14px,
    'weight': 400,
    'line-height': 1.5,
  ),
  'base': (
    'size': 16px,
    'weight': 400,
    'line-height': 1.5,
  ),
);

@mixin typography($level) {
  $config: map-get($typography, $level);
  font-size: map-get($config, 'size');
  font-weight: map-get($config, 'weight');
  line-height: map-get($config, 'line-height');
}

// Usage
.button {
  @include typography('base');
  color: color('primary');
  padding: spacing('sm') spacing('md');
}
```

## Performance Best Practices

### 1. **Minimize CSS Overhead**

```scss
// ❌ Bad: Excessive nesting creates verbose selectors
.page {
  .container {
    .row {
      .column {
        .card {
          .title {
            font-size: 18px;
          }
        }
      }
    }
  }
}

// ✅ Good: Flat, maintainable selectors
.card__title {
  font-size: 18px;
}
```

### 2. **Avoid Over-Engineering**

```scss
// ❌ Bad: Too complex
@mixin button-state($size, $color, $weight, $padding) {
  font-size: $size;
  color: $color;
  font-weight: $weight;
  padding: $padding;
}

// ✅ Good: Simple, focused
@mixin button-variant($color) {
  background-color: $color;

  &:hover {
    background-color: darken($color, 10%);
  }
}
```

### 3. **PurgeCSS/Tree-Shaking**

```scss
// Mark as unused if not found in templates
/* purgecss start ignore */
.legacy-class {
  color: red;
}
/* purgecss end ignore */
```

## Accessibility in CSS

**Color Contrast:**
```scss
// Ensure 4.5:1 ratio for normal text, 3:1 for large
$text-color: #000000; // dark
$bg-color: #ffffff;   // light

// Meets WCAG AA (4.5:1)
// Verify at: https://webaim.org/resources/contrastchecker/
```

**Focus Indicators:**
```scss
@mixin focus-visible {
  outline: 2px solid var(--color-focus);
  outline-offset: 2px;

  // Remove default focus outline
  &:focus-visible {
    outline: 2px solid var(--color-focus);
  }
}

button {
  @include focus-visible;
}
```

**Reduced Motion:**
```scss
@media (prefers-reduced-motion: reduce) {
  * {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}
```

## References

- [MDN CSS Reference](https://developer.mozilla.org/en-US/docs/Web/CSS)
- [Sass Documentation](https://sass-lang.com/documentation)
- [BEM Methodology](https://getbem.com/)
- [SMACSS Architecture](http://smacss.com/)
- [CSS Custom Properties](https://developer.mozilla.org/en-US/docs/Web/CSS/--*)
- [WCAG Color Contrast](https://www.w3.org/WAI/WCAG21/Understanding/contrast-minimum.html)

## Templates Available

- `button.scss` - Complete button with all patterns
- `design-tokens.scss` - Token management system
- `mixins.scss` - Reusable mixin library
- `bem-structure.scss` - BEM methodology example
- `responsive-grid.scss` - Mobile-first grid system
