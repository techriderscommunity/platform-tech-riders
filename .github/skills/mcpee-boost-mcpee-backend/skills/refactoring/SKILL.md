# Refactoring Skill

## Goal

Plan and execute behavior-safe backend refactors that reduce complexity and improve maintainability.

## When To Use

- When hotspots show excessive coupling, high cyclomatic complexity, or fragile tests.
- When preparing code for feature expansion, reliability hardening, or architecture shifts.

## Required Inputs

- User request.
- Relevant specs.
- Project knowledge if available.
- Project memory if available.
- Provider context if available.

## Procedure

1. Define non-regression boundaries and critical flows to preserve.
2. Segment changes into safe increments with explicit verification checkpoints.
3. Improve structure by separating responsibilities and dependency direction.
4. Upgrade tests from implementation-coupled to behavior-oriented coverage.
5. Track residual debt and hand off a continuation plan with risk notes.

## Provider Needs

- code-navigation
- dependency-analysis
- blast-radius
- test-discovery

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

- Applying large refactors without guardrails, causing hidden behavior regressions.
- Reducing readability in the name of abstraction or pattern purity.

## SkillOpt Notes

This skill can be optimized by SkillOpt. Do not place universal knowledge here if it belongs in specs/.
