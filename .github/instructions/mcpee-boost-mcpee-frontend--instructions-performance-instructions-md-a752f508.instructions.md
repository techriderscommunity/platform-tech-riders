# Performance Instructions

## Objetivo

Optimizar rendimiento frontend con baseline, hipotesis y medicion antes/despues para mejorar CWV sin degradar UX ni mantenibilidad.

## Cuando aplicar

- Problemas de LCP, INP, CLS o TTI.
- Aumento de bundle o degradacion de interaccion.
- Features que impactan rutas criticas.

## Reglas operativas

- Define baseline por entorno y dispositivo antes de optimizar.
- Prioriza cuellos de botella por impacto y esfuerzo.
- Separa quick wins de cambios estructurales.
- Evalua code splitting, cache y estrategia de carga.
- Verifica impacto en accesibilidad y SEO.

## Checklist de calidad

- LCP, INP y CLS dentro de objetivos del producto.
- Bundle principal dentro de presupuesto acordado.
- Carga diferida en bloques no criticos.
- Imagenes, fuentes y cache configuradas correctamente.
- Alertas de regresion definidas en CI o monitoreo.

## Criterios de salida

- Diagnostico priorizado con evidencia.
- Plan de mejoras con estimacion de impacto.
- Medicion comparativa antes/despues.
- Riesgos de UX por optimizacion y mitigacion.

## Anti-patrones a bloquear

- Optimizar sin baseline.
- Memoizacion ciega sin evidencia.
- Lazy loading en contenido critico inicial.
- Spliting excesivo con waterfalls de red.
