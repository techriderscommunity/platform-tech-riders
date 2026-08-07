# React Modern Instructions

## Objetivo

Construir React de produccion con composicion clara, limites entre server/client state y rendimiento basado en evidencia.

## Cuando aplicar

- Features nuevas en React.
- Refactor de componentes, hooks y formularios.
- Optimizacion de render, bundle y UX interactiva.

## Reglas operativas

- Prefiere componentes funcionales y hooks pequenos con responsabilidad unica.
- Separa server state, UI state y URL state de forma explicita.
- Usa Suspense/lazy solo cuando haya beneficio real de carga.
- Define loading, empty y error en flujos criticos.
- Evita useEffect para derivar estado calculable.

## Checklist de calidad

- Sin prop drilling profundo no justificado.
- Sin context global para todo el estado.
- Contratos de tipos estrictos entre capas.
- Formularios con validaciones y errores accesibles.
- Plan de tests proporcional al riesgo del cambio.

## Criterios de salida

- Estrategia de estado documentada para la feature.
- Riesgos de render y mitigaciones explicitas.
- Impacto en performance y bundle evaluado.
- Decision y alternativa descartada documentadas.

## Anti-patrones a bloquear

- Hooks con multiples responsabilidades.
- Memoizacion prematura sin metrica.
- Side effects ocultos en utilidades puras.
- Regresiones de UX por optimizacion agresiva.
