# angular-modern.spec.md

## Proposito

Definir el estandar Angular de produccion para asegurar consistencia entre equipos en arquitectura de features, DX, rendimiento y calidad de release.

## Ambito

- Angular LTS.
- Aplicaciones SPA y shells de microfrontends en Angular.
- Nuevas features y refactors de modulos existentes.

## Decisiones estandar

1. Arquitectura por dominio, no por tipo tecnico.
2. Standalone components por defecto.
3. Estado local con signals cuando simplifique el flujo.
4. Lazy loading en rutas no criticas de arranque.
5. Servicios acotados por bounded context.

## Reglas obligatorias

- Cada feature define estados de loading, empty, error y retry.
- Todo formulario expone errores accesibles y validaciones consistentes.
- No introducir NgModules nuevos salvo excepcion documentada.
- No usar any sin justificacion y plan de correccion.
- Cambios de alto impacto arquitectonico requieren ADR.

## Antipatrones a bloquear

- God services con logica de multiples dominios.
- Subscriptions manuales sin limpieza o sin necesidad.
- Estado duplicado entre componente y servicio sin ownership claro.
- Rutas pesadas en carga inicial sin justificacion.

## Checklist de validacion

- [ ] La decision esta documentada.
- [ ] Hay owner claro.
- [ ] Estructura de feature por dominio definida.
- [ ] Impacto en DX evaluado.
- [ ] Impacto en accesibilidad evaluado.
- [ ] Impacto en performance evaluado.
- [ ] Impacto en seguridad evaluado.
- [ ] Tests asociados por criticidad.

## Evidencias esperadas

- Diff o blueprint de estructura de feature.
- Plan de test unit/integration/e2e segun riesgo.
- Evidencia de no regresion en rendimiento.
- Validacion de accesibilidad en elementos interactivos.
