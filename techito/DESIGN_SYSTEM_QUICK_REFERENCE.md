# TechRiders Design System — Quick Reference

## Using Design Tokens

### Colors
```scss
// Primary
background-color: var(--bg-canvas);      // Page background
color: var(--text-primary);              // Main text
border-color: var(--border-default);     // Default borders

// States
color: var(--accent-success);            // Success (green)
color: var(--accent-error);              // Error (red)
color: var(--accent-warning);            // Warning (orange)
color: var(--accent-info);               // Info (blue)

// Dark mode (default)
// Light mode: @media (prefers-color-scheme: light)
```

### Spacing (8px grid)
```scss
margin: var(--space-4);                  // 16px
padding: var(--space-2) var(--space-4);  // 8px 16px
gap: var(--space-6);                     // 24px

/* Utilities */
.gap-1, .gap-2, .gap-3, .gap-4, .gap-6, .gap-8
.mt-1, .mt-2, .mt-3, .mt-4, .mt-6, .mt-8
.mb-1, .mb-2, .mb-3
```

### Typography
```scss
font-size: var(--font-size-base);        // 16px
font-size: var(--font-size-sm);          // 14px
font-size: var(--font-size-lg);          // 18px
font-weight: var(--font-weight-semibold); // 600

// Scale: xs (12px) → sm (14px) → base (16px) → lg (18px) → 5xl (48px)
```

### Shadows
```scss
box-shadow: var(--shadow-md);            // Medium shadow
box-shadow: var(--shadow-lg);            // Large shadow (hover)
box-shadow: var(--shadow-glow);          // Glow effect (brand blue)
```

---

## Using Component Classes

### Buttons
```html
<button class="btn btn-primary">Primary</button>
<button class="btn btn-secondary">Secondary</button>
<button class="btn btn-outline">Outline</button>
<button class="btn btn-sm">Small</button>
<button class="btn btn-lg">Large (touch-friendly)</button>
<button class="btn is-loading">Loading...</button>
<button class="btn" disabled>Disabled</button>
```

### Forms
```html
<div class="form-group">
  <label for="email">Email</label>
  <input 
    id="email" 
    type="email"
    aria-describedby="email-help"
    aria-required="true"
  />
  <p id="email-help" style="font-size: 0.875rem;">Helper text</p>
</div>

<input class="is-error" aria-invalid="true" />
<p role="alert" class="error-message">Error message</p>

<input class="is-success" />
<p role="status" class="success-message">Success message</p>

<input readonly />
```

### Cards
```html
<div class="card">
  <h3>Card Title</h3>
  <p>Card content with default padding and shadow.</p>
</div>

<div class="card card-compact">
  <p>Compact card for dense layouts</p>
</div>
```

### Badges
```html
<span class="badge badge-info">Info</span>
<span class="badge badge-success">Success</span>
<span class="badge badge-warning">Warning</span>
<span class="badge badge-error">Error</span>
```

### Layouts
```html
<!-- Flex layout -->
<div class="flex gap-4">
  <div style="flex: 1;">Item 1</div>
  <div style="flex: 1;">Item 2</div>
</div>

<!-- Grid layout (responsive) -->
<div class="grid grid-auto-fit">
  <div>Item 1</div>
  <div>Item 2</div>
  <div>Item 3</div>
</div>

<!-- 2-column (1 column on mobile) -->
<div class="grid grid-cols-2">
  <div>Col 1</div>
  <div>Col 2</div>
</div>

<!-- Container (max 1280px, centered) -->
<div class="container">
  <h1>Centered content</h1>
</div>
```

---

## Accessibility Patterns

### Forms
```html
<!-- Always link label to input -->
<label for="password">Password</label>
<input id="password" type="password" />

<!-- Describe errors to screen readers -->
<input 
  aria-invalid="true"
  aria-describedby="pwd-error"
/>
<p id="pwd-error" role="alert">Password too short</p>

<!-- Mark required fields -->
<label for="name">Full Name *</label>
<input 
  id="name"
  aria-required="true"
  required
/>
```

### Buttons
```html
<!-- Clear button names for screen readers -->
<button aria-label="Submit form">Send</button>

<!-- Loading state -->
<button aria-busy="true" disabled>Submitting...</button>
```

### Navigation
```html
<nav aria-label="Main navigation">
  <a href="/" aria-current="page">Home</a>
  <a href="/about">About</a>
</nav>
```

### Modals
```html
<div role="dialog" aria-modal="true" aria-labelledby="title">
  <h2 id="title">Modal Title</h2>
  <p>Modal content</p>
  <button>Close</button>
</div>
```

