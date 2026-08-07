# Microfrontend Instructions

## Objetivo

Disenar microfrontends con independencia de despliegue real, contratos estables y acoplamiento runtime minimo.

## Cuando aplicar

- Decidir entre microfrontend y modular monolith.
- Definir host/remotes y estrategia de federacion.
- Evolucionar una plataforma multi-equipo.

## Reglas operativas

- Justifica complejidad frente a beneficios de autonomia.
- Define bounded contexts por dominio y ownership.
- Mantiene contratos host-remote versionados.
- Minimiza shared dependencies y controla versiones.
- Diseña fallback y degradacion si un remote falla.

## Checklist de calidad

- Build y deploy independientes por dominio.
- Contratos de integracion documentados.
- Trazabilidad y observabilidad cross-app activas.
- Sin leaks de estilos ni dependencias internas ocultas.
- Plan de pruebas contractuales y e2e cross-app.

## Criterios de salida

- Recomendacion argumentada de arquitectura.
- Riesgos de acoplamiento identificados y mitigados.
- Politica de versionado y compatibilidad definida.
- Estrategia de rollback y resiliencia disponible.

## Anti-patrones a bloquear

- Shared package monolitico para todo.
- Imports internos entre remotes.
- Deploy coordinado obligatorio para cambios pequenos.
- Host sin manejo de fallo de remotes.
