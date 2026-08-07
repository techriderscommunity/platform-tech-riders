# Evals del boost

Este directorio contiene evaluaciones de humo para evitar regresiones en el paquete.

- `frontend-smoke.json`: matriz de cobertura por dominio (agent + prompt + skill + spec).

## Ejecucion local

- `npm run evals`
- `npm run quality` (incluye JSON + evals)

## Que valida

1. Existencia de artefactos declarados en `mcpee.json` y `plugin.json`.
2. Existencia de cada caso en la suite (`agent`, `prompt`, `skill`, `spec`).
3. Consistencia de frontmatter: prompt `agent` y agent `name`.
4. Que skills/specs usadas en casos pertenecen al core catalog.
5. Cobertura: todos los prompts y agentes del repo deben estar cubiertos por la suite.
6. Contratos minimos de contenido en agentes, prompts, skills y specs (secciones obligatorias).

Si falla, el script sale con codigo distinto de cero para bloquear CI/release.