### Alerts
```html
<!-- Announcement (intrusive) -->
<div role="alert" aria-live="assertive">
  Error: Form submission failed
</div>

<!-- Notification (non-intrusive) -->
<div role="status" aria-live="polite">
  Form saved successfully
</div>
```

---

## States

### Loading
```html
<button class="is-loading">Loading...</button>
<!-- Shows spinner overlay, disables interaction -->
```

### Error
```html
<input class="is-error" />
<p role="alert" class="error-message">Error text</p>
<!-- aria-invalid="true" automatically set by component -->
```

### Success
```html
<input class="is-success" />
<p role="status" class="success-message">Success text</p>
```

### Readonly
```html
<input readonly value="Cannot edit" />
<!-- Disabled appearance, but not form-disabled -->
```

---

## Themes

### Automatic (System Preference)
```html
<!-- HTML respects OS theme automatically -->
<!-- Dark mode: Windows/Mac dark mode enabled -->
<!-- Light mode: Windows/Mac light mode enabled -->
<!-- No CSS changes needed -->
```

### Manual (JavaScript)
```typescript
// Enable light mode
document.documentElement.dataset.theme = 'light';

// Enable dark mode (default)
document.documentElement.dataset.theme = 'dark';

// Use system preference
document.documentElement.dataset.theme = '';
```

---

## Keyboard Navigation

| Key | Action |
|-----|--------|
| Tab | Move to next interactive element |
| Shift+Tab | Move to previous interactive element |
| Enter | Activate button, submit form, follow link |
| Space | Toggle checkbox, activate button |
| Escape | Close modal, menu, dropdown |
| Arrow keys | Navigate menu items, select options |

---

## Screen Reader Testing

### NVDA (Windows)
```bash
# Start NVDA
# Arrow keys navigate
# Enter activates
# Tab moves through links/buttons
```

### Verify Announcements
- Form labels: "Email, text input"
- Buttons: "Submit button"
- Alerts: "Alert: Error message"
- Status: "Status: Form saved"

---

## Color Palette

### Dark Mode (Default)
- Background: #0B1929 (canvas), #1D2D45 (primary), #2A3F5F (secondary)
- Text: #FFFFFF (primary), #C5D3E5 (secondary), #8BA4BC (muted)
- Accent: #00AEEF (blue), #00AA44 (success), #EF4444 (error)

### Light Mode
- Background: #FFFFFF (canvas), #F5F7FA (primary), #EEEFF5 (secondary)
- Text: #0B1929 (primary), #3A5674 (secondary), #5C7A96 (muted)
- Accent: #0066CC (blue), #00AA44 (success), #E53935 (error)

---

## Common Patterns

### Form with Validation
```html
<form>
  <div class="form-group">
    <label for="email">Email</label>
    <input
      id="email"
      type="email"
      aria-describedby="email-help"
      aria-invalid="false"
      class="form-input"
    />
    <p id="email-help" class="help-text">We'll never share your email</p>
  </div>

  <button type="submit" class="btn btn-primary btn-lg">
    Submit
  </button>
</form>
```

### Card Grid
```html
<div class="grid grid-auto-fit gap-4">
  <div class="card">
    <h3>Card 1</h3>
    <p>Content here</p>
  </div>
  <div class="card">
    <h3>Card 2</h3>
    <p>Content here</p>
  </div>
</div>
```

### Alert Message
```html
<div class="container mt-4">
  <div role="alert" aria-live="polite" class="card">
    <strong>✓ Success!</strong> Your form was submitted.
  </div>
</div>
```

---

## Touch Target Sizes (WCAG AAA)

All interactive elements must be **minimum 44px** high:

- ✓ Buttons: 44px height (default)
- ✓ Form inputs: 44px height (default)
- ✓ Links in navigation: 44px height
- ✓ Checkboxes/radio: 44px clickable area

---

## Running Storybook

```bash
# Start Storybook dev server (port 6006)
npm run storybook

# Build static Storybook site
npm run build-storybook
```

Access at: http://localhost:6006

---

## Build & Deploy

```bash
# Development build
npm run build:dev

# Production build
npm run build

# Run tests
npm run test

# E2E tests
npm run e2e
```

---

## Support

For issues or questions about the design system:
1. Check `DESIGN_SYSTEM_REPORT.md` for detailed documentation
2. Review `ACCESSIBILITY.patterns.ts` for ARIA examples
3. Test with axe DevTools for accessibility issues
4. Test with NVDA/JAWS for screen reader compatibility

**Design System**: TechRiders v1.0  
**Framework**: Angular 20 Standalone Components  
**Last Updated**: July 5, 2026
