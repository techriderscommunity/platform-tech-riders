# Data Access Review Skill

## Goal

Detect correctness and performance issues in repositories, queries, transaction scopes, and data contracts.

## When To Use

- When endpoints or jobs show latency spikes, lock contention, or data consistency defects.
- When introducing new persistence patterns, ORMs, or cross-database operations.

## Required Inputs

- User request.
- Relevant specs.
- Project knowledge if available.
- Project memory if available.
- Provider context if available.

## Procedure

1. Trace data flows from API/service layer to persistence and external dependencies.
2. Inspect query patterns for N+1, full scans, missing limits, and eager/lazy misuse.
3. Validate transaction boundaries, isolation assumptions, and side-effect ordering.
4. Evaluate schema access patterns, indexes, and migration safety constraints.
5. Recommend measurable fixes with expected impact on latency, throughput, and consistency.

## Provider Needs

- code-navigation
- schema-analysis
- telemetry
- dependency-analysis

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

- Optimizing query syntax while leaving transaction and consistency risks unresolved.
- Claiming bottlenecks without evidence from plans, traces, or metrics.

## SkillOpt Notes

This skill can be optimized by SkillOpt. Do not place universal knowledge here if it belongs in specs/.
