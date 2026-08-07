---
name: vue-component-styling
description: 'Domina el estilado de componentes Vue con estilos scoped, CSS Modules, Tailwind CSS y patrones específicos de Vue. Incluye soporte de temas, estilado dinámico, diseño responsivo y mejores prácticas de accesibilidad.'
---

# Estilado de Componentes Vue

Construye componentes Vue 3 listos para producción con enfoques modernos de estilado. Este skill cubre todos los patrones de estilado específicos de Vue siguiendo https://vuejs.org/guide/extras/ways-of-using-vue.html#single-file-components.

## Enfoques de Estilado

### 1. **Vue Scoped Styles** (Recomendado - Por Defecto)
CSS encapsulado al componente con la característica `<style scoped>` de Vue - sin configuración necesaria.

**Pattern:**
```vue
<template>
  <button
    :class="['btn', `btn--${variant}`]"
    :disabled="disabled"
  >
    <slot />
  </button>
</template>

<script setup lang="ts">
interface Props {
  variant?: 'primary' | 'secondary';
  disabled?: boolean;
}

withDefaults(defineProps<Props>(), {
  variant: 'primary',
  disabled: false,
});
</script>

<style scoped>
.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-height: 44px;
  padding: 8px 16px;
  border-radius: 4px;
  border: 1px solid transparent;
  font-weight: 500;
  font-size: 16px;
  cursor: pointer;
  transition: all 200ms ease;
  user-select: none;

  &:hover:not(:disabled) {
    transform: translateY(-2px);
  }

  &:focus-visible {
    outline: 2px solid var(--color-focus);
    outline-offset: 2px;
  }

  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
}

.btn--primary {
  background-color: var(--color-primary);
  color: var(--color-on-primary);
}

.btn--primary:hover:not(:disabled) {
  background-color: var(--color-primary-hover);
  box-shadow: 0 4px 12px rgba(0, 102, 204, 0.2);
}

.btn--secondary {
  background-color: var(--color-secondary);
  color: var(--color-on-secondary);
}

.btn--secondary:hover:not(:disabled) {
  background-color: var(--color-secondary-hover);
}

@media (prefers-reduced-motion: reduce) {
  .btn {
    transition: none;
  }

  .btn:hover:not(:disabled) {
    transform: none;
  }
}
</style>
```

**Características Clave:**
- ✅ CSS automáticamente encapsulado al componente
- ✅ Sin conflictos de nombres
- ✅ Sintaxis simple, sin configuración
- ✅ Soporte completo de SCSS/LESS

### 2. **CSS Modules** (Para Proyectos Complejos)
Importaciones de módulos explícitas con seguridad de tipos.

**Configuración (Vite):**
```ts
// vite.config.ts
export default {
  css: {
    modules: {
      localsConvention: 'camelCase'
    }
  }
}
```

**Patrón:**
```vue
<template>
  <button
    :class="[styles.btn, styles[`variant${capitalize(variant)}`]]"
    :disabled="disabled"
  >
    <slot />
  </button>
</template>

<script setup lang="ts">
import { capitalize } from 'vue';
import styles from './Button.module.scss';

interface Props {
  variant?: 'primary' | 'secondary';
  disabled?: boolean;
}

withDefaults(defineProps<Props>(), {
  variant: 'primary',
});
</script>

<style module>
.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-height: 44px;
  padding: 8px 16px;
  border-radius: 4px;
  border: 1px solid transparent;
  font-weight: 500;
  cursor: pointer;
  transition: all 200ms ease;
}

.variantPrimary {
  background-color: var(--color-primary);
  color: var(--color-on-primary);
}

.variantSecondary {
  background-color: var(--color-secondary);
  color: var(--color-on-secondary);
}
</style>
```

