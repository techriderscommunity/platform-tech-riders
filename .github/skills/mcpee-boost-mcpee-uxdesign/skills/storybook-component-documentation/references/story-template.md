# Story Template — Storybook CSF3

> Plantilla base para documentar componentes con Storybook (Component Story Format v3).  
> Copia, sustituye `[ComponentName]` y añade las variantes relevantes.

---

## Plantilla base (TypeScript + React)

```tsx
import type { Meta, StoryObj } from '@storybook/react';
import { [ComponentName] } from './[ComponentName]';

// ─── Meta ────────────────────────────────────────────────────────────────────
const meta: Meta<typeof [ComponentName]> = {
  title: '[Sección]/[ComponentName]',       // ej. 'Forms/Button', 'Data/Table'
  component: [ComponentName],
  tags: ['autodocs'],                        // activa la página de docs automática
  parameters: {
    layout: 'centered',                      // 'centered' | 'fullscreen' | 'padded'
    docs: {
      description: {
        component: '[Descripción breve del componente y cuándo usarlo]',
      },
    },
  },
  argTypes: {
    // Define los controles para cada prop
    // variant: { control: 'select', options: ['primary', 'secondary', 'ghost'] },
    // disabled: { control: 'boolean' },
    // onClick: { action: 'clicked' },
  },
};

export default meta;
type Story = StoryObj<typeof [ComponentName]>;

// ─── Stories ─────────────────────────────────────────────────────────────────

// Estado por defecto — siempre el primero
export const Default: Story = {
  args: {
    // props mínimas para renderizar el componente
  },
};

// Variantes principales
export const Primary: Story = {
  args: {
    variant: 'primary',
    children: 'Primary Action',
  },
};

export const Secondary: Story = {
  args: {
    variant: 'secondary',
    children: 'Secondary Action',
  },
};

// Estado desactivado
export const Disabled: Story = {
  args: {
    disabled: true,
    children: 'Disabled',
  },
};

// Estado de carga
export const Loading: Story = {
  args: {
    loading: true,
    children: 'Loading...',
  },
};

// Estado de error (si aplica)
export const Error: Story = {
  args: {
    error: true,
    errorMessage: 'Something went wrong',
  },
};

// Estado vacío (si aplica — para listas, tablas, etc.)
export const Empty: Story = {
  args: {
    items: [],
  },
};

// Variante mobile / responsive (si aplica)
export const Mobile: Story = {
  parameters: {
    viewport: { defaultViewport: 'mobile1' },
  },
  args: {
    // props para la versión mobile
  },
};
```

---

## Plantilla con play function (testing de interacciones)

```tsx
import { within, userEvent } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

export const Interactive: Story = {
  args: {
    // props iniciales
  },
  play: async ({ canvasElement }) => {
    const canvas = within(canvasElement);

    // Simular interacción
    const button = canvas.getByRole('button', { name: /submit/i });
    await userEvent.click(button);

    // Verificar resultado
    await expect(canvas.getByText('Success')).toBeInTheDocument();
  },
};
```

---

## Plantilla con testing de accesibilidad

```tsx
export const AccessibilityCheck: Story = {
  args: {
    // props normales
  },
  parameters: {
    a11y: {
      // config opcional para axe-core
      config: {
        rules: [
          { id: 'color-contrast', enabled: true },
          { id: 'focus-trap', enabled: true },
        ],
      },
    },
  },
};
```

---

## Convenciones de nomenclatura

| Story | Cuándo usarla |
|-------|--------------|
| `Default` | Estado base, siempre presente |
| `Primary` / `Secondary` / `Ghost` | Variantes visuales |
| `Disabled` | Estado no interactivo |
| `Loading` | Estado de carga |
| `Error` | Estado de error |
| `Empty` | Sin datos (listas, tablas) |
| `WithLongContent` | Prueba de overflow / wrapping |
| `Mobile` | Viewport reducido |
| `DarkMode` | Tema oscuro |
| `Interactive` | Con play function para test de interacción |
| `AccessibilityCheck` | Auditoría a11y explícita |

---

## Título (`title`) — estructura recomendada

```
'[Categoría]/[ComponentName]'

Ejemplos:
'Forms/Button'
'Forms/Input'
'Data/Table'
'Data/Badge'
'Feedback/Alert'
'Feedback/Toast'
'Navigation/Tabs'
'Navigation/Breadcrumb'
'Overlay/Modal'
'Overlay/Tooltip'
```

---

## Parámetros de layout

```tsx
parameters: {
  layout: 'centered',    // componentes pequeños (botones, badges, inputs)
  layout: 'padded',      // componentes medianos (cards, forms)
  layout: 'fullscreen',  // componentes de página completa (nav, layouts)
}
```
