# Security Instructions

## Objetivo

Reducir riesgos de seguridad frontend en autenticacion, manejo de tokens, inyecciones, dependencias y exposicion de datos.

## Cuando aplicar

- Flujos de login/logout/refresh token.
- Integraciones con OAuth2/OIDC/MSAL.
- Pantallas con datos sensibles o privilegios elevados.
- Revisiones de dependencias y supply chain.

## Reglas operativas

- Nunca exponer secretos en cliente, repositorio o logs.
- Evita construir HTML inseguro y valida/sanitiza entrada.
- Define controles contra XSS, CSRF y clickjacking segun arquitectura.
- Aplica principio de minimo privilegio en scopes y claims.
- Revisa dependencias y CVEs para librerias nuevas o actualizadas.

## Checklist de calidad

- Token handling documentado y sin almacenamiento inseguro.
- Rutas protegidas y errores de auth controlados.
- CSP y cabeceras de seguridad alineadas con la app.
- Sin hardcodes de secretos o endpoints privados.
- Tests para escenarios de auth fallida y expiracion de sesion.

## Criterios de salida

- Riesgos de seguridad priorizados y mitigaciones propuestas.
- Decisiones de autenticacion/autorizacion justificadas.
- Impacto en UX y operacion documentado.
- Riesgo residual explicito si hay deuda.

## Anti-patrones a bloquear

- Guardar tokens sensibles sin criterio de riesgo.
- Interpolar contenido no confiable sin sanitizacion.
- Confiar solo en validaciones de cliente.
- Introducir dependencias sin evaluacion de seguridad.
