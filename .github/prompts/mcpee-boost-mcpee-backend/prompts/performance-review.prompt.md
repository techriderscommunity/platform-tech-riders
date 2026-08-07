# Performance Review Prompt

## Purpose

Guide runtime composition for bottleneck localization and impact-prioritized optimization planning.

## Runtime Inputs

- Agent identity.
- Official specs.
- Official skills.
- Generated skill if exists.
- Local override if exists.
- Project knowledge.
- Project memory.
- Provider context.
- Telemetry constraints.

## Domain Focus

- Latency/throughput bottlenecks tied to real telemetry signals.
- Hot-path compute, I/O, serialization, and caching inefficiencies.
- Optimization sequencing with risk and verification criteria.

## Prompt Structure

```text
You are executing capability: {capability.id}

Agent:
{agent}

Official Specs:
{specs}

Composed Skill:
{skill}

Project Knowledge:
{knowledge}

Project Memory:
{memory}

Provider Context:
{providerContext}

Execution rules:
- Use evidence first.
- Distinguish confirmed findings from assumptions.
- Prioritize by business impact and operational risk.
- Propose actionable remediation with safe sequencing.
- Cite file paths and reasoning inputs when available.

Domain-specific guardrails:
- Do not recommend optimizations without baseline and target metrics.
- Do not prioritize micro-optimizations over critical-path bottlenecks.
```

## Output Format

- Executive Summary
- Prioritized Findings (Critical, High, Medium, Low)
- Impacted Areas
- Evidence Used
- Recommended Actions
- Validation Plan
- Open Risks / Unknowns
