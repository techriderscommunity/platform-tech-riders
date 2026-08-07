---
name: react-component-styling
description: 'Domina el estilado de componentes React con CSS Modules, styled-components, Emotion, Tailwind CSS y patrones CSS-in-JS. Incluye integración de frameworks, optimización de rendimiento, theming, modo oscuro y enfoques accesibilidad-primero.'
---

# Estilado de Componentes React

Construye componentes React listos para producción con enfoques modernos de estilado. Este skill cubre todos los patrones principales de estilado React con mejores prácticas de https://react.dev/learn/styling.

## Enfoques de Estilado

### 1. **CSS Modules** (Recomendado para Equipos Grandes)
Estilado encapsulado con cero overhead en runtime.

**Cuándo usarlo:**
- Equipos grandes que necesitan predecibilidad
- Proyectos sin estilado dinámico intenso
- Máxima optimización en tiempo de compilación
- Estilado de componentes fuertemente tipado

**Configuración:**
```bash
# Already supported in Create React App, Vite, Next.js
# Enable in vite.config.ts:
export default {
  css: {
    modules: {
      localsConvention: 'camelCase'
    }
  }
}
```

**Pattern:**
```tsx
// Button.module.scss
.button {
  padding: 8px 16px;
  border-radius: 4px;
  transition: background-color 200ms;

  &:hover {
    background-color: var(--color-primary-hover);
  }

  &.variant-primary {
    background-color: var(--color-primary);
    color: var(--color-on-primary);
  }

  &.variant-secondary {
    background-color: var(--color-secondary);
  }

  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
}

// Button.tsx
import styles from './Button.module.scss';

interface ButtonProps {
  variant?: 'primary' | 'secondary';
  disabled?: boolean;
  children: React.ReactNode;
}

export const Button: React.FC<ButtonProps> = ({
  variant = 'primary',
  disabled,
  children
}) => (
  <button
    className={`${styles.button} ${styles[`variant-${variant}`]}`}
    disabled={disabled}
  >
    {children}
  </button>
);
```

### 2. **Styled Components** (Recomendado para Estilos Dinámicos)
CSS-in-JS con todas las capacidades de JavaScript, estilos encapsulados por componente.

**Instalar:**
```bash
npm install styled-components
npm install -D @types/styled-components
```

**Pattern:**
```tsx
import styled from 'styled-components';

interface ButtonProps {
  $variant?: 'primary' | 'secondary';
  $isActive?: boolean;
}

const StyledButton = styled.button<ButtonProps>`
  padding: 8px 16px;
  border-radius: 4px;
  border: none;
  cursor: pointer;
  transition: all 200ms ease;
  font-weight: 500;

  background-color: ${props => props.$variant === 'primary'
    ? 'var(--color-primary)'
    : 'var(--color-secondary)'};
  color: ${props => props.$variant === 'primary'
    ? 'var(--color-on-primary)'
    : 'var(--color-on-secondary)'};

  &:hover:not(:disabled) {
    transform: translateY(-2px);
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  }

  &:active:not(:disabled) {
    transform: translateY(0);
  }

  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }

  ${props => props.$isActive && `
    border-bottom: 2px solid currentColor;
  `}

  @media (max-width: 640px) {
    padding: 6px 12px;
    font-size: 14px;
  }
`;

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary';
  isActive?: boolean;
}

export const Button = React.forwardRef<HTMLButtonElement, ButtonProps>(
  ({ variant = 'primary', isActive = false, ...props }, ref) => (
    <StyledButton
      ref={ref}
      $variant={variant}
      $isActive={isActive}
      {...props}
    />
  )
);
Button.displayName = 'Button';
```

**Estilos Globales:**
```tsx
import { createGlobalStyle } from 'styled-components';

export const GlobalStyles = createGlobalStyle`
  :root {
    --color-primary: #0066cc;
    --color-on-primary: #ffffff;
    --color-secondary: #f0f0f0;
    --color-on-secondary: #000000;
  }

  [data-theme="dark"] {
    --color-primary: #3399ff;
    --color-on-primary: #000000;
    --color-secondary: #2a2a2a;
    --color-on-secondary: #ffffff;
  }

  * {
    box-sizing: border-box;
  }

  body {
    margin: 0;
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto;
    background-color: var(--color-background, white);
    color: var(--color-text, black);
  }
`;

// App.tsx
export function App() {
  return (
    <>
      <GlobalStyles />
      <YourComponent />
    </>
  );
}
```

### 3. **Emotion** (Rendimiento + Flexibilidad)
CSS-in-JS ligero con APIs potentes.

**Instalar:**
```bash
npm install @emotion/react @emotion/styled
npm install -D @emotion/babel-plugin
```

**Pattern:**
```tsx
import styled from '@emotion/styled';
import { css } from '@emotion/react';

const buttonStyles = css`
  padding: 8px 16px;
  border-radius: 4px;
  border: none;
  cursor: pointer;
  transition: background-color 200ms;
  font-weight: 500;

  &:hover {
    transform: translateY(-2px);
  }

  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
`;

interface StyledButtonProps {
  variant?: 'primary' | 'secondary';
}

const StyledButton = styled.button<StyledButtonProps>`
  ${buttonStyles}

  background-color: ${props => props.variant === 'primary'
    ? 'var(--color-primary)'
    : 'var(--color-secondary)'};
  color: ${props => props.variant === 'primary'
    ? 'var(--color-on-primary)'
    : 'var(--color-on-secondary)'};
`;

export const Button: React.FC<ButtonProps> = ({
  variant = 'primary',
  children,
  ...props
}) => (
  <StyledButton variant={variant} {...props}>
    {children}
  </StyledButton>
);
```

