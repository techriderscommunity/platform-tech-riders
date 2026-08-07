# Accessibility Instructions

## Objetivo

Garantizar cumplimiento WCAG AA en interacciones reales: teclado, foco visible, semantica correcta y soporte de lectores de pantalla.

## Cuando aplicar

- Desarrollo de componentes y pantallas nuevas.
- Auditorias previas a release.
- Correccion de issues de foco, ARIA, labels o contraste.

## Reglas operativas

- Prioriza HTML semantico antes de ARIA.
- Garantiza navegacion completa por teclado.
- Asegura nombres accesibles, labels y errores anunciables.
- Verifica contraste y estados interactivos.
- Revisa dialogs, menus y componentes custom con foco controlado.

## Checklist de calidad

- Tab order logico y sin focus traps accidentales.
- Focus visible en todos los elementos interactivos.
- Formularios con validacion accesible y mensajes claros.
- Roles/atributos ARIA solo cuando son necesarios.
- Casos criticos validados manualmente con screen reader.

## Criterios de salida

- Hallazgos priorizados por severidad y criterio WCAG.
- Plan de remediacion accionable.
- Pruebas automatizadas y manuales definidas.
- Riesgo residual documentado si queda deuda.

## Anti-patrones a bloquear

- Divs clicables simulando botones.
- Placeholder como unico label.
- ARIA redundante o decorativa.
- Quitar focus outline sin reemplazo accesible.
