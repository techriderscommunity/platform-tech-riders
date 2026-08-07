# Design System Baseline Spec

## Capability

Create or normalize foundational design tokens and component standards for consistent UX/UI delivery.

## Inputs

- Brand domain context.
- Existing token files if present.
- Optional Figma source.

## Outputs

- Token files: palette, typography, spacing/radius/shadows, patterns.
- Usage guidance for component and page-level implementation.
- Risk notes when tokens are missing or contradictory.

## Acceptance Criteria

- All token files exist in `docs/design-tokens/`.
- Token naming is semantic and non-duplicated.
- Includes light/dark compatibility guidance.
- Includes minimum accessibility guardrails.

## Dependencies

- `skills/design-system-generator/SKILL.md`
- `skills/figma-integration/SKILL.md`
- `docs/design-tokens/*.json`
