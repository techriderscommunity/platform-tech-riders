# microfrontend-decision

## Description

Evalua y define estrategia de microfrontends con contratos host-remote, versionado y bajo acoplamiento runtime.

## When to use

Usa esta skill para decidir microfrontend vs modular monolith, definir federation y establecer reglas de integracion entre equipos.

## Instructions

1. Analiza autonomia de equipos y cadencia de release.
2. Valora complejidad operativa frente a beneficio real.
3. Define contratos host-remote (routing, eventos, auth, errores).
4. Establece politica de shared dependencies y versionado.
5. Diseña fallbacks ante caida de remotes.
6. Define pruebas contractuales y e2e cross-app.

## Output esperado

- Decision justificada: microfrontend o alternativa.
- Contratos de integracion definidos.
- Riesgos de acoplamiento y mitigaciones.
- Plan de adopcion por fases.
- Validacion operativa y de release.

## Checklist

- [ ] Independencia de build/deploy por dominio.
- [ ] Contratos versionables y trazables.
- [ ] Shared deps minimizadas.
- [ ] Fallback runtime definido.
- [ ] Testing cross-app definido.
