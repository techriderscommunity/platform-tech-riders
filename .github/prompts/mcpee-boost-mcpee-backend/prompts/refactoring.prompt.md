# Refactoring Review Prompt

## Purpose

Guide runtime composition for behavior-safe refactoring plans with clear sequencing.

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

- Complexity and coupling hotspots that block safe change.
- Incremental refactor strategy with non-regression checkpoints.
- Test adaptation needed to protect behavior during structural changes.

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
- Do not suggest high-risk refactors without rollback or containment strategy.
- Do not optimize structure at the cost of domain clarity.
```

## Output Format

- Executive Summary
- Prioritized Findings (Critical, High, Medium, Low)
- Impacted Areas
- Evidence Used
- Recommended Actions
- Validation Plan
- Open Risks / Unknowns
