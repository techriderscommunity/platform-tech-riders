---
name: 'aria-accessibility-patterns'
description: 'Guía de implementación profunda para patrones ARIA, roles/estados/propiedades WAI-ARIA 1.2, optimización de lectores de pantalla y navegación por teclado. Basada en especificaciones oficiales W3C.'
---

# Skill: Patrones de Accesibilidad ARIA

Este skill proporciona guía definitiva para implementar **ARIA (Aplicaciones de Internet Ricas y Accesibles)** basada en estándares oficiales W3C.

## Fundamentos y Referencias

### Estándares Oficiales
- **WAI-ARIA 1.2 Specification**: https://www.w3.org/TR/wai-aria-1.2/
- **ARIA Authoring Practices Guide (APG)**: https://www.w3.org/WAI/ARIA/apg/
- **Using ARIA**: https://www.w3.org/WAI/ARIA/apg/practices/
- **WCAG 2.1 Level AA**: https://www.w3.org/WAI/WCAG21/quickref/
- **The Anatomy of Accessible Web Apps**: https://www.a11y-101.com/design/

## Regla ARIA #1: No Usar ARIA

> "Si puedes usar un elemento HTML nativo, úsalo en su lugar."

### ✅ Prefiere HTML Nativo

```typescript
// ✅ DO THIS
<button onClick={handleClick}>Submit</button>
<button disabled>Disabled Button</button>

// ❌ DON'T DO THIS
<div role="button" onClick={handleClick} tabIndex={0}>Submit</div>
<div role="button" onClick={handleClick} aria-disabled="true">Disabled</div>
```

### Elementos Nativos con Accesibilidad Integrada

```typescript
// Button
<button>Click me</button>  // ✅ Built-in: keyboard (Enter/Space), role

// Link
<a href="/page">Link</a>   // ✅ Built-in: keyboard (Enter), role

// Input
<input type="text" />      // ✅ Built-in: keyboard, role, states

// Form
<form>                     // ✅ Built-in: submit on Enter
  <fieldset>               // ✅ Built-in: groups related inputs
    <legend>Group</legend>  // ✅ Built-in: labels fieldset
  </fieldset>
</form>

// Heading
<h1>Page Title</h1>        // ✅ Built-in: semantic, navigation

// Navigation
<nav>                      // ✅ Built-in: landmark region
  <a href="/">Home</a>
</nav>

// Main Content
<main>                     // ✅ Built-in: landmark region
  Content
</main>

// Footer
<footer>                   // ✅ Built-in: landmark region
  Copyright
</footer>

// Emphasis
<strong>Important</strong> // ✅ Built-in: semantic
<em>Emphasis</em>          // ✅ Built-in: semantic
```

## Roles, Estados y Propiedades ARIA

### Categoría 1: Roles de Referencia (Landmarks)

```typescript
// Defines page structure and organization

// application - Custom web app with no semantic HTML equivalent
<div role="application" aria-label="Calendar Application">
  {/* Complex interactive content */}
</div>

// banner - Header/masthead (typically <header>)
<header role="banner">
  Logo, site title
</header>

// complementary - Sidebar content (typically <aside>)
<aside role="complementary" aria-labelledby="sidebar-title">
  Sidebar content
</aside>

// contentinfo - Footer information (typically <footer>)
<footer role="contentinfo">
  Copyright, privacy policy
</footer>

// main - Main content area (typically <main>)
<main role="main">
  Primary content
</main>

// navigation - Navigation links (typically <nav>)
<nav role="navigation" aria-label="Primary Navigation">
  <a href="/">Home</a>
</nav>

// region - Generic region when no semantic element applies
<section role="region" aria-labelledby="section-title">
  Custom content region
</section>

// search - Search functionality
<form role="search" aria-label="Site search">
  <input type="search" placeholder="Search..." />
  <button type="submit">Search</button>
</form>
```

### Categoría 2: Roles de Widget (Componentes Interactivos)

