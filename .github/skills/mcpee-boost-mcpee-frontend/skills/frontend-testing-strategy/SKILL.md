# frontend-testing-strategy

## Description

Define estrategia equilibrada de tests para frontend enterprise.

## When to use

Usa esta skill para definir estrategia de tests por riesgo, quality gates de CI y mitigacion de flakiness.

## Instructions

1. Lee el contexto existente antes de proponer cambios.
2. Clasifica riesgos funcionales, de negocio y de release.
3. Mapea riesgo a nivel de test correcto (unit/integration/e2e).
4. Define datos de prueba y politica de mocks.
5. Incluye estrategia anti-flakiness.
6. Define quality gates con umbrales claros.

## Output esperado

- Matriz riesgo -> tipo de test.
- Casos criticos obligatorios.
- Propuesta de quality gates CI.
- Plan de reduccion de flakiness.
- Riesgo residual de testing.

## Checklist

- [ ] Tipos correctos.
- [ ] Cobertura ligada a riesgo real.
- [ ] E2E limitados a journeys criticos.
- [ ] Suites estables y deterministas.
- [ ] Mocks minimos y controlados.
- [ ] Gates CI definidos.
