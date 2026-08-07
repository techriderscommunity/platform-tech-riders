# react-production-review

## Description

Revisa o implementa React moderno con hooks, estado, efectos, rendering, accesibilidad, performance y tests.

## When to use

Usa esta skill para diseno de features React, refactor de componentes/hooks, estrategia de estado y optimizacion de render.

## Instructions

1. Lee el contexto existente antes de proponer cambios.
2. Modela el dominio por composicion, no por jerarquia tecnica.
3. Separa explicitamente server state, UI state y URL state.
4. Limita useEffect a efectos reales; evita derivar estado por efectos.
5. Define loading/empty/error y comportamiento de reintentos.
6. Evalua impacto de rendering y bundle antes de cerrar.

## Output esperado

- Decision recomendada y trade-off principal.
- Arbol de componentes y ownership funcional.
- Estrategia de estado y data fetching.
- Plan de implementacion incremental.
- Plan de test y riesgo residual.

## Checklist

- [ ] Tipos correctos.
- [ ] Hooks acotados y sin responsabilidades mezcladas.
- [ ] Sin context global innecesario.
- [ ] Estados loading/error/empty cubiertos.
- [ ] A11y validada en componentes interactivos.
- [ ] Coste de render y bundle revisado.
- [ ] Tests definidos por riesgo.
