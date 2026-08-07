# Architecture Review Skill

## Goal

Evaluate backend architecture boundaries, dependency direction, and scalability trade-offs with evidence.

## When To Use

- When reviewing modular monolith or microservice boundaries and coupling risks.
- When planning structural changes that can impact delivery speed or reliability.

## Required Inputs

- User request.
- Relevant specs.
- Project knowledge if available.
- Project memory if available.
- Provider context if available.

## Procedure

1. Map modules, bounded contexts, and dependency directions from code and runtime flow.
2. Identify boundary leaks (domain -> infrastructure, controller -> persistence shortcuts).
3. Assess cross-cutting concerns: transactions, observability, resiliency, and deployability.
4. Classify findings by blast radius, migration complexity, and operational risk.
5. Recommend staged remediation with clear sequencing and verification checkpoints.

## Provider Needs

- dependency-analysis
- call-path-analysis
- knowledge-graph
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

- Proposing a target architecture without a migration path from current constraints.
- Ignoring runtime characteristics while focusing only on static folder structure.

## SkillOpt Notes

This skill can be optimized by SkillOpt. Do not place universal knowledge here if it belongs in specs/.
