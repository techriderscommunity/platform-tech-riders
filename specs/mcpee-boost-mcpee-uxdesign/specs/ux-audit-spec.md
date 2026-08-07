# UX Audit Capability Spec

## Capability

End-to-end UX/UI audit with accessibility, consistency, responsiveness, and actionable remediation priorities.

## Inputs

- Repository source code or UI snapshots.
- Optional Figma references.
- Optional user journeys and business goals.

## Outputs

- Executive summary.
- Prioritized issues by severity.
- Accessibility findings mapped to WCAG.
- Remediation plan by phase.

## Acceptance Criteria

- Includes critical/major/minor severity split.
- Includes at least one measurable success metric per issue group.
- Includes accessibility section with keyboard and ARIA checks.
- Includes state coverage: loading, empty, error, success.

## Dependencies

- `skills/ux-audit/SKILL.md`
- `skills/screenshot-reporter/SKILL.md`
- `skills/aria-accessibility-patterns/SKILL.md`
