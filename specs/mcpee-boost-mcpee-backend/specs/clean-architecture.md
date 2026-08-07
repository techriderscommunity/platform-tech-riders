# Clean Architecture Spec

## Purpose

Preserve directional boundaries so backend business rules remain independent from delivery and infrastructure concerns.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Domain rules must not depend on framework or infrastructure implementation details.
- Application use-cases orchestrate workflows through interfaces, not concrete adapters.
- Infrastructure concerns remain replaceable and isolated behind ports/adapters.

## Validation Rules

- Dependency direction always points inward toward domain/application layers.
- Controllers and handlers do not access persistence details directly.
- Cross-layer communication contracts are explicit and testable.

## Anti-Patterns

- Domain services importing infrastructure entities or repositories directly.
- Transport/controller logic containing business decision rules.
- Adapters driving use-case orchestration instead of application services.

## Evidence Required

Use import/dependency graphs, call paths, and test boundaries to verify architectural direction.

## Related Specs

- specs/ddd.md
- specs/dependency-injection.md
- specs/modular-monolith.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
