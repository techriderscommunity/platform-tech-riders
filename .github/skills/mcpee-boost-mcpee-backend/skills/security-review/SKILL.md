# Security Review Skill

## Goal

Surface exploitable backend security weaknesses and prescribe prioritized, evidence-backed remediations.

## When To Use

- When auditing authentication, authorization, data protection, and secret handling controls.
- When preparing external launch, compliance evidence, or post-incident hardening.

## Required Inputs

- User request.
- Relevant specs.
- Project knowledge if available.
- Project memory if available.
- Provider context if available.

## Procedure

1. Enumerate trust boundaries, identities, permissions, and externally reachable surfaces.
2. Validate authN/authZ paths, tenant isolation, and object-level access controls.
3. Inspect input handling, output encoding, and sensitive data exposure in errors/logs.
4. Review dependency and configuration risks: secrets, TLS, token lifetime, least privilege.
5. Produce severity-ranked findings with exploit scenario, impact, and fix strategy.

## Provider Needs

- code-navigation
- dependency-analysis
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

- Reporting checklist items without exploitability or business impact context.
- Ignoring authorization edge cases in async paths and background jobs.

## SkillOpt Notes

This skill can be optimized by SkillOpt. Do not place universal knowledge here if it belongs in specs/.
