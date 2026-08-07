---
name: 'storybook-component-documentation'
description: 'Documentación de componentes estándar de la industria con Storybook. Crea galerías interactivas de componentes, testing de regresión visual, auditorías de accesibilidad y documentación viva siguiendo las mejores prácticas de Storybook.'
---

# Skill de Documentación de Componentes con Storybook

Este skill implementa **Storybook** - el estándar de la industria para desarrollo y documentación orientado a componentes.

## Referencias y Fundamentos

- **Official**: https://storybook.js.org/
- **Best Practices**: https://storybook.js.org/docs/react/writing-stories/best-practices
- **Addon Ecosystem**: https://storybook.js.org/addons
- **Testing Integration**: Visual regression, a11y, interaction testing
- **Teams Using**: Airbnb, GitHub, Microsoft, Shopify, etc.

## ¿Qué es Storybook?

Storybook es un **entorno de desarrollo de componentes aislado** que permite:

1. **Aislamiento de Componentes** - Desarrolla componentes en aislamiento sin dependencias
2. **Documentación** - Documentación autogenerada con ejemplos en vivo
3. **Testing** - Testing de regresión visual, accesibilidad e interacciones
4. **Colaboración** - Comparte el estado de los componentes con el equipo/stakeholders
5. **Integración CI** - Testing y despliegue automatizados

## Capacidades

### 1. Creación de Stories
- **Formato de Story**: MDX (Markdown + JSX) o CSF (Component Story Format v3)
- **Variantes de Componente**: Documenta todos los estados del componente
- **Documentación de Props**: Autogenerada desde TypeScript/JSDoc
- **Ejemplos de Uso**: Ejemplos interactivos con fragmentos de código
- **Playground**: Edición en vivo con knobs/controls

### 2. Testing de Accesibilidad
- **Addon a11y**: Auditorías de accesibilidad automatizadas por story
- **Validación de Contraste de Color**: Verifica cumplimiento WCAG AA/AAA
- **Validación ARIA**: Verifica patrones ARIA
- **Navegación por Teclado**: Testea el orden de tabulación e interacción por teclado
- **Soporte de Lector de Pantalla**: Valida con árbol de accesibilidad

### 3. Testing de Regresión Visual
- **Integración Chromatic**: Testing visual en la nube
- **Snapshots Locales**: Capturas base/comparación
- **Detección de Diferencias**: Detección automática de cambios visuales
- **Flujo de Revisión**: Aprobar o rechazar cambios en la UI

### 4. Testing de Interacciones
- **Función Play**: Testea interacciones del componente (click, escribir, etc.)
- **Eventos de Usuario**: Simula comportamiento real del usuario
- **Aserciones**: Verifica el estado del componente después de interacciones
- **Estados de Error**: Documenta y testea condiciones de error

### 5. Documentación y Compartir
- **Docs Autogenerados**: Desde comentarios JSDoc/TypeScript
- **Páginas Markdown**: Páginas de documentación personalizadas
- **Jerarquía de Componentes**: Visualiza la estructura de componentes
- **Vista Previa Responsiva**: Testea en múltiples breakpoints
- **Embeber y Compartir**: Comparte stories individuales o docs completos

## Configuración

### Instalación

```bash
# Initialize Storybook in React/Vue/Angular project
npx storybook@latest init

# Or add to existing project
npm install -D @storybook/react @storybook/addon-essentials
```

### Estructura de Configuración

```
.storybook/
├── main.ts           # Main configuration
├── preview.ts        # Global settings, decorators
├── manager-head.html # Manager UI customization
└── webpack.config.js # Custom webpack config (if needed)
```

### Configuración Esencial (storybook/main.ts)

```typescript
import type { StorybookConfig } from '@storybook/react-webpack5';

const config: StorybookConfig = {
  framework: '@storybook/react-webpack5',
  stories: ['../src/**/*.stories.@(js|jsx|mjs|ts|tsx|mdx)'],
  addons: [
    '@storybook/addon-essentials',
    '@storybook/addon-a11y',
    '@storybook/addon-interactions',
    '@storybook/addon-onboarding',
    '@storybook/addon-links',
    'storybook-dark-mode',
  ],
  docs: {
    autodocs: 'tag', // Auto-generate docs from JSDoc
  },
};
export default config;
```

