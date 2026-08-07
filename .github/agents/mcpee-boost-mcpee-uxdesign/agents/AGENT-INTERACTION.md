# Agent Interaction Model

## Primary Orchestration

1. `@ux-ui-expert` is the default entrypoint for full audits and strategy.
2. `@ux-ui-expert` delegates to specialized agents based on objective.

## Delegation Paths

### Path A: Audit and Remediation

- Entry: `@ux-ui-expert`
- Delegates:
  - `@component-inventory-mapper` for component mapping and duplicate detection.
  - `@design-system-architect` for token normalization and design consistency.
- Exit: prioritized remediation plan with measurable goals.

### Path B: Design Source Alignment

- Entry: `@ux-ui-expert` or `@design-system-architect`
- Delegate: `@figma-design-auditor` for design extraction and drift detection.
- Exit: synchronized design recommendations and token updates.

### Path C: Capability-First Execution

- Entry: specialized agent directly.
- Exit: focused output for that capability, then optional consolidation by `@ux-ui-expert`.

### Path D: Optional Hallmark Visual Exploration

- Entry: `@ux-ui-expert` with Hallmark bridge prompts.
- Optional external engine: Hallmark (`npx skills add nutlope/hallmark`).
- Flow:
  - Generate or redesign visual direction with Hallmark verbs.
  - Normalize visual output into design tokens via `@design-system-architect`.
  - Validate accessibility and consistency via `@ux-ui-expert` + `ux-audit`.
- Exit: visually distinctive proposal that still passes BoostDesign quality gates.

## Interaction Contracts

- Every handoff must include: objective, scope, constraints, and expected output format.
- Every output must include: findings, rationale, and next actions.
- Accessibility considerations are mandatory in all final UX recommendations.
