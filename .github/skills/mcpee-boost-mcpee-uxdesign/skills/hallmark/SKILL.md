---
name: 'hallmark'
description: 'Bridge skill to use Hallmark as an optional visual exploration complement while preserving BoostDesign UX governance and accessibility standards.'
---

# Hallmark Bridge Skill

Use this skill when you want stronger visual differentiation in hero/landing explorations while keeping BoostDesign quality gates.

## Purpose
- Combine Hallmark visual generation/redesign capability with BoostDesign governance.
- Prevent visual drift by normalizing outputs into design tokens and accessibility checks.
- Keep Hallmark optional and scoped to exploratory visual work.

## External Dependency
- Install Hallmark engine:
  - `npx skills add nutlope/hallmark`
- Project source:
  - https://github.com/Nutlope/hallmark

## Bridge Workflow
1. Visual Exploration
- Use Hallmark verbs (`default`, `audit`, `redesign`, `study`) to generate direction.

2. Token Normalization
- Pass visual output to `@design-system-architect`.
- Extract semantic colors, typography pairings, spacing rhythm, and component rules.

3. UX Governance
- Validate with `@ux-ui-expert` and `ux-audit`.
- Ensure WCAG AA targets, component state completeness, and consistency constraints.

4. Production Handoff
- Persist outputs in `docs/design-tokens/`.
- Document decisions in templates/spec artifacts.

## Output Contract
- Visual intent summary.
- Token mapping proposal.
- Accessibility and consistency risk list.
- Remediation actions for non-compliant visual choices.

## Guardrails
- Hallmark output cannot bypass BoostDesign accessibility checks.
- Hallmark output cannot replace token-driven implementation rules.
- If conflict exists, BoostDesign governance has precedence.

## Related Artifacts
- `references/integration-playbook.md`
- `specs/hallmark-bridge-spec.md`
- `templates/hallmark-handoff.md`
- `examples/hallmark-to-ux-flow.md`
