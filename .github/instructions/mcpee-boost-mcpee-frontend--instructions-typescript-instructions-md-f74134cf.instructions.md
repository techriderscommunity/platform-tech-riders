# TypeScript Strict Instructions

## Objetivo

Mantener TypeScript estricto y expresivo para reducir errores en runtime y preservar contratos estables entre capas.

## Cuando aplicar

- Cualquier cambio de codigo TypeScript.
- Diseno de APIs internas y contratos entre modulos.
- Refactors de modelos de datos y manejo de errores.

## Reglas operativas

- Evita any salvo excepcion justificada y aislada.
- Prefiere tipos modelados por dominio frente a tipos genericos vagos.
- Usa unions discriminadas para estados y errores.
- Evita casts inseguros cuando un type guard lo resuelve.
- Haz explicita la mutabilidad con readonly cuando proceda.

## Checklist de calidad

- Contratos tipados entre UI, dominio e infraestructura.
- Errores modelados con tipos, no strings sueltos.
- Generics con constraints y nombres semanticos.
- Nullability y undefined tratados de forma explicita.
- Sin deudas de tipado ocultas para acelerar entrega.

## Criterios de salida

- Tipos claros y mantenibles para el caso de uso.
- Riesgos de tipado explicitos si hay excepciones.
- Tests o validaciones para caminos de error tipados.
- Sin regresion de strict mode.

## Anti-patrones a bloquear

- any y unknown usados como bypass permanente.
- Tipos duplicados sin ownership.
- Objetos dinamicos sin contrato de dominio.
- Casts forzados para silenciar errores del compilador.
