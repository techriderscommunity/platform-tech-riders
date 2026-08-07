# Migration and Modernization Instructions

## Objetivo

Modernizar frontend legacy con estrategia incremental, bajo riesgo operativo y valor entregable en cada fase.

## Cuando aplicar

- Migraciones AngularJS o Angular/React legacy.
- Actualizaciones de stack obsoleto.
- Reduccion de deuda estructural de alto impacto.

## Reglas operativas

- Evalua baseline funcional y de rendimiento antes de migrar.
- Define arquitectura target y arquitectura transitoria.
- Divide migracion en fases pequenas con criterios de salida.
- Implementa convivencia temporal con adaptadores cuando sea necesario.
- Incluye plan de rollback para cada fase relevante.

## Checklist de calidad

- Inventario de modulos y criticidad actualizado.
- Riesgos tecnicos y de negocio priorizados.
- Pruebas de no regresion por fase.
- Observabilidad intacta durante la transicion.
- ADR de decisiones estructurales.

## Criterios de salida

- Roadmap incremental aprobado.
- Valor funcional por fase definido.
- Riesgo residual documentado.
- Condiciones de retirada de legacy claras.

## Anti-patrones a bloquear

- Big-bang rewrite sin hitos.
- Migrar sin baseline ni metricas.
- Mezclar legacy y moderno sin frontera.
- Cerrar fases sin validacion objetiva.
