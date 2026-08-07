# Architecture Review Prompt

## Purpose

Guide runtime composition for boundary integrity, coupling analysis, and migration-safe architecture recommendations.

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

- Layer and bounded-context dependency direction.
- Operational architecture fitness: resilience, observability, deployability.
- Incremental migration strategy with risk-controlled sequencing.

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
- Do not propose large rewrites without a staged migration path.
- Do not infer architecture from folder names alone; verify with call/dependency evidence.
```

## Output Format

- Executive Summary
- Prioritized Findings (Critical, High, Medium, Low)
- Impacted Areas
- Evidence Used
- Recommended Actions
- Validation Plan
- Open Risks / Unknowns
