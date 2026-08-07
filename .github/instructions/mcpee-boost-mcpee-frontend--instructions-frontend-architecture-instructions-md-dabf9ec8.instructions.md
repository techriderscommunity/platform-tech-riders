# Frontend Architecture Instructions

## Objetivo

Tomar decisiones de arquitectura frontend de alto impacto con trade-offs explicitos, escalabilidad de equipos y control de riesgo.

## Cuando aplicar

- Definicion de arquitectura base del producto.
- Decisiones de boundaries, monorepo/multirepo o microfrontends.
- Cambios estructurales con alto costo de reversa.

## Reglas operativas

- Distingue decisiones reversibles vs irreversibles.
- Presenta decision recomendada y alternativas descartadas.
- Define ownership tecnico y contratos entre dominios.
- Establece quality gates de lint, test, a11y, performance y seguridad.
- Documenta decisiones relevantes en ADR.

## Checklist de calidad

- Boundaries de dominio sin acoplamiento circular.
- Contratos y versionado definidos entre modulos.
- Plan de rollout incremental con rollback.
- KPI de exito y alertas de regresion definidos.
- Riesgos de negocio y tecnicos explicitados.

## Criterios de salida

- ADR listo con decision y trade-offs.
- Plan de adopcion por fases.
- Riesgo residual y mitigaciones claros.
- Validacion de factibilidad operativa.

## Anti-patrones a bloquear

- Cambios estructurales sin ADR.
- Shared libs sin ownership claro.
- Dependencias internas sin contrato.
- Migraciones masivas sin estrategia de contingencia.