### 3. **Tailwind CSS** (Utility-First)
Listo para producción con clases de utilidad, excelente experiencia de desarrollo.

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
    './src/**/*.{vue,js,ts,jsx,tsx}',
  ],
  theme: {
    extend: {
      colors: {
        primary: 'var(--color-primary)',
        secondary: 'var(--color-secondary)',
      },
    },
  },
} satisfies Config;
```

**Pattern:**
```vue
<template>
  <button
    :class="[
      'inline-flex items-center justify-center',
      'min-h-11 px-4 py-2',
      'rounded-md border border-transparent',
      'font-medium cursor-pointer',
      'transition-all duration-200',
      'focus-visible:outline-2 focus-visible:outline-offset-2',
      'disabled:opacity-50 disabled:cursor-not-allowed',
      'motion-reduce:transition-none',
      variantClasses,
    ]"
    :disabled="disabled"
  >
    <slot />
  </button>
</template>

<script setup lang="ts">
import { computed } from 'vue';

interface Props {
  variant?: 'primary' | 'secondary';
  disabled?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  variant: 'primary',
});

const variantClasses = computed(() => {
  return props.variant === 'primary'
    ? 'bg-blue-600 text-white hover:enabled:bg-blue-700'
    : 'bg-gray-100 text-gray-900 hover:enabled:bg-gray-200';
});
</script>
```

### 4. **CSS-in-JS (UnoCSS / Windi CSS)**
Motor CSS atómico con integración Vue.

**Instalar UnoCSS:**
```bash
npm install -D unocss
```

**Pattern:**
```vue
<template>
  <button
    class="inline-flex items-center justify-center min-h-11 px-4 py-2 rounded-md border-transparent font-medium cursor-pointer transition-all duration-200 focus-visible:outline-2 disabled:opacity-50 disabled:cursor-not-allowed"
    :class="[
      variant === 'primary'
        ? 'bg-blue-600 text-white hover:enabled:bg-blue-700'
        : 'bg-gray-100 text-gray-900 hover:enabled:bg-gray-200'
    ]"
    :disabled="disabled"
  >
    <slot />
  </button>
</template>

<script setup lang="ts">
interface Props {
  variant?: 'primary' | 'secondary';
  disabled?: boolean;
}

withDefaults(defineProps<Props>(), {
  variant: 'primary',
});
</script>
```

## Implementación de Temas

**Composable para Gestión de Temas:**
```ts
// composables/useTheme.ts
import { ref, computed, watch } from 'vue';

type Theme = 'light' | 'dark';

const theme = ref<Theme>('light');
const storageKey = 'app-theme';

export const useTheme = () => {
  const initTheme = () => {
    const stored = localStorage.getItem(storageKey) as Theme | null;
    if (stored && ['light', 'dark'].includes(stored)) {
      theme.value = stored;
    } else if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
      theme.value = 'dark';
    }
    applyTheme();
  };

  const applyTheme = () => {
    document.documentElement.setAttribute('data-theme', theme.value);
    localStorage.setItem(storageKey, theme.value);
  };

  const setTheme = (newTheme: Theme) => {
    theme.value = newTheme;
    applyTheme();
  };

  const toggleTheme = () => {
    theme.value = theme.value === 'light' ? 'dark' : 'light';
    applyTheme();
  };

  watch(theme, applyTheme);

  if (process.client && !theme.value) {
    initTheme();
  }

  return {
    theme: computed(() => theme.value),
    setTheme,
    toggleTheme,
  };
};
```

**Uso en App:**
```vue
<template>
  <div class="app">
    <header class="flex justify-between items-center">
      <h1>My App</h1>
      <button @click="toggleTheme">
        {{ theme === 'light' ? '🌙' : '☀️' }}
      </button>
    </header>
    <main>
      <slot />
    </main>
  </div>
</template>

<script setup lang="ts">
import { useTheme } from '@/composables/useTheme';

const { theme, toggleTheme } = useTheme();
</script>

<style scoped>
.app {
  background-color: var(--color-background);
  color: var(--color-text);
}
</style>
```

## Estilado Dinámico

**Clases Computadas:**
```vue
<template>
  <div :class="buttonClasses">
    <slot />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

interface Props {
  size?: 'sm' | 'md' | 'lg';
  variant?: 'primary' | 'secondary';
  disabled?: boolean;
  loading?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  size: 'md',
  variant: 'primary',
});

