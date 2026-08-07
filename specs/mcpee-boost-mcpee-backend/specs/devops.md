# DevOps Spec

## Purpose

Define backend delivery practices that maximize release safety, repeatability, and recovery speed.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Automate build, test, and deployment with explicit control points for risk containment.
- Prefer immutable, traceable artifacts across environments.
- Design rollback and incident response as mandatory release capabilities.

## Validation Rules

- Pipelines include quality gates tied to test signal and policy checks.
- Production promotions require controlled strategy (approval/canary/verification).
- Rollback path is documented, tested, and operationally feasible within target RTO.

## Anti-Patterns

- Direct production deploys with no post-deploy verification stage.
- Environment-specific configuration drift with undocumented overrides.
- Treating failed rollback as an acceptable operational state.

## Evidence Required

Use pipeline configs, deployment logs, incident postmortems, and runbook quality to validate maturity.

## Related Specs

- specs/ci-cd.md
- specs/observability.md
- specs/security.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