## Ejemplos de Stories

### Formato CSF 3.0 (Recomendado)

```typescript
// Button.stories.ts
import type { Meta, StoryObj } from '@storybook/react';
import { Button } from './Button';

const meta = {
  title: 'Components/Button',
  component: Button,
  tags: ['autodocs'],
  parameters: {
    layout: 'centered',
    a11y: {
      config: {
        rules: [
          {
            id: 'color-contrast',
            enabled: true,
          },
        ],
      },
    },
  },
  argTypes: {
    variant: {
      control: 'select',
      options: ['primary', 'secondary', 'tertiary'],
      description: 'Button visual variant',
    },
    size: {
      control: 'select',
      options: ['sm', 'md', 'lg'],
      description: 'Button size (touch target >= 44px)',
    },
    disabled: {
      control: 'boolean',
      description: 'Disabled state',
    },
  },
} satisfies Meta<typeof Button>;

export default meta;
type Story = StoryObj<typeof meta>;

// Primary variant
export const Primary: Story = {
  args: {
    variant: 'primary',
    size: 'md',
    children: 'Click me',
  },
};

// Secondary variant
export const Secondary: Story = {
  args: {
    variant: 'secondary',
    size: 'md',
    children: 'Secondary',
  },
};

// Disabled state
export const Disabled: Story = {
  args: {
    disabled: true,
    children: 'Disabled',
  },
};

// With interaction testing
export const WithInteraction: Story = {
  args: {
    children: 'Click me',
  },
  play: async ({ canvasElement }) => {
    const canvas = within(canvasElement);
    const button = canvas.getByRole('button');
    await userEvent.click(button);
  },
};
```

### Formato MDX (Más Flexible)

```markdown
# Button

A versatile button component supporting multiple variants.

## Variants

<Canvas>
  <Story of={ButtonStories.Primary} />
</Canvas>

## API

<ArgsTable of={Button} />

## Accessibility

- Touch target minimum 44x44px (WCAG 2.5.5)
- Proper ARIA labels
- Keyboard accessible (Enter, Space)
- Focus indicators visible
```

## Addons Esenciales

| Addon | Purpose | Install |
|-------|---------|---------|
| **@storybook/addon-a11y** | Accessibility audits | `npm install -D @storybook/addon-a11y` |
| **@storybook/addon-interactions** | Interaction testing | `npm install -D @storybook/addon-interactions` |
| **@storybook/addon-coverage** | Code coverage | `npm install -D @storybook/addon-coverage` |
| **chromatic** | Visual regression CI | `npm install -D chromatic` |
| **storybook-dark-mode** | Dark mode support | `npm install -D storybook-dark-mode` |
| **@storybook/addon-viewport** | Responsive testing | Built-in |
| **@storybook/addon-measure** | Spacing measurement | Built-in |

## Integración de Testing

### Regresión Visual con Chromatic

```bash
# Link to Chromatic
npx chromatic --project-token=<token>

# Automatic in CI
npx chromatic --auto --exit-zero-on-changes
```

### Auditorías de Accesibilidad (axe)

Storybook ejecuta automáticamente auditorías de accesibilidad axe en cada story:

```typescript
// Violations appear in the a11y panel
// Violations shown inline in component preview
// Reports generated in CI
```

### Testing de Interacciones

```typescript
// Play function for interaction testing
export const CompleteForm: Story = {
  play: async ({ canvasElement }) => {
    const canvas = within(canvasElement);

    // Fill form
    const nameInput = canvas.getByLabelText('Name');
    await userEvent.type(nameInput, 'John Doe');

    // Submit
    const submitButton = canvas.getByRole('button', { name: /submit/i });
    await userEvent.click(submitButton);

    // Assert result
    await expect(canvas.getByText('Form submitted')).toBeInTheDocument();
  },
};
```

## Estrategia de Documentación

### Niveles de Documentación de Componentes

**Nivel 1: Autogenerado** (desde JSDoc)
```typescript
/**
 * A flexible button component supporting multiple variants.
 *
 * @component
 * @example
 * return <Button variant="primary">Click me</Button>
 */
export const Button: React.FC<ButtonProps> = ({ children, ...props }) => {
  // Implementation
};
```

