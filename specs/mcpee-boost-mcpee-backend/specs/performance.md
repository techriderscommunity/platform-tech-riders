# Performance Spec

## Purpose

Define measurable performance engineering practices for backend latency, throughput, and efficiency.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Base optimization on profiling and production telemetry, not assumptions.
- Prioritize changes by user impact and system-wide bottleneck contribution.
- Protect correctness and reliability when introducing performance improvements.

## Validation Rules

- Each optimization includes baseline, target, and verification metrics.
- P95/P99 and saturation metrics are tracked for critical endpoints and jobs.
- Resource and cost effects are evaluated alongside latency gains.

## Anti-Patterns

- Micro-optimizing non-critical code paths while major bottlenecks persist.
- Deploying performance changes without rollback criteria.
- Ignoring serialization and I/O overhead in hot execution paths.

## Evidence Required

Use traces, flamegraphs, benchmark data, and capacity metrics to validate improvements.

## Related Specs

- specs/caching.md
- specs/data-access.md
- specs/observability.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
