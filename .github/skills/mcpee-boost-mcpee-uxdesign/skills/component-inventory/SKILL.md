---
name: 'component-inventory'
description: 'Cataloga y gestiona componentes UI en tu codebase. Crea inventarios, detecta duplicados, identifica oportunidades de reutilizabilidad y genera informes de uso de componentes con documentación visual.'
---

# Skill de Inventario de Componentes

Este skill te ayuda a entender y optimizar el panorama de tus componentes.

## Capacidades

### 1. Detección y Catalogación de Componentes
- Descubre automáticamente todos los componentes UI
- Extrae metadatos del componente (props, exports, dependencias)
- Genera inventario de componentes
- Rastrea ubicaciones y relaciones de componentes

### 2. Análisis e Insights
- Detecta componentes duplicados o similares
- Identifica oportunidades de reutilizabilidad
- Analiza dependencias de componentes
- Genera estadísticas de uso de componentes
- Encuentra componentes huérfanos o sin uso

### 3. Identificación de Librerías
- Detecta las librerías de componentes en uso (Material-UI, Chakra, Shadcn, etc.)
- Analiza componentes custom vs. componentes de librería
- Identifica librerías mixtas o en conflicto
- Genera informe de uso de librerías

### 4. Generación de Documentación
- Crea inventario de componentes con descripciones
- Genera ejemplos de uso de componentes
- Crea documentación de API de componentes
- Genera gráficos de dependencias de componentes
- Crea catálogo visual de componentes

### 5. Recomendaciones de Reutilizabilidad
- Identifica componentes que deberían extraerse
- Sugiere composiciones de componentes
- Recomienda abstracciones
- Guía esfuerzos de refactoring

## Artefactos de Salida

### Archivos de Inventario
- `component-inventory.json` - Complete component catalog
- `component-map.md` - Human-readable component map
- `components.html` - Visual component gallery

### Informes de Análisis
- `duplicates.json` - Duplicate component analysis
- `unused-components.md` - Potentially unused components
- `reusability-opportunities.md` - Refactoring recommendations
- `library-report.md` - Library usage analysis
- `dependency-graph.html` - Component dependency visualization

### Documentación
- `COMPONENT_GUIDE.md` - Component usage guide
- `REFACTORING_PLAN.md` - Component improvements roadmap

## Formato del Inventario de Componentes

```json
{
  "components": [
    {
      "name": "Button",
      "path": "src/components/Button/Button.tsx",
      "type": "functional",
      "props": [
        {"name": "variant", "type": "string", "required": false},
        {"name": "size", "type": "string", "required": false},
        {"name": "onClick", "type": "function", "required": false}
      ],
      "description": "Primary button component",
      "usageCount": 42,
      "lastModified": "2024-01-15",
      "library": "custom",
      "exports": ["Button", "ButtonGroup"]
    }
  ],
  "libraries": ["@mui/material", "custom"],
  "stats": {
    "total": 125,
    "custom": 95,
    "library": 30,
    "duplicates": 3
  }
}
```

## Uso

```bash
# Generate component inventory
npm run component:inventory

# Analyze for duplicates
npm run component:analyze-duplicates

# Find unused components
npm run component:find-unused

# Generate visual catalog
npm run component:generate-catalog

# Create refactoring plan
npm run component:refactoring-plan
```

## Tipos de Archivo Soportados

- **React**: `.tsx`, `.jsx`, `.ts`, `.js`
- **Angular**: `.component.ts`, `.component.html`
- **Vue**: `.vue`
- **Web Components**: `.ts`, `.js`

## Reglas de Detección

Scans for:
- React functional and class components
- Angular component decorators
- Vue single-file components
- Web components (custom elements)
- Component exports and default exports

## Analysis Features

### Duplication Detection
- Identifies components with similar props and structure
- Suggests consolidation opportunities
- Provides side-by-side comparison

### Unused Detection
- Tracks component imports
- Identifies components with zero usage
- Filters out utility/utility-component patterns
- Generates safe-to-remove list

### Reusability Scoring
- Scores components by reusability potential
- Prioritizes high-impact refactoring
- Suggests component hierarchies
- Identifies composition patterns

## Integration Points

- **Figma**: Map components to design system
- **Documentation**: Auto-generate component guides
- **CI/CD**: Track component metrics over time
- **Testing**: Generate component test templates
- **Linting**: Enforce component standards

## Output Examples

### Component Card
```markdown
## Button

**Path**: `src/components/Button/Button.tsx`
**Usage**: 42 instances across 15 files
**Type**: Custom component
**Props**: `variant`, `size`, `onClick`, `disabled`, `children`

Versatile button component used for all interactive button needs.
Supports multiple sizes and variants.

### Example
```tsx
<Button variant="primary" size="lg" onClick={handleClick}>
  Click me
</Button>
```
```

## Best Practices

1. **Regular Updates**: Re-run inventory monthly
2. **Track Metrics**: Monitor component growth
3. **Document Standards**: Define component patterns
4. **Refactor Incrementally**: Address high-impact duplicates first
5. **Version Components**: Track component changes
6. **Test Coverage**: Ensure all components have tests

## Related Skills

- `design-system-generator` - Create systems from components
- `figma-integration` - Map to design system
- `ux-audit` - Audit component consistency
- `screenshot-reporter` - Generate visual documentation

## Next Steps

1. **Review Inventory**: Understand current component landscape
2. **Identify Opportunities**: Find quick wins for consolidation
3. **Plan Refactoring**: Create prioritized improvement plan
4. **Execute**: Implement changes incrementally
5. **Document**: Update component guide with findings
