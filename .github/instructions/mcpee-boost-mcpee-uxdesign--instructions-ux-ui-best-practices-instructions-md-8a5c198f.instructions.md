---
description: 'Aplica mejores prácticas UX/UI, estándares de accesibilidad (WCAG 2.1 AA), patrones de diseño responsivo y HTML semántico al crear o modificar componentes de UI.'
applyTo: '**.tsx, **.jsx, **.vue, **.html, **.component.ts'
---

# Pautas de Mejores Prácticas UX/UI

Al trabajar con componentes y estilos de UI, sigue estas mejores prácticas establecidas:

## Accesibilidad (Mínimo WCAG 2.1 AA)

- Usa HTML semántico: `<button>`, `<nav>`, `<main>`, `<section>`, `<article>` en lugar de `<div>` con manejadores de click
- Incluye atributos `aria-label`, `aria-describedby`, `role` donde sea necesario
- Asegúra que el ratio de contraste de color sea al menos 4.5:1 para texto (estándar WCAG AA)
- Soporta navegación por teclado: Tab, Enter, Escape, teclas de flecha
- Incluye indicadores de foco visibles: `outline: 2px solid currentColor; outline-offset: 2px;`
- Testea con lectores de pantalla (NVDA, JAWS, VoiceOver)
- Soporta `prefers-reduced-motion` para animaciones
- Implementa enlaces de salto de navegación para usuarios de teclado

## Diseño Responsivo (Mobile-First)

- Usa enfoque mobile-first: estilos base para móvil, luego añade breakpoints
- Define breakpoints claros: 320px (xs), 640px (sm), 1024px (md), 1440px (lg), 1920px (xl)
- Usa layouts flexibles: Flexbox para layouts 1D, CSS Grid para 2D
- Asegúra que los touch targets sean al menos 44x44px (mínimo WCAG), 48x48px recomendado
- Testea en múltiples tamaños de pantalla y orientaciones
- Usa `max-width` para legibilidad (longitud de línea de 50-80 caracteres)
- Implementa escalado de tipografía responsivo

## Diseño de Componentes

- Sigue el Principio de Responsabilidad Única: un componente = un propósito
- Haz los componentes componibles y reutilizables
- Acepta datos vía props, no valores hardcodeados
- Soporta temas claro/oscuro vía variables CSS o design tokens
- Mantiene nombres consistentes entre componentes similares
- Documenta el componente: props, slots, eventos, ejemplos de uso
- Implementa estados de error y loading correctamente
- Considera el impacto en rendimiento (lazy loading, code splitting)

## Mejores Prácticas de Estilos y CSS

- Usa variables CSS (custom properties) para design tokens
- Mantiene espaciado consistente usando grid 8px: 4px, 8px, 16px, 24px, 32px, 48px
- Usa clases CSS o clases de utilidad para consistencia
- Evita estilos inline excepto para valores verdaderamente dinámicos
- Soporta reduced-motion: `@media (prefers-reduced-motion: reduce) { ... }`
- Usa nombres de color semánticos: `--color-primary`, `--color-on-primary`, `--color-error`
- Implementa soporte de temas para modos claro/oscuro

## Tipografía

- Usa fuentes del sistema o fuentes web-safe desde CDN de confianza
- Mantiene escala tipográfica consistente: 12px (xs), 14px (sm), 16px (base), 18px (lg), 20px (xl), 24px, 32px
- Line-height: 1.5 para texto de cuerpo, 1.2-1.4 para encabezados
- Letter-spacing: normal para cuerpo, ajusta para encabezados si es necesario
- Asegúra longitud de línea suficiente para legibilidad (50-80 caracteres)
- Usa `font-weight: 500+` para elementos interactivos para mejorar visibilidad

## Feedback del Usuario

- Proporciona feedback visual claro para todas las interacciones (hover, focus, active)
- Usa indicadores de estado consistentes: loading, disabled, error, success
- Muestra errores de validación cerca del input, no al inicio del formulario
- Proporciona mensajes de error útiles: "El nombre debe tener 2-50 caracteres"
- Usa color + icono/texto para el significado (no relies solo en el color)
- Implementa notificaciones toast o confirmaciones de éxito

## Consistencia

- Sigue los design tokens del sistema de diseño establecido
- Usa el mismo componente para la misma funcionalidad en todo el proyecto
- Mantiene espaciado consistente entre elementos relacionados
- Aplica estilos consistentes a elementos interactivos
- Sigue las convenciones de nombres establecidas en el equipo
- Documenta las desviaciones del design system con justificación

## Testing y Validación

- Testea con navegación solo por teclado (sin ratón)
- Testea con lector de pantalla habilitado (NVDA en Windows)
- Valida contraste de color con WebAIM o axe DevTools
- Comprueba el diseño responsivo en dispositivos/navegadores reales
- Testea con `prefers-reduced-motion: reduce` habilitado
- Verifica que todos los inputs de formulario tengan etiquetas asociadas
- Valida HTML semántico con el validador W3C
