---
name: 'screenshot-reporter'
description: 'Genera informes visuales usando capturas de pantalla y Playwright. Crea informes de auditoría de diseño, galerías de componentes, documentación de diseño responsivo y visualizaciones de violaciones de accesibilidad.'
---

# Skill de Reporter de Capturas de Pantalla

Este skill te ayuda a documentar y visualizar tu UI a través de capturas de pantalla e informes visuales.

## Capacidades

### 1. Generación Automatizada de Capturas
- Capturas de página completa
- Capturas de componentes
- Capturas por breakpoints responsivos
- Variantes de modo oscuro
- Capturas de estados interactivos

### 2. Generación de Informes Visuales
- Informes visuales de auditoría de diseño
- Galería de componentes con capturas
- Muestra de diseño responsivo
- Comparaciones antes/después
- Resaltado de violaciones de accesibilidad

### 3. Documentación Responsiva
- Capturas multi-viewport (móvil, tablet, escritorio)
- Documentación de comportamiento responsivo
- Validación de breakpoints
- Detección de cambios de layout

### 4. Muestras de Componentes
- Catálogo visual de componentes
- Variaciones de estado de componentes (default, hover, active, disabled)
- Ejemplos de composición de componentes
- Documentación de uso con visuales

### 5. Visualización de Accesibilidad
- Resaltar problemas de contraste
- Mostrar indicadores de foco
- Visualizar etiquetas ARIA
- Marcar rutas de navegación por teclado

## Requisitos Previos

```bash
npm install -D playwright @playwright/test
```

## Configuración

Crea `playwright.config.ts`:

```typescript
import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './e2e',
  use: {
    baseURL: 'http://localhost:3000',
    screenshot: 'only-on-failure',
  },
  webServer: {
    command: 'npm run dev',
    port: 3000,
  },
  projects: [
    { name: 'chromium', use: { ...devices['Desktop Chrome'] } },
    { name: 'mobile', use: { ...devices['Pixel 5'] } },
  ],
});
```

## Ejemplos de Uso

### Captura Básica

```typescript
import { test } from '@playwright/test';

test('capture component', async ({ page }) => {
  await page.goto('/components/button');
  await page.screenshot({ path: 'screenshots/button.png' });
});
```

### Capturas Responsivas

```typescript
const viewports = {
  mobile: { width: 375, height: 667 },
  tablet: { width: 768, height: 1024 },
  desktop: { width: 1440, height: 900 },
};

for (const [name, viewport] of Object.entries(viewports)) {
  await page.setViewportSize(viewport);
  await page.screenshot({ path: `screenshots/${name}.png` });
}
```

### Galería de Componentes

```typescript
// Generate screenshots of all component states
async function generateComponentGallery() {
  const states = ['default', 'hover', 'active', 'disabled'];

  for (const state of states) {
    await page.goto(`/components/button?state=${state}`);
    await page.screenshot({
      path: `gallery/button-${state}.png`
    });
  }
}
```

### Informe de Accesibilidad

```typescript
// Visual accessibility report
async function generateA11yReport() {
  const page = await browser.newPage();

  // Highlight focus indicators
  await page.addInitScript(() => {
    const style = document.createElement('style');
    style.textContent = `
      *:focus-visible {
        outline: 3px solid #FF0000 !important;
      }
    `;
    document.head.appendChild(style);
  });

  await page.goto('/components');
  await page.keyboard.press('Tab'); // Show focus
  await page.screenshot({ path: 'a11y-report.png' });
}
```

## Output Artifacts

### Screenshots
- `screenshots/` - Individual component screenshots
- `gallery/` - Component state variations
- `responsive/` - Multi-viewport showcases
- `a11y/` - Accessibility violation highlights

### Reports
- `design-audit-report.html` - Visual audit with findings
- `component-gallery.html` - Interactive component showcase
- `responsive-report.html` - Responsive design documentation
- `a11y-report.html` - Accessibility violations

### Report Format

```html
<!DOCTYPE html>
<html>
<head>
  <title>Design Audit Report</title>
  <style>
    .component-screenshot {
      max-width: 100%;
      margin: 20px 0;
      border: 1px solid #ddd;
    }
    .issue { color: #d32f2f; }
    .success { color: #388e3c; }
  </style>
</head>
<body>
  <h1>Design Audit Report</h1>
  <section>
    <h2>Component: Button</h2>
    <img src="button.png" class="component-screenshot" alt="Button component">
    <p class="issue">⚠️ Color contrast insufficient in dark mode</p>
  </section>
</body>
</html>
```

## Viewer Options

```typescript
// Full page
await page.screenshot({
  path: 'full-page.png',
  fullPage: true
});

// Specific element
const element = await page.locator('.component');
await element.screenshot({ path: 'component.png' });

// Omit fixed elements
await page.screenshot({
  path: 'screenshot.png',
  mask: [await page.locator('header')]
});
```

## Integration

### CI/CD Pipeline

```yaml
# .github/workflows/screenshots.yml
name: Generate Screenshots
on: [push]

jobs:
  screenshots:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-node@v3
      - run: npm ci
      - run: npm run build
      - run: npm run screenshots:generate
      - uses: actions/upload-artifact@v3
        with:
          name: screenshots
          path: screenshots/
```

### Documentation Site

```bash
# Generate automated documentation
npm run docs:generate

# Serves visual component library
npm run docs:serve
```

## Best Practices

1. **Consistent Breakpoints**: Use standard breakpoints
2. **Consistent Styling**: Use same base styles across screenshots
3. **Clear Naming**: Use descriptive screenshot names
4. **Regular Updates**: Regenerate regularly
5. **Baseline Tests**: Use for visual regression testing
6. **Git Tracking**: Version control key screenshots

## Related Skills

- `component-inventory` - Generate for cataloged components
- `ux-audit` - Create visual audit reports
- `design-system-generator` - Document design systems visually
- `figma-integration` - Compare with design system

## Advanced Usage

### Visual Diff
```bash
npm run screenshots:diff baseline/button.png current/button.png
```

### Performance Metrics
```typescript
const metrics = await page.evaluate(() => {
  return {
    LCP: performance.getEntriesByType('largest-contentful-paint'),
    FID: performance.getEntriesByType('first-input'),
  };
});
```

### Accessibility Scanner Integration
```typescript
import { injectAxe, checkA11y } from 'axe-playwright';

await injectAxe(page);
await checkA11y(page, null, {}, 'v4.4');
```
