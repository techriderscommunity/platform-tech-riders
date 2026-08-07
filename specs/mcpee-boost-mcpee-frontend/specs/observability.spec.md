# observability.spec.md

## Proposito

Definir estandar de observabilidad frontend para detectar, diagnosticar y prevenir incidentes de UX y rendimiento en produccion.

## Ambito

- Telemetria de errores, eventos de negocio y performance.
- Core Web Vitals y trazabilidad de request/session.
- Dashboards, alertas y runbooks de diagnostico.

## Decisiones estandar

1. Error tracking con contexto de ruta/version.
2. Eventos de negocio con contrato y nomenclatura estable.
3. Core Web Vitals medidos en campo.
4. Correlation id entre frontend y backend cuando aplique.
5. Dashboards y alertas para rutas/journeys criticos.

## Reglas obligatorias

- No registrar datos sensibles en telemetria.
- Todos los eventos deben tener ownership y documentacion.
- Definir umbrales de alerta con accion de respuesta.
- Monitorizar degradaciones de rendimiento por release.
- Revisar calidad de telemetria tras cambios mayores.

## Antipatrones a bloquear

- Logging excesivo sin utilidad operativa.
- Eventos sin contrato o naming inconsistente.
- Alertas sin runbook de accion.
- Ausencia de metricas de experiencia real.

## Checklist de validacion

- [ ] La decision esta documentada.
- [ ] Hay owner claro.
- [ ] Mapa de eventos y errores definido.
- [ ] Umbrales y alertas definidos.
- [ ] Impacto en seguridad evaluado.
- [ ] Impacto en performance evaluado.
- [ ] Correlacion frontend-backend definida.
- [ ] Validacion de telemetria asociada.

## Evidencias esperadas

- Catalogo de eventos y errores.
- Dashboard minimo por dominio/ruta critica.
- Regla de alertado con responsable.
- Resultado de validacion post-release.
