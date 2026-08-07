# Microservices Spec

## Purpose

Define the non-negotiable guidance for microservices decisions in backend systems.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Keep microservices decisions explicit, traceable, and aligned with system boundaries.
- Prefer stable contracts and reversible changes over short-term convenience.
- Optimize for reliability, observability, and long-term maintainability.

## Validation Rules

- Every recommendation must be justified with direct evidence from code, configuration, or runtime behavior.
- Changes must include a verification strategy (tests, metrics, or rollout checks) before production adoption.
- Risk-prone modifications must define rollback or containment guidance.

## Anti-Patterns

- Applying microservices advice without checking constraints of the current architecture and workload.
- Mixing implementation details with policy-level decisions in the same recommendation.
- Treating style preferences as critical issues when no measurable impact exists.

## Evidence Required

Collect code-level and operational evidence that shows current behavior, affected flows, and impact scope before proposing changes.

## Related Specs

- specs/cloud-native.md
- specs/observability.md
- specs/resilience.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
