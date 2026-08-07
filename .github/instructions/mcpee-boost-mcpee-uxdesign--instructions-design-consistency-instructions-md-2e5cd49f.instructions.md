---
description: 'Mantén consistencia de diseño en componentes: usa design tokens, implementa comportamiento responsivo correctamente, asegúra que los estados de componente coincidan con el design system, soporta modos claro/oscuro y documenta especificaciones de componentes.'
applyTo: '**.component.scss, **.component.css, **.module.css, **.styles.ts, **.module.scss'
---

# Pautas de Consistencia de Diseño

Al trabajar con estilos de componentes, asegúra la consistencia en todo el design system:

## Uso de Design Tokens

- **Colores**: Usa nombres semánticos, nunca valores hex hardcodeados
  - Primary: `--color-primary`, `--color-on-primary`
  - Secondary: `--color-secondary`, `--color-on-secondary`
  - States: `--color-success`, `--color-warning`, `--color-error`
  - Neutrals: `--color-text`, `--color-background`, `--color-border`

- **Espaciado**: Usa el sistema de grid 8px de forma consistente
  - Tamaños: `--spacing-xs (4px)`, `--spacing-sm (8px)`, `--spacing-md (16px)`, `--spacing-lg (24px)`, `--spacing-xl (32px)`
  - Usa `var(--spacing-md)` en lugar de hardcodear `16px`

- **Tipografía**: Implementa la escala del design system
  - Tamaños: `--font-size-xs, --font-size-sm, --font-size-base, --font-size-lg, --font-size-xl`
  - Pesos: `--font-weight-normal (400)`, `--font-weight-medium (500)`, `--font-weight-bold (700)`
  - Line-height: `--line-height-tight (1.2)`, `--line-height-normal (1.5)`

- **Sombras/Elevación**: Usa tokens de sombra consistentes
  - Niveles: `--shadow-sm`, `--shadow-md`, `--shadow-lg`, `--shadow-xl`
  - Ejemplo: `box-shadow: var(--shadow-md);`

- **Border Radius**: Mantiene escala de radios consistente
  - Tamaños: `--radius-sm (2px)`, `--radius-md (4px)`, `--radius-lg (8px)`

## Estados de Componente

Todos los componentes interactivos deben soportar estos estados:

- **Default**: Estado normal e inactivo
- **Hover**: Feedback visual al pasar el cursor (solo escritorio)
- **Focus**: Indicador de foco visible para usuarios de teclado (outline 2px con offset 2px)
- **Active**: Estado presionado/seleccionado
- **Disabled**: Estado no interactivo (opacity 0.5 + `cursor: not-allowed`)
- **Loading**: Estado en progreso con spinner o skeleton
- **Error**: Estado inválido o fallido (usa color de error, mensaje claro)

Los estados deben ser claramente distintos y seguir los patrones del design system.

## Comportamiento Responsivo

- **Mobile-First**: Los estilos base son para móvil, luego añade `@media (min-width: breakpoint)`
- **Breakpoints**: 320px (xs), 640px (sm), 1024px (md), 1440px (lg), 1920px (xl)
- **Touch Targets**: Mínimo 44px × 44px en dispositivos móviles
- **Tipografía**: Escala el texto en pantallas mayores
  - Móvil: 14px, Escritorio: 16px
  - Usa `@media (min-width: 1024px) { font-size: 16px; }`
- **Padding**: Ajusta padding/margins según el tamaño de pantalla
  - Móvil: `var(--spacing-md)`, Escritorio: `var(--spacing-lg)`
- **Layout**: Cambia de una columna a múltiples columnas donde corresponda

## Soporte de Modo Oscuro

- **CSS Variables Approach** (Recommended):
  ```css
  :root {
    --color-background: #ffffff;
    --color-text: #000000;
  }

  [data-theme="dark"] {
    --color-background: #1a1a1a;
    --color-text: #ffffff;
  }
  ```

- **Prefers Color Scheme**:
  ```css
  @media (prefers-color-scheme: dark) {
    body {
      background-color: #1a1a1a;
      color: #ffffff;
    }
  }
  ```

- **Requisitos**:
    - Siempre testea el contraste en modo oscuro (sigue requiriendo ratio 4.5:1)
    - Asegúra que imágenes e iconos funcionen en ambos modos
    - Verifica la legibilidad del texto en modo oscuro
    - Considera filtro invert para iconos SVG si es necesario

## Accesibilidad en Estilos

- **Focus Indicators**:
  ```css
  button:focus-visible {
    outline: 2px solid var(--color-focus);
    outline-offset: 2px;
  }
  ```

- **Contraste de Color**: Mínimo 4.5:1 para texto normal (WCAG AA)
  - Verifica con WebAIM Contrast Checker
  - Usa colores semánticos que mantengan contraste en modo oscuro

- **Movimiento Reducido**:
  ```css
  @media (prefers-reduced-motion: reduce) {
    * {
      animation: none !important;
      transition: none !important;
    }
  }
  ```

- **Touch Targets**: Asegúra que los elementos interactivos sean mínimo 44×44px

## Documentación de Componentes

Cada estilo de componente debe documentar:

1. **Design Tokens Usados**
   - Colores, espaciado, tipografía utilizados
   - Soporte de tema (claro/oscuro)

2. **Estados del Componente**
   - Default, hover, focus, active, disabled, error, loading

3. **Comportamiento Responsivo**
   - ¿Cómo se ve este componente en móvil/tablet/escritorio?
   - ¿Algún elemento oculto o cambio de layout?

4. **Características de Accesibilidad**
   - Indicadores de foco
   - Contraste de color verificado
   - Navegación por teclado soportada

5. **Ejemplos de Uso**
   ```scss
   // Component: Button
   // Tokens: --color-primary, --spacing-sm, --spacing-md
   // States: default, hover, focus, disabled
   // Responsive: Touch target 44px on mobile
   // a11y: Focus outline 2px, 4.5:1 contrast
   ```

## Verificaciones de Consistencia

Antes de hacer commit de estilos de componente:

- [ ] Todos los colores usan variables CSS (sin hex hardcodeados)
- [ ] Todos los espaciados usan design tokens (sin valores px arbitrarios)
- [ ] Todos los estados están implementados (default, hover, focus, disabled)
- [ ] Modo oscuro testeado y contraste verificado
- [ ] Diseño responsivo testeado en múltiples tamaños de pantalla
- [ ] Indicador de foco visible y correcto (outline 2px)
- [ ] Componente documentado con design tokens y estados
- [ ] Sin estilos inline (excepto valores dinámicos de JS)
- [ ] Convención de nombres BEM o similar seguida
- [ ] Animación respeta `prefers-reduced-motion`
