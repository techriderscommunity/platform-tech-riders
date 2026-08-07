# Testing Instructions

## Objetivo

Construir una estrategia de pruebas por riesgo que maximize confianza de release con tiempo de CI sostenible.

## Cuando aplicar

- Cualquier feature o fix con impacto funcional.
- Definicion o ajuste de quality gates de CI.
- Reduccion de flakiness o de tiempo de suites.

## Reglas operativas

- Mapea riesgos a nivel de test adecuado.
- Unit para logica pura; integration para contratos de capa; e2e para journeys criticos.
- Prioriza legibilidad y determinismo sobre cantidad de tests.
- Usa mocks minimos y realistas para no ocultar errores.
- Revisa cobertura de escenarios negativos y edge cases.

## Checklist de calidad

- Plan de pruebas alineado con riesgo de negocio.
- Casos de error, vacio y permisos contemplados.
- Flujos criticos cubiertos en e2e.
- Estrategia anti-flakiness documentada.
- Umbrales de calidad definidos para merge.

## Criterios de salida

- Suite propuesta ejecutable y mantenible.
- Gaps de cobertura explicitos.
- Riesgo residual de testing documentado.
- Recomendacion de quality gates para CI.

## Anti-patrones a bloquear

- Cobertura porcentual como objetivo unico.
- E2E para todo el sistema.
- Tests acoplados a detalles internos inestables.
- Suites lentas sin segmentacion por riesgo.
