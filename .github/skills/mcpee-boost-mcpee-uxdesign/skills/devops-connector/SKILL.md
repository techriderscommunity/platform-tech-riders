---
name: 'devops-connector'
description: 'Conecta con Azure DevOps o Jira, autentica al usuario, busca y extrae tareas/historias de usuario del backlog para procesarlas con otras skills de BoostDesign.'
---

# Skill: DevOps Connector

Este skill permite conectar directamente con el backlog del equipo (Azure DevOps o Jira) para extraer tareas funcionales e historias de usuario sin necesidad de copiar y pegar manualmente.

## Referencias y Documentación Oficial

- **Azure DevOps REST API**: https://learn.microsoft.com/en-us/rest/api/azure/devops/
- **Azure DevOps Personal Access Tokens**: https://learn.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate
- **Jira REST API**: https://developer.atlassian.com/cloud/jira/platform/rest/v3/
- **Jira API Authentication**: https://developer.atlassian.com/cloud/jira/platform/basic-auth-for-rest-apis/

## Sistemas Soportados

| Sistema | Autenticación | Protocolo |
|---------|---------------|-----------|
| Azure DevOps | PAT (Personal Access Token) | REST API v7 |
| Jira Cloud | API Token + email | REST API v3 |
| Jira Server | Basic Auth o PAT | REST API v2 |

## Capacidades

### 1. Autenticación y Conexión
- Solicita y valida el PAT o API Token del usuario
- Configura la URL base del proyecto (organización/proyecto)
- Verifica permisos de lectura antes de operar
- Nunca almacena credenciales — solo se usan en sesión

### 2. Búsqueda de Tareas
- Busca por sprint activo, iteración o etiqueta
- Filtra por tipo de tarea: Historia de Usuario, Bug, Tarea, Feature
- Filtra por estado: Nuevo, En progreso, Listo para revisión
- Filtra por responsable, etiqueta o área de producto

### 3. Extracción de Contenido
- Extrae: título, descripción, criterios de aceptación, comentarios clave
- Preserva el formato original (Markdown, HTML → Markdown)
- Incluye metadatos: ID, estado, responsable, sprint, fechas
- Soporta tareas con subtareas o tareas vinculadas

### 4. Integración con el Flujo BoostDesign
- Devuelve el contenido en formato listo para pasar a `functional-to-ux-spec`
- Puede encadenar automáticamente ambas skills en un solo comando
- Registra qué tarea se procesó para trazabilidad

## Comandos de Uso

```
# Buscar tareas UX del sprint actual
@devops-connector busca tareas UX en el sprint actual

# Extraer una tarea específica por ID
@devops-connector trae la tarea #12345 de Azure DevOps

# Buscar historias de usuario de un área
@devops-connector busca historias de usuario del área Pagos

# Encadenar con interpretación UX automáticamente
@devops-connector trae tarea #12345 e interpreta como spec UX
```

## Flujo Paso a Paso

1. **Configuración inicial** — El usuario proporciona:
   - URL del proyecto (ej. `https://dev.azure.com/org/project`)
   - PAT o API Token (se solicita de forma segura, no se guarda)
2. **Búsqueda** — La skill lista las tareas según los filtros indicados
3. **Selección** — El usuario elige la tarea a procesar
4. **Extracción** — Se extrae el contenido completo de la tarea
5. **Entrega** — Se pasa el texto a `functional-to-ux-spec` para generar la spec UX

## Seguridad

- Los tokens se usan únicamente durante la sesión activa
- No se persisten credenciales en ningún archivo del proyecto
- Se usa HTTPS para todas las llamadas a la API
- Mínimo privilegio: solo permisos de lectura (`Work Items - Read`)

## Artefactos de Salida

- Texto estructurado de la tarea (título + descripción + criterios)
- Metadatos: `{ id, título, estado, responsable, sprint, sistema }`
- Input listo para `functional-to-ux-spec`

## Integración con BoostDesign

```
@devops-connector → extrae tarea → @functional-to-ux-spec → spec UX completa
```

También puede usarse de forma independiente para consultar el backlog sin necesitar generar specs.