**Nivel 2: Stories** (formato CSF)
- Estado por defecto
- Todas las variantes
- Todos los tamaños
- Estados deshabilitado/loading
- Casos extremos

**Nivel 3: Docs Personalizados** (MDX)
- Accessibility requirements
- Design tokens used
- Migration guides
- Related components
- Figma links

### Documentation Checklist

```markdown
For each component story:
- [ ] Props documented with JSDoc
- [ ] All variants documented
- [ ] Accessibility requirements listed
- [ ] Touch target size noted (>= 44x44px)
- [ ] Keyboard interaction tested
- [ ] ARIA labels verified
- [ ] Color contrast checked
- [ ] Dark mode tested
- [ ] Responsive breakpoints shown
- [ ] Related components linked
```

## CI/CD Integration

### GitHub Actions Example

```yaml
name: Storybook Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3

      - name: Install dependencies
        run: npm ci

      - name: Run Storybook tests
        run: npm run test:storybook

      - name: Run accessibility tests
        run: npm run test:a11y

      - name: Upload coverage
        run: npm run coverage

      - name: Chromatic snapshot test
        uses: chromaui/action@v1
        with:
          projectToken: ${{ secrets.CHROMATIC_PROJECT_TOKEN }}
```

### NPM Scripts

```json
{
  "scripts": {
    "storybook": "storybook dev -p 6006",
    "build-storybook": "storybook build",
    "test:storybook": "test-storybook --coverage",
    "test:a11y": "storybook-a11y test --coverage",
    "coverage": "coverage report"
  }
}
```

## Best Practices

### Story Organization

```
src/
├── components/
│   ├── Button/
│   │   ├── Button.tsx
│   │   ├── Button.test.ts
│   │   └── Button.stories.ts
│   ├── Input/
│   │   ├── Input.tsx
│   │   ├── Input.test.ts
│   │   └── Input.stories.ts
```

### Story Naming Conventions

```typescript
// ✅ Good
export const PrimaryLarge: Story = {};
export const SecondarySmall: Story = {};
export const DisabledState: Story = {};

// ❌ Avoid
export const Button1: Story = {};
export const Variant2: Story = {};
```

### Props Documentation

```typescript
// ✅ Good
argTypes: {
  size: {
    control: { type: 'select' },
    options: ['sm', 'md', 'lg'],
    description: 'Component size (touch target >= 44x44px)',
    table: {
      type: { summary: 'string' },
      defaultValue: { summary: 'md' },
    },
  },
}

// ❌ Avoid
argTypes: {
  size: { control: 'select', options: ['sm', 'md', 'lg'] },
}
```

### Accessibility in Stories

```typescript
parameters: {
  a11y: {
    // Auto-run accessibility checks
    config: {
      rules: [
        // Disable irrelevant rules
        { id: 'color-contrast', enabled: true },
        { id: 'heading-order', enabled: true },
      ],
    },
  },
}
```

## Metrics & Benefits

**Companies Using Storybook**:
- Airbnb
- GitHub
- Microsoft
- Shopify
- Slack
- Twitch
- Uber

**Statistics**:
- ~80k+ GitHub stars
- ~1.5M+ npm weekly downloads
- Used in 50%+ of React component libraries
- Reduces component development time by 40%+

## Related Skills

- `screenshot-reporter` - Generate static screenshots
- `design-system-generator` - Design system documentation
- `component-inventory` - Component cataloging
- `aria-accessibility-patterns` - ARIA implementation
- `design-patterns-library` - Pattern documentation

## Next Steps

1. **Initialize Storybook** in your project
2. **Create stories** for your components
3. **Enable a11y addon** for accessibility checks
4. **Set up Chromatic** for visual regression testing
5. **Document patterns** in MDX
6. **Integrate in CI/CD** for automated testing
7. **Share with team** via deployed Storybook

## Resources

- **Storybook Docs**: https://storybook.js.org/docs/
- **Learning**: https://storybook.js.org/tutorials/
- **Addon Index**: https://storybook.js.org/addons
- **Community**: https://discord.gg/storybook
- **GitHub**: https://github.com/storybookjs/storybook