const buttonClasses = computed(() => ({
  btn: true,
  [`btn--${props.size}`]: true,
  [`btn--${props.variant}`]: true,
  'is-loading': props.loading,
  'is-disabled': props.disabled,
}));
</script>
```

**Estilos Inline Dinámicos (Cuando Sea Necesario):**
```vue
<template>
  <div :style="customStyles">
    <slot />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

interface Props {
  backgroundColor?: string;
  textColor?: string;
  padding?: string;
}

const props = defineProps<Props>();

const customStyles = computed(() => ({
  backgroundColor: props.backgroundColor || 'var(--color-primary)',
  color: props.textColor || 'var(--color-on-primary)',
  padding: props.padding || '8px 16px',
}));
</script>
```

## Diseño Responsivo

**Media Queries en Estilos Scoped:**
```vue
<style scoped>
.button {
  font-size: 16px;
  padding: 8px 16px;
}

@media (max-width: 640px) {
  .button {
    font-size: 14px;
    padding: 6px 12px;
  }
}

@media (prefers-reduced-motion: reduce) {
  .button {
    transition: none !important;
  }
}
</style>
```

**Prefijos Responsivos de Tailwind:**
```vue
<template>
  <div class="text-sm sm:text-base md:text-lg lg:text-xl">
    Responsive typography
  </div>
</template>
```

## Mejores Prácticas de Rendimiento

**Evita la Construcción Dinámica de Clases en Templates:**
```vue
<!-- ❌ Bad: Recalculates every render -->
<button :class="'btn btn--' + variant">Click</button>

<!-- ✅ Good: Uses computed -->
<button :class="buttonClasses">Click</button>

<script setup>
import { computed } from 'vue';

const buttonClasses = computed(() => ({
  btn: true,
  [`btn--${variant.value}`]: true,
}));
</script>
```

**Rendimiento de CSS Modules:**
```vue
<!-- ✅ Good: Direct class reference -->
<button :class="[styles.btn, styles[`variant${variant}`]]">
  Click
</button>
```

## Accesibilidad

**Gestión de Foco:**
```vue
<style scoped>
.btn:focus-visible {
  outline: 2px solid var(--color-focus);
  outline-offset: 2px;
}

@media (prefers-reduced-motion: reduce) {
  .btn {
    transition: none;
  }
}
</style>
```

**Contraste de Color (Variables CSS):**
```css
:root {
  --color-primary: #0066cc;
  --color-on-primary: #ffffff;
}

[data-theme="dark"] {
  --color-primary: #3399ff;
  --color-on-primary: #000000;
}
```

## Cuándo Usar Cada Uno

| Patrón | Mejor Para | Compromisos |
|---------|----------|-----------|
| **Scoped Styles** | La mayoría de proyectos Vue | Estilos menos reutilizables |
| **CSS Modules** | Estilado complejo, reutilización | Más verboso |
| **Tailwind** | Desarrollo rápido, consistencia | Curva de aprendizaje |
| **UnoCSS** | Rendimiento + flexibilidad | Menor ecosistema |
| **Inline Styles** | Solo prototipado | Sin pseudo-clases, bajo rendimiento |

## Referencias

- [Guía de Estilos Vue SFC](https://vuejs.org/guide/extras/ways-of-using-vue.html#single-file-components)
- [Documentación `<style>` Vue](https://vuejs.org/api/sfc-spec.html#style-block)
- [Documentación Tailwind CSS](https://tailwindcss.com/docs)
- [MDN CSS Modules](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Modules)
- [WCAG Indicadores de Foco](https://www.w3.org/WAI/WCAG21/Understanding/focus-visible.html)
- [Documentación UnoCSS](https://unocss.dev/)
- [Documentación Windi CSS](https://windicss.org/)

## Plantillas Disponibles

- `Button.scoped-styles.vue` - Ejemplo de estilos scoped
- `Button.css-modules.vue` - Ejemplo de CSS Modules
- `Button.tailwind.vue` - Ejemplo de Tailwind CSS
- `useTheme.ts` - Composable de tema
- `tailwind.config.ts` - Plantilla de configuración Tailwind
