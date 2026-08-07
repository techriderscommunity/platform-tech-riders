# Design System Instructions

## Objetivo

Construir y evolucionar un design system reutilizable con tokens estables, componentes accesibles y governance sostenible.

## Cuando aplicar

- Definicion o evolucion de tokens y temas.
- Creacion o refactor de componentes base.
- Planes de adopcion del design system en equipos producto.

## Reglas operativas

- Define capas de tokens: global, semantic y component.
- Mantiene APIs de componentes pequenas y predecibles.
- Asegura estados interactivos completos por defecto.
- Prioriza accesibilidad y rendimiento en componentes base.
- Documenta cambios y deprecaciones con versionado semantico.

## Checklist de calidad

- Taxonomia de tokens clara y consistente.
- Contratos de componentes con variantes y estados definidos.
- Compatibilidad Angular/React cuando aplique.
- Guia de uso y ejemplos de composicion reales.
- Plan de adopcion incremental por equipos.

## Criterios de salida

- Decision de arquitectura de design system documentada.
- Riesgos de adopcion y mitigaciones explicitos.
- Cambios listos para release con versionado coherente.
- Impacto en accesibilidad y performance evaluado.

## Anti-patrones a bloquear

- Componentes opinionados para casos puntuales.
- Props ambiguas o redundantes.
- Theming por overrides locales sin contrato.
- Releases sin estrategia de deprecacion.
