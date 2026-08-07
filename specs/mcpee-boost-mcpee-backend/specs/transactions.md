# Transactions Spec

## Purpose

Define safe transactional behavior for backend workflows under concurrency and failure conditions.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Keep transactional boundaries minimal and aligned with business invariants.
- Design for idempotency and replay safety in distributed operations.
- Prefer explicit consistency strategies over accidental coupling.

## Validation Rules

- Write operations define isolation expectations and conflict handling behavior.
- Cross-boundary side effects use reliable coordination (outbox, saga, compensations).
- Retries do not violate business invariants or duplicate effects.

## Anti-Patterns

- Holding DB transactions across external network calls.
- Mixing unrelated aggregate updates in one implicit transaction scope.
- Retrying non-idempotent operations without deduplication safeguards.

## Evidence Required

Use transaction scopes, lock/timeout traces, and workflow state transitions to verify safety.

## Related Specs

- specs/data-access.md
- specs/event-driven.md
- specs/resilience.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
