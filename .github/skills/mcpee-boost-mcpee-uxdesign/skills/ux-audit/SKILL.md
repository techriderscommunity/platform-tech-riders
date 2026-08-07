---
name: 'ux-audit'
description: 'Realiza auditorías UX/UI completas incluyendo accesibilidad (WCAG), consistencia de diseño, diseño responsivo, patrones de usabilidad y mejores prácticas. Genera informes detallados de auditoría con recomendaciones priorizadas.'
---

# Skill de Auditoría UX/UI

Este skill te ayuda a evaluar y mejorar la calidad UX/UI de tu aplicación.

## Áreas de Auditoría

### 1. Accesibilidad (WCAG 2.1)

**Verificaciones de Cumplimiento Nivel AA:**
- Ratios de contraste de color (4.5:1 para texto normal, 3:1 para texto grande)
- Soporte de navegación por teclado
- Compatibilidad con lectores de pantalla
- Visibilidad de indicadores de foco
- Corrección de etiquetas y roles ARIA
- Asociaciones de etiquetas de formulario
- Claridad de mensajes de error
- Tamaños de touch targets (mínimo 44x44px)
- Efectos de movimiento y animación
- Atributos de idioma

**Herramientas Usadas:**
- axe-core para escaneo automatizado
- Testing manual de navegación por teclado
- Testing con lector de pantalla (NVDA, JAWS)
- Validadores de contraste de color

### 2. Consistencia de Diseño

**Verificaciones:**
- Consistencia visual de componentes
- Adherencia a la escala de espaciado
- Consistencia de escala tipográfica
- Uso de paleta de colores
- Consistencia del sistema de elevación/sombras
- Consistencia de border radius
- Consistencia de estilo de iconos
- Consistencia de patrones de interacción

**Entregable:** Informe de consistencia con comparaciones visuales

### 3. Diseño Responsivo

**Breakpoints Testeados:**
- Móvil (320px, 375px, 425px)
- Tablet (640px, 768px)
- Escritorio (1024px, 1280px, 1440px+)

**Verificaciones:**
- Adaptación del layout en breakpoints
- Legibilidad del texto en todos los tamaños
- Touch targets adecuados en móvil
- Manejo de overflow
- Escalado de imágenes
- Patrones de navegación

### 4. Patrones de Experiencia de Usuario

**Verificaciones:**
- Claridad e intuitividad de la navegación
- Usabilidad de formularios y validación
- Manejo de errores y recuperación
- Estados de carga y feedback
- Diseño de estados vacíos
- Ayuda y orientación (tooltips, etc.)
- Claridad de call-to-action
- Evaluación de carga cognitiva

### 5. Rendimiento

**Métricas:**
- Core Web Vitals (LCP, FID, CLS)
- First Contentful Paint (FCP)
- Time to Interactive (TTI)
- Cumulative Layout Shift (CLS)

### 6. Mejores Prácticas

**Verificaciones:**
- Enfoque mobile-first
- Mejora progresiva
- Uso de HTML semántico
- Mejores prácticas CSS
- Calidad de composición de componentes
- Organización del código

## Proceso de Auditoría

### Fase 1: Preparación
- Definir el alcance de la auditoría
- Identificar los recorridos clave del usuario
- Recopilar métricas base
- Establecer criterios de éxito

### Fase 2: Análisis Automatizado
- Ejecutar scanner de accesibilidad (axe)
- Verificar contrastes de color
- Validar diseño responsivo
- Medir métricas de rendimiento
- Analizar consistencia de componentes

### Fase 3: Testing Manual
- Testear navegación por teclado
- Testear con lectores de pantalla
- Testear en dispositivos reales
- Testear flujos de usuario
- Verificar casos extremos

### Fase 4: Análisis
- Categorizar hallazgos por severidad
- Cuantificar el impacto
- Priorizar problemas
- Crear hoja de ruta de mejora

### Fase 5: Informe
- Generar informe completo
- Crear documentación visual
- Proporcionar recomendaciones
- Establecer métricas de éxito

## Artefactos de Salida

### Informes de Auditoría
- `ux-audit-report.md` - Resumen ejecutivo
- `accessibility-report.json` - Hallazgos WCAG
- `consistency-report.json` - Consistencia de diseño
- `responsive-report.json` - Resultados de diseño responsivo
- `performance-report.json` - Métricas de rendimiento

### Análisis Detallado
- `issues-by-priority.md` - Problemas ordenados por severidad
- `issues-by-component.md` - Problemas organizados por componente
- `recommendations.md` - Plan de mejora priorizado
- `metrics-dashboard.html` - Dashboard visual de métricas

### Evidencia
- `screenshots/` - Capturas de evidencia de problemas
- `a11y-violations/` - Detalles de violaciones de accesibilidad
- `responsive-tests/` - Evidencia de diseño responsivo

## Formato del Informe

