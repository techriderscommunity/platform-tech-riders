# Observability Review Skill

## Goal

Evaluate whether logs, metrics, and traces are sufficient to detect, diagnose, and resolve backend failures quickly.

## When To Use

- When teams struggle to isolate root cause or correlate events across services.
- When defining SLO-driven telemetry standards for backend platforms.

## Required Inputs

- User request.
- Relevant specs.
- Project knowledge if available.
- Project memory if available.
- Provider context if available.

## Procedure

1. Map critical user and system flows to required telemetry signals.
2. Validate correlation context propagation across sync and async boundaries.
3. Assess metric quality (SLI coverage, cardinality control, actionable alerting).
4. Review log structure, redaction policy, and queryability under incident pressure.
5. Recommend improvements that reduce MTTD/MTTR with explicit validation metrics.

## Provider Needs

- telemetry
- call-path-analysis
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

- Treating observability as dashboard quantity instead of diagnosability quality.
- Recommending alerts without threshold rationale or ownership paths.

## SkillOpt Notes

This skill can be optimized by SkillOpt. Do not place universal knowledge here if it belongs in specs/.
