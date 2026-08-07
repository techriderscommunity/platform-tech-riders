# Refactoring Spec

## Purpose

Define safe, incremental backend refactoring standards that preserve behavior while reducing design debt.

This spec represents universal backend boost knowledge. It must not contain customer-specific documentation or project-specific one-off decisions.

## Principles

- Refactor in small, test-protected increments aligned to bounded responsibilities.
- Preserve externally observable behavior unless contract changes are explicitly approved.
- Use refactoring to improve clarity, dependency direction, and changeability.

## Validation Rules

- Critical behavior is protected by deterministic tests before structural changes.
- Each step reduces complexity or coupling with measurable before/after evidence.
- Migration sequencing includes rollback or containment strategy for risky moves.

## Anti-Patterns

- Large-scale rewrites with no staged verification gates.
- Introducing abstraction layers that hide business intent.
- Refactoring style without addressing maintainability bottlenecks.

## Evidence Required

Use complexity metrics, dependency graphs, and regression results to prove refactor value.

## Related Specs

- specs/code-quality.md
- specs/testing.md
- specs/clean-architecture.md

## Notes for Agents

Use this spec to anchor decisions, explicitly call out assumptions, and prioritize high-impact remediation first.
