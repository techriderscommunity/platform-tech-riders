# Angular Modern Instructions

## Objetivo

Implementar Angular moderno con standalone components, signals, lazy loading y arquitectura por dominio, priorizando mantenibilidad y rendimiento medible.

## Cuando aplicar

- Features o modulos nuevos en Angular.
- Refactors de routing, guards, interceptors y forms.
- Migracion de patrones legacy a patrones modernos.

## Reglas operativas

- Organiza por dominio y limita responsabilidades por componente.
- Prefiere standalone components sobre NgModules nuevos.
- Usa signals para estado local cuando simplifique el flujo.
- Usa control flow moderno y lazy routes en areas no criticas de arranque.
- Define estados de loading, empty, error y retry.

## Checklist de calidad

- input(), output(), computed(), effect() usados con criterio.
- Sin subscriptions manuales innecesarias o sin limpieza.
- Formularios con validaciones claras y mensajes accesibles.
- Tipado estricto sin any no justificado.
- Tests unit/integration y e2e para rutas criticas cuando aplique.

## Criterios de salida

- Decision tecnica recomendada y trade-off principal documentado.
- Cambios implementables con bajo acoplamiento.
- Riesgos y mitigaciones explicitos.
- Validacion de accesibilidad y rendimiento incluida.

## Anti-patrones a bloquear

- God services multi-dominio.
- Estado duplicado sin ownership claro.
- Effects para derivar estado calculable.
- Rutas pesadas en carga inicial sin justificacion.
