# Testing Review Skill

## Goal

Assess backend test strategy quality and improve confidence, stability, and release safety.

## When To Use

- When flaky tests, escaped defects, or low-signal coverage reduce delivery confidence.
- When defining risk-based test coverage for critical backend workflows.

## Required Inputs

- User request.
- Relevant specs.
- Project knowledge if available.
- Project memory if available.
- Provider context if available.

## Procedure

1. Map critical flows and failure modes to existing unit, integration, and contract tests.
2. Evaluate signal quality: determinism, isolation, and relevance to production behavior.
3. Detect coverage gaps in error paths, compatibility contracts, and concurrency behavior.
4. Correlate test-suite performance with pipeline feedback loops and release cadence.
5. Recommend prioritized test improvements with ownership and measurable outcomes.

## Provider Needs

- test-discovery
- dependency-analysis
- code-navigation
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

- Equating test count with confidence while critical paths remain weakly validated.
- Ignoring flakiness root causes and only rerunning unstable tests.

## SkillOpt Notes

This skill can be optimized by SkillOpt. Do not place universal knowledge here if it belongs in specs/.
