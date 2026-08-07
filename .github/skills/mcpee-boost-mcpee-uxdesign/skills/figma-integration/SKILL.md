---
name: 'figma-integration'
description: 'Integra archivos de diseño Figma con tu codebase. Extrae design tokens, mapea componentes entre diseño y código, y genera especificaciones de diseño y documentación de sincronización.'
---

# Skill de Integración Figma

Este skill permite una conexión fluida entre los diseños Figma y la implementación de tu codebase.

## Capacidades

### 1. Extracción de Design Tokens
- Extrae colores, tipografía y espaciado desde Figma
- Genera definiciones de tokens (variables CSS, JSON, etc.)
- Crea sistemas de temas (soporte modo claro/oscuro)
- Mapea variables Figma a código

### 2. Mapeo de Componentes
- Mapea componentes Figma a componentes de código
- Genera especificaciones de componentes
- Rastrea la consistencia diseño-código
- Identifica implementaciones faltantes

### 3. Análisis de Diseño
- Analiza el design system en Figma
- Identifica patrones y abstracciones de diseño
- Genera documentación de diseño
- Resalta inconsistencias

### 4. Configuración de Sync
- Configura la conexión a la API de Figma
- Crea workflows de sincronización
- Genera scripts de integración CI/CD
- Monitoriza cambios de diseño

## Configuración

### Requisitos Previos
1. Cuenta Figma con acceso a la API
2. Personal Access Token (https://www.figma.com/developers/api#access-tokens)
3. ID o URL del archivo Figma

### Configuración

```bash
# Set your Figma token (secure storage recommended)
export FIGMA_ACCESS_TOKEN="your-token-here"
export FIGMA_FILE_ID="your-file-id"
```

## Uso

### Extraer Design Tokens

```javascript
// Use the Figma API to extract tokens
const tokens = await extractFigmaTokens({
  fileId: process.env.FIGMA_FILE_ID,
  token: process.env.FIGMA_ACCESS_TOKEN,
  groups: ['colors', 'typography', 'spacing', 'shadows']
});
```

### Mapear Componentes

```javascript
const componentMap = await mapFigmaComponents({
  figmaFile: FIGMA_FILE_ID,
  codebaseRoot: './src',
  outputFormat: 'json'
});
```

### Generar Documentación

```bash
# Generate design specifications from Figma
npm run figma:generate-specs
```

## Artefactos de Salida

- `design-tokens.json` - Definiciones de tokens
- `component-map.json` - Mapeo Figma-código
- `design-specs.md` - Especificaciones de diseño
- `theme-config.ts` - Configuración de tema
- `sync-guide.md` - Documentación de configuración de sync

## Puntos de Integración

- **Figma API**: Extrae datos de diseño
- **Herramientas de Design System**: Generación de tokens (Design Tokens, Tokens Studio, etc.)
- **CI/CD**: Sync automatizado de tokens en cambios de Figma
- **Generación de Código**: Auto-genera stubs de componentes desde Figma

## Mejores Prácticas

1. Mantener Figma como única fuente de verdad para design tokens
2. Sincronizar tokens desde Figma al codebase regularmente
3. Documentar todos los cambios de tokens
4. Testear implementaciones de componentes contra las especificaciones Figma
5. Versionar tokens junto con cambios de código
6. Mantener documentación de mapeo diseño-código

## Solución de Problemas

### Token No Sincroniza
- Verifica que el Figma Personal Access Token sea válido
- Comprueba que el token tenga los scopes correctos
- Asegúra que el ID del archivo Figma sea correcto
- Revisa los límites de tasa de la API de Figma

### Problemas de Mapeo de Componentes
- Verifica que las convenciones de nombres de componentes coincidan
- Comprueba que los componentes de código existan antes de mapear
- Asegúra que los componentes Figma estén correctamente estructurados
- Revisa las especificaciones de componentes

## Skills Relacionados

- `design-system-generator` - Create design systems from tokens
- `component-inventory` - Catalog and manage components
- `ux-audit` - Audit design consistency against specifications