```markdown
# UX/UI Audit Report

**Date**: 2024-01-15
**Auditor**: UX Audit Skill
**Scope**: Full application

## Executive Summary

- **Overall Score**: 72/100
- **Critical Issues**: 5
- **Major Issues**: 12
- **Minor Issues**: 8
- **Compliant Areas**: 15

## Key Findings

### 1. Accessibility

**Status**: ⚠️ Needs Improvement

- ❌ 3 color contrast violations
- ⚠️ 2 missing ARIA labels
- ✅ Keyboard navigation working
- ✅ Form labels properly associated

**Impact**: High - Affects ~15% of users
**Priority**: CRITICAL
**Effort**: 2-3 days

### 2. Responsive Design

**Status**: ✅ Good

- ✅ Mobile layout (375px) functional
- ✅ Tablet layout (768px) functional
- ⚠️ Desktop spacing could be optimized
- ✅ Touch targets adequate

**Impact**: Medium
**Priority**: MEDIUM
**Effort**: 1-2 days

### 3. Design Consistency

**Status**: ⚠️ Partial

- ⚠️ 4 inconsistent button styles
- ✅ Spacing scale mostly adhered
- ⚠️ Typography scale has exceptions
- ✅ Color palette used correctly

**Impact**: Medium
**Priority**: MEDIUM
**Effort**: 3-4 days

### 4. Performance

**Metrics**:
- LCP: 2.8s (Target: <2.5s) ⚠️
- FID: 85ms (Target: <100ms) ✅
- CLS: 0.08 (Target: <0.1) ✅

**Impact**: Medium
**Priority**: MEDIUM
**Effort**: 2-3 days

## Recommendations

### Phase 1: Critical (Next Sprint)
1. Fix color contrast violations
2. Add missing ARIA labels
3. Optimize LCP performance

### Phase 2: Important (Within 2 Sprints)
1. Standardize button styles
2. Update typography scale
3. Optimize tablet spacing

### Phase 3: Nice-to-have (Backlog)
1. Enhance loading states
2. Improve empty state designs
3. Add more micro-interactions

## Success Metrics

| Metric | Current | Target | Impact |
|--------|---------|--------|--------|
| WCAG AA Score | 72% | 95% | High |
| Performance (LCP) | 2.8s | <2.5s | Medium |
| Design Consistency | 80% | 95% | Medium |
```

## Niveles de Severidad

- **CRÍTICO**: Bloquea la tarea del usuario, violación de accesibilidad, problema mayor de usabilidad
- **MAYOR**: Impacta la eficiencia, viola pautas, confuso
- **MENOR**: Problema de polish, inconsistente con pautas, caso extremo
- **MEJORA**: Oportunidad de mejora, nice-to-have

## Herramientas y Tecnologías

- **axe-core**: Escaneo automatizado de accesibilidad
- **Playwright**: Testing multi-navegador y capturas
- **Lighthouse**: Rendimiento y mejores prácticas
- **WebAIM**: Validación de contraste de color
- **WAVE**: Evaluación de accesibilidad

## Integración

### Auditoría Continua

```bash
# Run audit in CI/CD
npm run audit:ux-ui

# Schedule regular audits
npm run audit:schedule --frequency weekly
```

### Seguimiento de Métricas

```typescript
// Track metrics over time
const auditHistory = await getAuditHistory();
// Monitor improvements
const trend = calculateTrend(auditHistory);
```

## Mejores Prácticas

1. **Establecer Línea Base**: Empieza con una auditoría inicial completa
2. **Auditorías Regulares**: Repite mensualmente o tras cambios importantes
3. **Seguir el Progreso**: Monitoriza la mejora de métricas con el tiempo
4. **Priorizar**: Enfocáte en problemas de alto impacto y fácil corrección primero
5. **Documentar**: Conserva los informes de auditoría para referencia
6. **Iterar**: Enfoque de mejora continua
7. **Involucrar al Equipo**: La accesibilidad es responsabilidad de todos

## Skills Relacionados

- `screenshot-reporter` - Genera evidencia visual
- `design-system-generator` - Crea sistemas de diseño consistentes
- `component-inventory` - Audita consistencia de componentes
- `figma-integration` - Compara con especificaciones de diseño

## Problemas Comunes y Soluciones

### Contraste de Color
- Usa validadores de contraste antes de publicar
- Define niveles mínimos de contraste en el design system
- Testea en modos claro y oscuro

### Navegación por Teclado
- Use semantic HTML buttons/links
- Implement visible focus indicators
- Test tab order regularly

### Responsive Design
- Test on actual devices
- Use viewport meta tags
- Implement responsive images

### Performance
- Optimize images and bundles
- Implement lazy loading
- Monitor Core Web Vitals

## Next Steps

1. **Run Initial Audit**: Get baseline metrics
2. **Review Findings**: Understand key issues
3. **Prioritize**: Create improvement roadmap
4. **Execute**: Fix issues incrementally
5. **Measure**: Track improvements
6. **Iterate**: Continuous improvement cycle
