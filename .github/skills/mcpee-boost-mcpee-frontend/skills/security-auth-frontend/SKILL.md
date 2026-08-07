# security-auth-frontend

## Description

Revisa autenticacion, autorizacion, tokens, XSS, CSP, CSRF y exposicion de secretos en frontend.

## When to use

Usa esta skill para revisar auth frontend, manejo de tokens, riesgo XSS/CSRF/CSP y exposicion de secretos o datos sensibles.

## Instructions

1. Lee el contexto existente antes de proponer cambios.
2. Revisa flujo de autenticacion/autorizacion de extremo a extremo.
3. Valida token handling y almacenamiento.
4. Evalua superficie de ataque XSS/CSRF/clickjacking.
5. Verifica dependencias y configuracion de seguridad.
6. Propone mitigaciones con impacto en UX explicitado.

## Output esperado

- Hallazgos de seguridad por severidad.
- Recomendaciones de mitigacion priorizadas.
- Impacto funcional/UX de mitigaciones.
- Plan de validacion de seguridad.
- Riesgo residual documentado.

## Checklist

- [ ] Tipos correctos.
- [ ] Sin secretos expuestos en frontend.
- [ ] Token handling revisado.
- [ ] Riesgo XSS/CSRF evaluado.
- [ ] CSP y cabeceras documentadas.
- [ ] Pruebas de auth fallida definidas.
