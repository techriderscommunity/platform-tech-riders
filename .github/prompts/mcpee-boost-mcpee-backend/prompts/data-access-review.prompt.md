# Data Access Review Prompt

## Purpose

Guide runtime composition for data correctness, query efficiency, and transaction safety reviews.

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

- Query and access-pattern performance risks (N+1, full scans, missing limits).
- Transaction boundaries, idempotency, and consistency behavior under failure.
- Schema/index usage aligned to endpoint and workflow patterns.

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
- Do not recommend query rewrites without explaining expected measurable impact.
- Do not ignore transaction and side-effect ordering risks in distributed workflows.
```

## Output Format

- Executive Summary
- Prioritized Findings (Critical, High, Medium, Low)
- Impacted Areas
- Evidence Used
- Recommended Actions
- Validation Plan
- Open Risks / Unknowns
