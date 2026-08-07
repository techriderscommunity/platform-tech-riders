# Error Handling Spec

## Purpose

Define consistent backend error semantics that improve recoverability, observability, and client behavior.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Expose predictable error contracts with stable machine-readable fields.
- Classify errors by responsibility (client, domain, infrastructure, transient).
- Preserve actionable context while protecting sensitive details.

## Validation Rules

- Error payload structure is consistent across endpoints and services.
- Retryability and user-actionability are explicit in error semantics.
- Internal traces/logs retain diagnostic context tied to error identifiers.

## Anti-Patterns

- Returning generic 500 responses for known business validation failures.
- Leaking stack traces or sensitive internals to external clients.
- Error taxonomies that differ between services without translation rules.

## Evidence Required

Use controller/handler responses, middleware behavior, and logs to validate consistency and safety.

## Related Specs

- specs/api-design.md
- specs/security.md
- specs/observability.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
