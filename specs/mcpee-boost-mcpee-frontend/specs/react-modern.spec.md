# react-modern.spec.md

## Proposito

Definir el estandar React de produccion para crear features mantenibles, predecibles y con rendimiento medible.

## Ambito

- React moderno en SPA y entornos con SSR/hydration cuando aplique.
- Nuevas features, refactor de componentes y estrategia de estado.
- Integracion con testing, accesibilidad y performance.

## Decisiones estandar

1. Composicion sobre herencia en componentes.
2. Separacion explicita entre server state, UI state y URL state.
3. Hooks pequenos con responsabilidad unica.
4. Suspense/lazy solo donde aporte valor real.
5. Manejo explicito de loading, empty, error y retry.

## Reglas obligatorias

- Evitar useEffect para derivar estado computable.
- Evitar contexto global para estado que puede quedar acotado.
- Mantener contratos tipados estrictos entre capas.
- Formularios con errores legibles y accesibles.
- Cambios de arquitectura de estado deben documentar trade-offs.

## Antipatrones a bloquear

- Hooks con multiples responsabilidades.
- Memoizacion prematura sin evidencia.
- Prop drilling profundo sin estrategia de composicion.
- Side effects ocultos en utilidades supuestamente puras.

## Checklist de validacion

- [ ] La decision esta documentada.
- [ ] Hay owner claro.
- [ ] Estrategia de estado por feature definida.
- [ ] Impacto en DX evaluado.
- [ ] Impacto en accesibilidad evaluado.
- [ ] Impacto en performance evaluado.
- [ ] Impacto en seguridad evaluado.
- [ ] Tests asociados por criticidad.

## Evidencias esperadas

- Arbol de componentes con ownership funcional.
- Plan de tests con alcance por riesgo.
- Evidencia de medicion de rendimiento antes/despues.
- Verificacion de accesibilidad en UI interactiva.
