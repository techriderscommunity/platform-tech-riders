# API Review Skill

## Goal

Audit backend APIs for contract quality, correctness, compatibility, and operational safety before release.

## When To Use

- When reviewing REST/gRPC endpoint design, error contracts, and version compatibility.
- When a team needs prioritized API findings with consumer impact and concrete remediation steps.

## Required Inputs

- User request.
- Relevant specs.
- Project knowledge if available.
- Project memory if available.
- Provider context if available.

## Procedure

1. Inventory public endpoints, versions, auth model, and request/response contracts.
2. Validate resource modeling, HTTP semantics, idempotency, and pagination consistency.
3. Inspect error payload standards, status code usage, and backward compatibility risks.
4. Cross-check security controls (authz per resource, input validation, data exposure).
5. Produce risk-ranked recommendations with migration guidance and test requirements.

## Provider Needs

- code-navigation
- symbol-analysis
- document-search
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

- Treating API style issues as critical while missing compatibility or data integrity regressions.
- Making breaking-change claims without tracing actual consumers and version policy.

## SkillOpt Notes

This skill can be optimized by SkillOpt. Do not place universal knowledge here if it belongs in specs/.