```typescript
// button - Clickable element (prefer <button>)
<div role="button" tabIndex={0} onClick={handleClick} onKeyDown={handleKeyDown}>
  Custom Button
</div>

// checkbox - Toggle on/off (prefer <input type="checkbox">)
<div
  role="checkbox"
  aria-checked={isChecked}
  tabIndex={0}
  onClick={toggle}
  aria-label="Accept terms"
>
  {isChecked ? '✓' : '☐'}
</div>

// radio - Select one from group (prefer <input type="radio">)
<div role="radiogroup" aria-labelledby="group-label">
  <legend id="group-label">Choose one:</legend>
  <div role="radio" aria-checked={selected === 'a'} tabIndex={selected === 'a' ? 0 : -1}>
    Option A
  </div>
</div>

// tab - Tab in tablist
<button role="tab" aria-selected={isActive} aria-controls={`panel-${id}`}>
  Tab Label
</button>

// tabpanel - Content panel for tab
<div role="tabpanel" aria-labelledby={`tab-${id}`} id={`panel-${id}`}>
  Tab content
</div>

// menuitem - Item in menu
<a role="menuitem" href="/option">Menu Option</a>

// dialog - Modal dialog
<div role="dialog" aria-labelledby="title" aria-modal="true">
  <h2 id="title">Dialog Title</h2>
  Dialog content
</div>

// slider - Input range (prefer <input type="range">)
<div
  role="slider"
  aria-valuemin={0}
  aria-valuemax={100}
  aria-valuenow={50}
  aria-valuetext="50%"
  tabIndex={0}
/>
```

### Categoría 3: Roles de Estructura de Documento

```typescript
// article - Self-contained content piece
<article role="article">
  <h2>Blog Post Title</h2>
  Post content
</article>

// document - Static document content
<div role="document" aria-label="PDF Document">
  Document content (read-only)
</div>

// group - Collection of related items
<div role="group" aria-labelledby="group-label">
  <h3 id="group-label">Related Items</h3>
  Items...
</div>

// img - Image (alternative when <img> not suitable)
<div role="img" aria-label="Sunset over mountains" style={{ backgroundImage: 'url(sunset.jpg)' }} />

// list - Collection of items (prefer <ul>, <ol>)
<div role="list">
  <div role="listitem">Item 1</div>
  <div role="listitem">Item 2</div>
</div>

// presentation / none - Remove semantic meaning
<div role="presentation">Visual-only decoration</div>
<div aria-hidden="true">Decorative element</div>
```

### Estados y Propiedades Globales

```typescript
// aria-label - Accessible name (when no text content)
<button aria-label="Close modal">×</button>
<div aria-label="Loading spinner" role="status"></div>

// aria-labelledby - Reference label element(s)
<h2 id="dialog-title">Confirm Action</h2>
<div role="dialog" aria-labelledby="dialog-title">
  Content...
</div>

// aria-describedby - Reference description element(s)
<input
  type="password"
  aria-describedby="pwd-hint"
  placeholder="Password"
/>
<div id="pwd-hint">Must contain uppercase, number, and symbol</div>

// aria-hidden - Hide from accessibility tree (visual-only content)
<span aria-hidden="true">→</span>  // Icon only for seeing users
<svg aria-hidden="true">...</svg>  // Decorative icon

// aria-live - Announce dynamic content updates
<div aria-live="polite" aria-atomic="true">
  {status}  // Updated automatically
</div>

// aria-current - Mark current page/section (navigation)
<nav>
  <a href="/">Home</a>
  <a href="/about" aria-current="page">About</a>  // Current page
  <a href="/contact">Contact</a>
</nav>

// aria-disabled - Custom disabled state (when not using HTML disabled)
<div role="button" aria-disabled="true">Disabled</div>

// aria-expanded - Expanded/collapsed state
<button aria-expanded={isOpen} aria-controls="menu">
  Menu
</button>
<div id="menu" hidden={!isOpen}>Menu items</div>

// aria-haspopup - Element has popup/dropdown
<button aria-haspopup="menu" aria-expanded={isOpen}>
  Options
</button>

// aria-invalid - Invalid/error state
<input type="email" aria-invalid={hasError} aria-describedby="error" />
<span id="error" role="alert">{errorMessage}</span>

// aria-required - Required field indicator
<input type="text" aria-required="true" />

// aria-selected - Selected state (in tablist, listbox, etc.)
<div role="tab" aria-selected={isActive}>Tab</div>

// aria-checked - Checked state for checkbox/radio
<div role="checkbox" aria-checked={isChecked}>Checkbox</div>

// aria-busy - Loading state
<div aria-busy="true">Loading...</div>

// aria-pressed - Pressed/toggled state (toggle buttons)
<button aria-pressed={isPressed} onClick={toggle}>
  Toggle Feature
</button>
```

