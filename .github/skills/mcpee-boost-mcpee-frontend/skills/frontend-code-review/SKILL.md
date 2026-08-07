# frontend-code-review

## Description

Ejecuta code review frontend severa orientada a defectos reales, seguridad, regresiones y riesgo de release.

## When to use

Usa esta skill para revisar PRs antes de merge, cambios de alto impacto o sospecha de regresiones en produccion.

## Instructions

1. Revisa primero seguridad, auth y manejo de datos.
2. Evalua defectos funcionales y regresiones de UX.
3. Analiza impacto en accesibilidad, performance y mantenibilidad.
4. Verifica cobertura de pruebas y gaps relevantes.
5. Clasifica hallazgos por severidad (Critical/High/Medium/Low).
6. Emite recomendacion de merge con riesgo residual.

## Output esperado

- Hallazgos por severidad con evidencia.
- Propuesta de fix para hallazgos relevantes.
- Gaps de test y escenarios faltantes.
- Riesgo residual para decision de merge.
- Acciones bloqueantes y no bloqueantes.

## Checklist

- [ ] Seguridad y auth revisadas.
- [ ] Manejo de errores y estados vacios.
- [ ] Impacto a11y/performance evaluado.
- [ ] Pruebas existentes y faltantes identificadas.
- [ ] Recomendacion final de merge clara.
