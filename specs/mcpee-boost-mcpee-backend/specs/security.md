# Security Spec

## Purpose

Define mandatory backend security controls for identity, authorization, data protection, and operational hardening.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Apply least privilege and explicit authorization at every resource boundary.
- Protect sensitive data in transit, at rest, and in telemetry outputs.
- Continuously reduce attack surface through secure defaults and verification.

## Validation Rules

- AuthN/AuthZ controls are enforced consistently across sync and async entry points.
- Secrets, tokens, and credentials follow rotation and scope minimization policies.
- Input/output and dependency surfaces are reviewed for exploitability and abuse paths.

## Anti-Patterns

- Authorization checks only at route-level without object-level validation.
- Sensitive data leakage in logs, errors, or debug traces.
- Shared high-privilege credentials across unrelated workloads.

## Evidence Required

Use code paths, IAM config, logs, and dependency manifests to validate effective controls.

## Related Specs

- specs/data-protection.md
- specs/error-handling.md
- specs/devops.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
