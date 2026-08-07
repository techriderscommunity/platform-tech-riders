# angular-production-review

## Description

Revisa o implementa Angular moderno con standalone, signals, lazy loading, accesibilidad, performance y tests.

## When to use

Usa esta skill para features Angular nuevas, refactors de modulos legacy, ajustes de routing/guards/interceptors y problemas de rendimiento en Angular.

## Instructions

1. Lee el contexto existente antes de proponer cambios.
2. Prioriza standalone components y boundaries por dominio.
3. Usa signals para estado local cuando simplifiquen el flujo.
4. Asegura estados de loading, empty, error y retry.
5. Incluye validacion de accesibilidad en formularios y foco.
6. Evalua impacto en bundle/render y define pruebas necesarias.

## Output esperado

- Decision tecnica recomendada y alternativa descartada.
- Estructura de feature (componentes, servicios, rutas).
- Plan de implementacion por fases cortas.
- Plan de tests (unit/integration/e2e segun riesgo).
- Riesgos tecnicos y mitigaciones.

## Checklist

- [ ] Tipos correctos.
- [ ] Standalone + lazy routing aplicados donde procede.
- [ ] Sin subscriptions manuales innecesarias.
- [ ] Estados loading/error/empty cubiertos.
- [ ] Formularios accesibles y con errores claros.
- [ ] Performance validada sin regresiones.
- [ ] Tests definidos por criticidad.
