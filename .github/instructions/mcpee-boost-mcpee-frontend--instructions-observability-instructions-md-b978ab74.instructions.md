# Observability Instructions

## Objetivo

Garantizar observabilidad frontend operativa para detectar regresiones, diagnosticar incidentes y mejorar experiencia real de usuario en produccion.

## Cuando aplicar

- Instrumentacion inicial de frontend en produccion.
- Revisiones de readiness antes de release.
- Analisis de incidentes de errores o degradacion de rendimiento.
- Definicion de dashboards y alertado por dominio.

## Reglas operativas

- Instrumenta errores con ruta, version, entorno y correlation id.
- Define eventos de negocio con contrato estable y ownership.
- Mide Core Web Vitals en campo (no solo laboratorio).
- Separa metricas por dispositivo, ruta critica y segmento.
- Crea alertas accionables con runbook y owner responsable.
- Evita envio de PII/secretos en logs o eventos.

## Checklist de calidad

- Catalogo de eventos y errores documentado.
- Baseline y objetivos de CWV definidos.
- Dashboards minimos por journey critico.
- Umbrales de alerta y escalado operativo definidos.
- Validacion post-release incluida en el plan.

## Criterios de salida

- Telemetria util para debugging y decision de producto.
- Alertas con bajo ruido y alta accionabilidad.
- Riesgos de observabilidad identificados y mitigados.
- Integracion con quality gates y revisiones de release.

## Anti-patrones a bloquear

- Eventos sin contrato o naming inconsistente.
- Alertas sin runbook ni owner.
- Logging masivo sin utilidad operativa.
- Falta de segmentacion en metricas reales.