## Regiones Live y Contenido Dinámico

### Niveles de Prioridad aria-live

```typescript
// aria-live="off" - Don't announce (default, no announcement)
<div aria-live="off">{staticContent}</div>

// aria-live="polite" - Announce when user pauses (after speech stops)
<div aria-live="polite">
  {notification}  // Announced after current speech finishes
</div>

// aria-live="assertive" - Announce immediately (interrupt current speech)
<div aria-live="assertive" role="alert">
  {urgentWarning}  // Announced immediately
</div>
```

### aria-atomic - Anunciar toda la región vs. solo los cambios

```typescript
// Without aria-atomic: Only changed content announced
<div aria-live="polite" aria-atomic="false">
  <p>Item 1</p>
  <p>Item 2</p>
  {/* If Item 3 added: only "Item 3" announced */}
</div>

// With aria-atomic: Entire region announced
<div aria-live="polite" aria-atomic="true">
  {/* Entire updated content announced */}
  Item count: {count}
</div>

// For alerts: use aria-atomic="true" + role="alert"
<div role="alert" aria-atomic="true" aria-live="assertive">
  {errorMessage}  // Entire error announced
</div>
```

## Mejores Prácticas para Lectores de Pantalla

### 1. Skip Links

```typescript
export const SkipLink = () => (
  <a href="#main" className="skip-link">
    Skip to main content
  </a>
);

// Usage
<SkipLink />
<header>Navigation</header>
<main id="main">
  Main content
</main>
```

### 2. Estructura de Encabezados

```typescript
// ✅ Proper heading hierarchy
<h1>Page Title</h1>
<h2>Section 1</h2>
<h3>Subsection</h3>
<h2>Section 2</h2>

// ❌ Skipping levels
<h1>Title</h1>
<h3>Wrong Level (skips h2)</h3>

// Screen readers use heading structure for navigation
```

### 3. Etiquetas de Formulario

```typescript
// ✅ Associated label (best)
<label htmlFor="email">Email:</label>
<input id="email" type="email" />

// ✅ Programmatic label with aria-label
<input type="email" aria-label="Email address" />

// ✅ aria-labelledby for complex labels
<span id="pwd-label">
  Password <span aria-label="required">*</span>
</span>
<input aria-labelledby="pwd-label" type="password" />

// ❌ No label
<input type="email" />  // Screen reader doesn't know purpose
```

### 4. Listas

```typescript
// ✅ Semantic lists
<ul>
  <li>Item 1</li>
  <li>Item 2</li>
</ul>
// Announced as: "List, 2 items"

// ✅ With role for custom markup
<div role="list">
  <div role="listitem">Item 1</div>
  <div role="listitem">Item 2</div>
</div>

// ❌ Divs without list roles
<div>Item 1</div>  // Not announced as list
<div>Item 2</div>
```

### 5. Link vs. Button

```typescript
// ✅ Link - navigates to URL
<a href="/page">Go to Page</a>

// ✅ Button - performs action
<button onClick={handleSubmit}>Submit</button>

// ❌ Link that acts as button
<a href="javascript:void(0)" onClick={handleSubmit}>Submit</a>

// ✅ Button that looks like link (if needed)
<button className="link-style" onClick={handleNavigate}>
  Go to Page
</button>
```

## Navegación por Teclado

### Implementación de Soporte de Teclado

