---
name: 'design-system-generator'
description: 'Genera sistemas de diseño completos desde cero o mejora los existentes. Crea design tokens, patrones de componentes, documentación y style guides adaptados a las necesidades de tu proyecto.'
---

# Skill Generador de Design System

Este skill te ayuda a construir design systems escalables y mantenibles para cualquier proyecto siguiendo estándares y mejores prácticas líderes de la industria.

## Fundamentos y Referencias

### Estándares y Recursos Oficiales
- **Design Tokens**: https://www.figma.com/resource-library/design-tokens/
- **W3C Design Tokens**: https://github.com/design-tokens/community-group
- **Material Design 3**: https://m3.material.io/
- **Shopify Polaris**: https://polaris.shopify.com/
- **WCAG 2.1**: https://www.w3.org/WAI/WCAG21/quickref/
- **Figma Design System Guide**: https://www.figma.com/resource-library/what-is-a-design-system/

## Capacidades

### 1. Generación del Sistema de Tokens
- Crea jerarquía de design tokens
- Genera escalas de espaciado
- Define paletas de color con validación de contraste
- Crea escalas tipográficas
- Genera sistemas de sombras/elevación
- Soporta variables CSS, SCSS, Tailwind, formatos de tokens

### 2. Definición de Patrones de Componentes
- Define patrones de componentes reutilizables
- Crea patrones de composición
- Establece patrones de gestión de estado
- Documenta props y slots de componentes
- Genera plantillas de componentes

### 3. Documentación de Diseño
- Genera documentación del design system
- Crea pautas de uso
- Construye galerías de componentes
- Genera pautas de accesibilidad
- Crea pautas de marca

### 4. Cumplimiento de Accesibilidad
- Genera patrones con cumplimiento WCAG 2.1 AA
- Crea validación de contraste de color
- Define tamaños de touch targets
- Genera patrones de navegación por teclado
- Crea pautas para lectores de pantalla

### 5. Soporte de Temas
- Genera sistemas de temas claro/oscuro
- Crea mecanismos de cambio de tema
- Soporta múltiples temas de marca
- Genera documentación de temas

## Enfoques Soportados

- **Desde Cero**: Nuevo design system adaptado a tu visión
- **Mejorar Existente**: Mejorar y formalizar componentes existentes
- **Migrar**: Convertir de una librería a otra
- **Multi-Marca**: Soporte para múltiples marcas en un solo sistema

## Artefactos de Salida

### Design Tokens
- `tokens.json` - Definiciones de design tokens
- `variables.css` - Custom properties CSS
- `theme.ts` - Configuración de tema TypeScript

### Documentación
- `design-system.md` - Visión general y principios del sistema
- `typography.md` - Pautas tipográficas
- `color-system.md` - Paleta de colores y uso
- `spacing.md` - Pautas de espaciado y layout
- `components/` - Documentación individual de componentes

### Implementación
- `tokens-config.ts` - Configuración de tokens
- `theme-provider.tsx` - Componente theme provider
- `use-theme.ts` - Hook de tema
- `styles.css` - Estilos globales

## Proceso

1. **Recopilar Requisitos**
   - Entender pautas de marca
   - Revisar componentes existentes
   - Definir principios de diseño
   - Identificar patrones de componentes

2. **Definir Tokens**
   - Crear escala de espaciado
   - Definir paleta de colores
   - Establecer tipografía
   - Crear sistema de elevación

3. **Documentar Patrones**
   - Documentar cada tipo de componente
   - Definir interacciones y estados
   - Crear pautas de composición
   - Especificar requisitos de accesibilidad

4. **Generar Implementaciones**
   - Crear implementaciones de tokens
   - Generar plantillas de componentes
   - Construir sitio de documentación
   - Crear sistemas de temas

5. **Validar y Testear**
   - Testear cumplimiento de accesibilidad
   - Verificar ratios de contraste
   - Testear comportamientos responsivos
   - Documentar casos extremos

## Uso

```bash
# Generate new design system
npm run design-system:generate

# Generate tokens from Figma
npm run design-system:tokens-from-figma

# Generate documentation
npm run design-system:docs

# Validate accessibility
npm run design-system:validate-a11y
```

## Estructura de Tokens de Ejemplo

```json
{
  "spacing": {
    "xs": "4px",
    "sm": "8px",
    "md": "16px",
    "lg": "24px",
    "xl": "32px"
  },
  "typography": {
    "body-sm": {"size": "14px", "weight": 400, "lineHeight": "20px"},
    "body-md": {"size": "16px", "weight": 400, "lineHeight": "24px"},
    "heading-lg": {"size": "24px", "weight": 600, "lineHeight": "32px"}
  },
  "colors": {
    "primary": "#0066CC",
    "secondary": "#00AA44",
    "error": "#CC0000"
  }
}
```

## Puntos de Integración

- **Figma**: Extrae design tokens desde Figma
- **CSS/SCSS**: Genera definiciones de variables CSS
- **Librerías de Componentes**: MUI, Chakra, Shadcn, custom
- **Motores de Temas**: Soporte para distintas soluciones de theming
- **Documentación**: Auto-genera sitios web del design system
- **CI/CD**: Valida y sincroniza design tokens

## Mejores Prácticas

1. Mantener tokens como única fuente de verdad
2. Versionar tokens con versionado semántico
3. Documentar todas las decisiones de diseño
4. Testear cumplimiento de accesibilidad
5. Proporcionar guías de migración claras
6. Mantener compatibilidad hacia atrás cuando sea posible
7. Revisiones y actualizaciones regulares

## Ejemplos

- **Sistema Material Design** - Basado en principios de Material Design
- **Sistema Mínimo** - Sistema ligero para proyectos pequeños
- **Sistema Enterprise** - Sistema completo con múltiples temas
- **Migración de Librería de Componentes** - Mejora una librería existente

## Skills Relacionados

- `figma-integration` - Extrae tokens desde Figma
- `component-inventory` - Cataloga componentes
- `ux-audit` - Valida el cumplimiento del design system
- `screenshot-reporter` - Documentación visual del design system
