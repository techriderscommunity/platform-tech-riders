# frontend-observability-telemetry

## Description

Define y audita observabilidad frontend de produccion: errores, eventos de negocio, Core Web Vitals, dashboards y alertas accionables.

## When to use

Usa esta skill para instrumentar telemetria, detectar regresiones por release, y reducir MTTR en incidentes de frontend.

## Instructions

1. Define objetivos operativos y rutas/journeys criticos.
2. Crea catalogo de eventos de negocio con contrato estable.
3. Instrumenta errores con contexto (ruta, version, entorno, correlation id).
4. Mide Core Web Vitals en campo y separa por dispositivo/segmento.
5. Define dashboards minimos por dominio y alertas con umbrales.
6. Asegura runbooks para alertas criticas y ownership explicito.
7. Verifica privacidad: no enviar PII ni secretos en telemetria.

## Output esperado

- Mapa de eventos/errores con naming y ownership.
- Baseline de CWV y objetivos por ruta critica.
- Propuesta de dashboards y reglas de alertado.
- Riesgos de observabilidad y mitigaciones.
- Plan de validacion post-release.

## Checklist

- [ ] Eventos de negocio con contrato y owner.
- [ ] Error tracking con contexto minimo obligatorio.
- [ ] CWV medidos en campo y con segmentacion.
- [ ] Alertas con umbrales y runbook.
- [ ] Cumplimiento de privacidad y seguridad.
- [ ] Validacion post-release definida.