```typescript
// Tab Navigation
<button>Button 1</button>
<button>Button 2</button>
{/* Tab moves between buttons, Shift+Tab reverses */}

// Arrow Keys (for lists, tabs, menus)
<div role="tablist">
  <button role="tab" onKeyDown={handleTabKeydown}>Tab 1</button>
  <button role="tab" onKeyDown={handleTabKeydown}>Tab 2</button>
</div>

const handleTabKeydown = (e: React.KeyboardEvent) => {
  if (e.key === 'ArrowRight' || e.key === 'ArrowDown') {
    e.preventDefault();
    focusNextTab();
  } else if (e.key === 'ArrowLeft' || e.key === 'ArrowUp') {
    e.preventDefault();
    focusPreviousTab();
  } else if (e.key === 'Home') {
    e.preventDefault();
    focusFirstTab();
  } else if (e.key === 'End') {
    e.preventDefault();
    focusLastTab();
  }
};

// Enter & Space (for activating)
<button
  onKeyDown={(e) => {
    if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      handleClick();
    }
  }}
>
  Click me
</button>

// Escape (for closing modals, menus)
<div
  role="dialog"
  onKeyDown={(e) => {
    if (e.key === 'Escape') {
      closeDialog();
    }
  }}
>
  Modal content
</div>
```

## Errores Comunes de ARIA a Evitar

```typescript
// ❌ MISTAKE 1: Using role when semantic HTML exists
<div role="button">Click</div>
// ✅ DO THIS INSTEAD
<button>Click</button>

// ❌ MISTAKE 2: aria-label on interactive elements without content
<button aria-label="Submit">
  {/* No content - confusing */}
</button>
// ✅ DO THIS INSTEAD
<button>Submit</button>

// ❌ MISTAKE 3: Hidden content marked as aria-hidden
<input type="text" aria-hidden="true" />
// ✅ DO THIS INSTEAD - only hide visual decoration
<span aria-hidden="true">→</span>

// ❌ MISTAKE 4: Incorrect heading levels
<h1>Title</h1>
<h3>Section</h3>  // Should be h2
// ✅ DO THIS INSTEAD
<h1>Title</h1>
<h2>Section</h2>

// ❌ MISTAKE 5: aria-label on images (use alt)
<img src="photo.jpg" aria-label="Photo" />
// ✅ DO THIS INSTEAD
<img src="photo.jpg" alt="Photo" />

// ❌ MISTAKE 6: Focus management not handled
<div role="dialog">Content</div>
// ✅ DO THIS INSTEAD - trap focus
useEffect(() => {
  focusFirstElement();
  return () => returnFocusToTrigger();
}, []);

// ❌ MISTAKE 7: Color alone to convey information
<div style={{ color: 'red' }}>Error</div>
// ✅ DO THIS INSTEAD
<div className="error">
  <span aria-hidden="true">⚠️</span>
  Error message text
</div>
```

## Testing ARIA Implementation

### Manual Testing Checklist

- [ ] Test with keyboard only (Tab, Shift+Tab, Arrow keys, Enter, Escape)
- [ ] Test with screen reader (NVDA, JAWS, VoiceOver)
- [ ] Check ARIA roles correctly applied
- [ ] Verify labels/names for all interactive elements
- [ ] Test state changes announced (aria-expanded, aria-selected, etc.)
- [ ] Verify focus management
- [ ] Check heading structure
- [ ] Test form labels and error messages
- [ ] Verify semantic HTML used when possible
- [ ] Test with axe DevTools browser extension

### Automated Testing

```typescript
import { render } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';

test('component is accessible', async () => {
  const { container } = render(<MyComponent />);
  const results = await axe(container);
  expect(results).toHaveNoViolations();
});
```

## Resources

- **WAI-ARIA 1.2**: https://www.w3.org/TR/wai-aria-1.2/
- **ARIA APG**: https://www.w3.org/WAI/ARIA/apg/
- **Using ARIA**: https://www.w3.org/WAI/ARIA/apg/practices/
- **WCAG 2.1**: https://www.w3.org/WAI/WCAG21/quickref/
- **Inclusive Components**: https://inclusive-components.design/
- **a11y-101**: https://www.a11y-101.com/

## Related Skills

- `design-patterns-library` - ARIA patterns in context
- `ux-audit` - Verify ARIA compliance
- `aria-accessibility-patterns` - Deep ARIA knowledge
- `screenshot-reporter` - Accessibility visualization

