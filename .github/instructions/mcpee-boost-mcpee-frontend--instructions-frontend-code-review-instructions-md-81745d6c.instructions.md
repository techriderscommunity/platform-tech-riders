# Frontend Code Review Instructions

## Objetivo

Ejecutar revisiones tecnicas con foco en bugs, seguridad, regresiones funcionales y deuda que afecte produccion.

## Cuando aplicar

- Revision de PR antes de merge.
- Cambios de alto riesgo o alta complejidad.
- Auditorias puntuales de calidad tecnica.

## Reglas operativas

- Reporta hallazgos por severidad primero.
- Prioriza defectos funcionales y de seguridad sobre estilo.
- Vincula cada hallazgo a evidencia tecnica concreta.
- Propone fix viable para hallazgos relevantes.
- Identifica gaps de test y riesgo residual.

## Checklist de calidad

- Seguridad, auth y datos revisados primero.
- Estados de error, vacio y permisos cubiertos.
- Tipado estricto sin bypass peligrosos.
- Impacto en rendimiento y accesibilidad verificado.
- Cobertura de tests proporcional al riesgo.

## Criterios de salida

- Lista de hallazgos ordenada por criticidad.
- Recomendacion de merge/no-merge argumentada.
- Riesgo residual explicitado para decision final.
- Acciones de seguimiento claras.

## Anti-patrones a bloquear

- Aprobar sin revisar impacto real del diff.
- Ignorar issues de seguridad por alcance limitado.
- Concluir sin revisar pruebas y checks.
- Marcar como menor un riesgo de produccion.
