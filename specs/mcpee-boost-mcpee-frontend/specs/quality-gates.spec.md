# quality-gates.spec.md

## Proposito

Definir quality gates de CI/CD para proteger merge y release con señal util, tiempos razonables y cobertura por riesgo.

## Ambito

- Validaciones previas a merge y previas a release.
- Type/lint/tests/a11y/performance/security.
- Criterios de bloqueo y excepciones temporales.

## Decisiones estandar

1. Merge gate: typecheck + lint + tests minimos por riesgo.
2. Release gate: pruebas de regresion y umbrales de performance.
3. A11y y seguridad con verificacion automatizada y manual selectiva.
4. Escalado de pruebas segun criticidad del cambio.
5. Excepciones con caducidad y owner obligatorio.

## Reglas obligatorias

- Ningun merge sin typecheck y lint en verde.
- Cambios de alto riesgo requieren e2e/regresion.
- Performance budget definido para bundles criticos.
- Dependency scan y revisiones de seguridad activas.
- PR checklist completado antes de merge.

## Antipatrones a bloquear

- Gates simbolicos sin enforcement real.
- Cobertura porcentual como unico criterio.
- E2E indiscriminados que degradan velocidad sin valor.
- Excepciones permanentes sin fecha de expiracion.

## Checklist de validacion

- [ ] La decision esta documentada.
- [ ] Hay owner claro.
- [ ] Gates de merge definidos.
- [ ] Gates de release definidos.
- [ ] Impacto en DX evaluado.
- [ ] Impacto en seguridad evaluado.
- [ ] Impacto en performance evaluado.
- [ ] Politica de excepciones definida.

## Evidencias esperadas

- Matriz de gates por tipo de cambio.
- Umbrales y condiciones de bloqueo.
- Historial de fallos de gate y acciones.
- Checklist de PR aplicado.
