# Testing Spec

## Purpose

Define backend testing strategy that optimizes confidence, defect prevention, and delivery speed.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Design tests around behavior and risk, not raw test volume.
- Maintain deterministic, isolated tests with clear ownership.
- Cover failure modes and contract boundaries of critical workflows.

## Validation Rules

- Critical business flows include unit, integration, and contract-level protection as needed.
- Flaky tests are tracked, triaged, and prevented through deterministic design rules.
- Test feedback latency supports release cadence and risk posture.

## Anti-Patterns

- Counting assertion volume as confidence without critical-path coverage.
- Fragile tests coupled to private implementation details.
- Ignoring negative-path and concurrency scenarios in backend workflows.

## Evidence Required

Use coverage by risk area, flake trends, and escaped-defect analysis to prioritize improvements.

## Related Specs

- specs/code-quality.md
- specs/clean-architecture.md
- specs/ci-cd.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
