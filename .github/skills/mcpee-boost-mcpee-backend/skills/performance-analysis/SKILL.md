# Performance Analysis Skill

## Goal

Identify and prioritize backend bottlenecks affecting latency, throughput, and cost efficiency.

## When To Use

- When SLOs are violated under load, burst traffic, or long-running workflows.
- When optimization work must be sequenced by measurable performance impact.

## Required Inputs

- User request.
- Relevant specs.
- Project knowledge if available.
- Project memory if available.
- Provider context if available.

## Procedure

1. Collect baseline latency/throughput/error metrics and hotspot traces.
2. Localize bottlenecks across compute, I/O, serialization, and data access layers.
3. Quantify impact and expected gain for each candidate optimization.
4. Evaluate side effects on correctness, resilience, and operational complexity.
5. Deliver a phased plan: quick wins, medium-term fixes, and architecture-level changes.

## Provider Needs

- telemetry
- dependency-analysis
- code-navigation
- knowledge-graph

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

- Suggesting optimizations without baseline and post-change measurement criteria.
- Over-optimizing low-impact paths while critical bottlenecks remain unaddressed.

## SkillOpt Notes

This skill can be optimized by SkillOpt. Do not place universal knowledge here if it belongs in specs/.
