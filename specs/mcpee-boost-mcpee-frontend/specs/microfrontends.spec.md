# microfrontends.spec.md

## Proposito

Definir cuando usar microfrontends y como operarlos con bajo acoplamiento runtime, contratos estables y despliegue independiente.

## Ambito

- Decision microfrontend vs modular monolith.
- Contratos host-remote y estrategia de integracion.
- Versionado, shared dependencies, resiliencia y observabilidad.

## Decisiones estandar

1. Adoptar microfrontends solo con justificacion de autonomia real.
2. Definir bounded contexts por dominio y ownership.
3. Contratos de integracion versionables y trazables.
4. Shared dependencies minimizadas y gobernadas.
5. Fallback definido ante caida de remotes.

## Reglas obligatorias

- Build y deploy independientes por dominio.
- No imports internos cruzados entre remotes.
- Routing y errores de integracion documentados.
- Telemetria unificada para trazabilidad cross-app.
- Cambios de contrato con plan de compatibilidad.

## Antipatrones a bloquear

- Shared package unico para todo el negocio.
- Dependencia de deploy coordinado para cambios menores.
- Host sin estrategia de degradacion.
- Integraciones runtime sin contrato formal.

## Checklist de validacion

- [ ] La decision esta documentada.
- [ ] Hay owner claro.
- [ ] Criterio de adopcion microfrontend justificado.
- [ ] Contratos host-remote definidos.
- [ ] Impacto en DX evaluado.
- [ ] Impacto en seguridad evaluado.
- [ ] Impacto en performance evaluado.
- [ ] Tests contractuales/e2e asociados.

## Evidencias esperadas

- Decision argumentada con trade-offs.
- Contratos de integracion documentados.
- Plan de versionado y compatibilidad.
- Plan de resiliencia y observabilidad.
