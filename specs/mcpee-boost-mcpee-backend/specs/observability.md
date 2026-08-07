# Observability Spec

## Purpose

Guarantee backend systems emit enough high-quality telemetry to explain behavior under normal and failure conditions.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Instrument around user journeys and business-critical flows, not only infrastructure health.
- Maintain end-to-end correlation context across synchronous and asynchronous boundaries.
- Treat telemetry schema consistency as a contract across services.

## Validation Rules

- Critical flows have traces, logs, and metrics with shared correlation identifiers.
- SLIs and alerts map to actionable ownership and remediation paths.
- Logs are structured, queryable, and redacted for sensitive data.

## Anti-Patterns

- High telemetry volume with low diagnostic value.
- Alerts disconnected from SLOs and on-call ownership.
- Trace context dropped at queue/event boundaries.

## Evidence Required

Use dashboards, trace samples, log schemas, and incident timelines to verify diagnosability.

## Related Specs

- specs/logging.md
- specs/tracing.md
- specs/telemetry.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
