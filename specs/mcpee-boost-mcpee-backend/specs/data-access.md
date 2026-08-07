# Data Access Spec

## Purpose

Ensure data access paths are correct, performant, and resilient under production load.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Align query shape with access patterns and bounded context invariants.
- Keep transaction scope minimal and explicit, especially across external side effects.
- Treat persistence performance as a first-class design concern, not an afterthought.

## Validation Rules

- High-cardinality reads enforce safe paging, filtering, and ordering contracts.
- Hot queries are index-aligned and avoid N+1 or repeated hydration patterns.
- Write workflows preserve consistency with clear retry and idempotency behavior.

## Anti-Patterns

- Unbounded list endpoints on large datasets.
- Long-lived transactions including external network I/O.
- Entity mapping leaks causing cross-context schema coupling.

## Evidence Required

Use query plans, ORM traces, lock metrics, and endpoint latency correlation to justify recommendations.

## Related Specs

- specs/database-first.md
- specs/performance.md
- specs/transactions.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
