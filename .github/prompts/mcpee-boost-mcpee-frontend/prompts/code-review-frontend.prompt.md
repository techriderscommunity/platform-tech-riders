---
description: 'Haz una code review frontend severa. Clasifica hallazgos como Critical, High, Medium o Low e incluye'
agent: 'FrontendCodeReviewAgent'
model: 'gpt-5'
---
Haz una code review frontend severa. Clasifica hallazgos como Critical, High, Medium o Low e incluye propuesta concreta de correccion.

Salida obligatoria:

- Hallazgos por severidad con evidencia tecnica concreta.
- Riesgos de seguridad, accesibilidad y rendimiento.
- Gaps de test y escenarios de regresion faltantes.
- Recomendacion de merge o no-merge con riesgo residual.
- Lista corta de acciones bloqueantes y no bloqueantes.
