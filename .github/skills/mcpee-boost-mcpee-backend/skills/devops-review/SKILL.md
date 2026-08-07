# DevOps Review Skill

## Goal

Assess CI/CD, release governance, rollback readiness, and operational safety for backend delivery.

## When To Use

- When reviewing pipelines, environment promotion rules, and deployment controls.
- When incidents reveal weak release safeguards or poor recovery procedures.

## Required Inputs

- User request.
- Relevant specs.
- Project knowledge if available.
- Project memory if available.
- Provider context if available.

## Procedure

1. Inspect pipeline stages, approval gates, test strategy, and artifact promotion flow.
2. Validate environment parity, secret handling, and configuration drift controls.
3. Review release safety mechanisms: smoke checks, canaries, rollback automation, runbooks.
4. Correlate pipeline decisions with operational telemetry and incident history.
5. Provide prioritized actions balancing speed, compliance, and recovery objectives.

## Provider Needs

- pipeline-analysis
- document-search
- repository-analysis
- project-memory

## Output Contract

The response must include:

- Summary.
- Findings.
- Impact.
- Evidence.
- Recommended next actions.

## Quality Criteria

- Grounded in evidence.
- Uses official specs.
- Distinguishes facts from assumptions.
- Prioritizes by risk and impact.
- Avoids unnecessary verbosity.

## Failure Modes

- Focusing on tooling preferences instead of release risk and recovery capability.
- Marking a pipeline healthy without validating post-deploy verification and rollback paths.

## SkillOpt Notes

This skill can be optimized by SkillOpt. Do not place universal knowledge here if it belongs in specs/.
