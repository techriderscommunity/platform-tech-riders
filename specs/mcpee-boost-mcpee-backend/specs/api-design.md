# API Design Spec

## Purpose

Define stable, consumer-safe API contracts that are explicit, evolvable, and operationally clear.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Model APIs around business resources and use consistent semantics for read/write behavior.
- Design contracts for forward/backward compatibility and explicit deprecation strategy.
- Standardize error and pagination contracts to reduce client integration ambiguity.

## Validation Rules

- Every endpoint has clear ownership, auth requirements, and idempotency expectations.
- Breaking changes require version strategy, migration path, and consumer impact assessment.
- Error payloads and status codes are consistent across bounded contexts.

## Anti-Patterns

- Inconsistent contract shapes across endpoints that force client-specific workarounds.
- Silent behavior changes under the same versioned contract.
- Leaking internal persistence models directly as public API contracts.

## Evidence Required

Use route definitions, DTO schemas, API docs, and integration test coverage to validate contract quality.

## Related Specs

- specs/versioning.md
- specs/error-handling.md
- specs/security.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