### 4. **Tailwind CSS** (Utility-First)
Listo para producción con CSS personalizado mínimo, excelente experiencia de desarrollo.

**Configuración:**
```bash
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p
```

**tailwind.config.ts:**
```ts
import type { Config } from 'tailwindcss';

export default {
  content: [
    './index.html',
    './src/**/*.{js,ts,jsx,tsx}',
  ],
  theme: {
    extend: {
      colors: {
        primary: 'var(--color-primary)',
        secondary: 'var(--color-secondary)',
      },
      spacing: {
        xs: '4px',
        sm: '8px',
        md: '16px',
        lg: '24px',
        xl: '32px',
      },
    },
  },
  plugins: [],
} satisfies Config;
```

**Pattern:**
```tsx
interface ButtonProps {
  variant?: 'primary' | 'secondary';
  size?: 'sm' | 'md' | 'lg';
  disabled?: boolean;
  children: React.ReactNode;
}

export const Button: React.FC<ButtonProps> = ({
  variant = 'primary',
  size = 'md',
  disabled,
  children,
  ...props
}) => {
  const baseStyles = 'font-medium rounded-md transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed';

  const variantStyles = {
    primary: 'bg-primary text-on-primary hover:shadow-lg hover:-translate-y-0.5',
    secondary: 'bg-secondary text-on-secondary hover:bg-secondary-hover',
  };

  const sizeStyles = {
    sm: 'px-2 py-1 text-sm',
    md: 'px-4 py-2 text-base',
    lg: 'px-6 py-3 text-lg',
  };

  return (
    <button
      className={`${baseStyles} ${variantStyles[variant]} ${sizeStyles[size]}`}
      disabled={disabled}
      {...props}
    >
      {children}
    </button>
  );
};
```

**Modo Oscuro (con Variables CSS):**
```html
<!-- index.html -->
<html data-theme="light">

<!-- CSS -->
:root {
  --color-primary: #0066cc;
}

[data-theme="dark"] {
  --color-primary: #3399ff;
}
```

## Implementación de Temas

**Proveedor de Design Tokens:**
```tsx
import { ReactNode, createContext, useContext, useState } from 'react';

interface ThemeContextType {
  theme: 'light' | 'dark';
  setTheme: (theme: 'light' | 'dark') => void;
}

const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

export const ThemeProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [theme, setTheme] = useState<'light' | 'dark'>('light');

  const handleSetTheme = (newTheme: 'light' | 'dark') => {
    setTheme(newTheme);
    document.documentElement.setAttribute('data-theme', newTheme);
    localStorage.setItem('theme', newTheme);
  };

  return (
    <ThemeContext.Provider value={{ theme, setTheme: handleSetTheme }}>
      {children}
    </ThemeContext.Provider>
  );
};

export const useTheme = () => {
  const context = useContext(ThemeContext);
  if (!context) throw new Error('useTheme must be used within ThemeProvider');
  return context;
};
```

## Optimización de Rendimiento

### Tamaño del Bundle
```tsx
// ✅ Good: Dynamic imports for heavy components
const HeavyComponent = React.lazy(() => import('./HeavyComponent'));

// ❌ Avoid: Loading all styles upfront
import * as styles from './all-styles.css';
```

### Rendimiento CSS-in-JS
```tsx
// ✅ Good: Memoize styled components
const MemoizedButton = React.memo(StyledButton);

// ✅ Good: Extract repeated styles
const commonTransition = css`transition: all 200ms`;

// ❌ Avoid: Creating styled components inside render
const BadComponent = () => {
  const DynamicButton = styled.button`color: red`; // BAD!
  return <DynamicButton />;
};
```

## Accesibilidad y Diseño Responsivo

**Media Queries con Tailwind:**
```tsx
<div className="text-sm sm:text-base md:text-lg lg:text-xl">
  Responsive typography
</div>
```

**Estados de Foco (Crítico):**
```tsx
const StyledButton = styled.button`
  outline: 2px solid transparent;
  outline-offset: 2px;

  &:focus-visible {
    outline-color: var(--color-focus);
  }

  @media (prefers-reduced-motion: reduce) {
    transition: none;
  }
`;
```

## Cuándo Usar Cada Uno

| Patrón | Mejor Para | Compromisos |
|---------|----------|-----------|
| **CSS Modules** | Equipos grandes, predecibilidad | Menos dinámico, más archivos |
| **Styled Components** | Estilos dinámicos, lógica de componente | Coste en runtime, tamaño del bundle |
| **Emotion** | Rendimiento + flexibilidad | Curva de aprendizaje |
| **Tailwind** | Desarrollo rápido, consistencia | Aprender nombres de utilidades |
| **Inline Styles** | Solo prototipado | Sin media queries, sin pseudo-clases |

## Referencias

- [Guía Oficial de Estilos React](https://react.dev/learn/styling)
- [Docs de Styled Components](https://styled-components.com/docs)
- [Documentación de Emotion](https://emotion.sh/docs/introduction)
- [Documentación de Tailwind CSS](https://tailwindcss.com/docs)
- [MDN CSS Modules](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Modules)
- [WCAG Indicadores de Foco](https://www.w3.org/WAI/WCAG21/Understanding/focus-visible.html)

## Plantillas Disponibles

- `Button.css-modules.tsx` - Ejemplo CSS Modules
- `Button.styled-components.tsx` - Ejemplo Styled Components
- `Button.emotion.tsx` - Ejemplo Emotion
- `Button.tailwind.tsx` - Ejemplo Tailwind CSS
- `ThemeProvider.tsx` - Implementación de tema/modo oscuro
- `tailwind.config.ts` - Plantilla de configuración Tailwind
