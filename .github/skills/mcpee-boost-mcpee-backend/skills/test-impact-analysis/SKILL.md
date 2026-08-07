# Test Impact Analysis Skill

## Goal

Provide an evidence-based test impact analysis workflow that turns repository signals into prioritized, actionable guidance.

## When To Use

- Use this skill when a request requires test impact analysis guidance with concrete evidence from the repository.
- Use this skill when trade-offs must be prioritized by risk, impact, and delivery constraints.

## Required Inputs

- User request.
- Relevant specs.
- Project knowledge if available.
- Project memory if available.
- Provider context if available.

## Procedure

1. Identify the scope of the test impact analysis request and the affected components.
2. Gather direct evidence from code, configuration, and project context before drawing conclusions.
3. Map findings to relevant specs and classify each issue by severity and business impact.
4. Propose actionable recommendations with a safe execution order and validation approach.
5. Summarize confirmed facts, open risks, and next implementation steps.

## Provider Needs

- dependency-analysis
- test-discovery
- blast-radius

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

- Recommendations are generic, not backed by code or configuration evidence.
- Critical risks are missed because assumptions are not separated from confirmed findings.

## SkillOpt Notes

This skill can be optimized by SkillOpt. Do not place universal knowledge here if it belongs in specs/.
