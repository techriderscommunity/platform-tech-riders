# Documento Funcional Completo - Plataforma Web Tech Riders

**Especificación de requisitos funcionales - Parte Pública + Intranet**  
**Versión:** 1.2
**Fecha base:** 20/07/2026  
**Última revisión:** 25/07/2026  
**Producto:** Plataforma Web Tech Riders  
**Estado:** Documento funcional ampliado para producto, backlog, arquitectura y planificación de desarrollo

---

## Índice

1. [Descripción general del proyecto](#1-descripción-general-del-proyecto)
2. [Quiénes somos](#2-quiénes-somos)
3. [Visión de producto](#3-visión-de-producto)
4. [Objetivos funcionales](#4-objetivos-funcionales)
5. [Alcance](#5-alcance)
6. [Modelo de comunidad Tech Riders](#6-modelo-de-comunidad-tech-riders)
7. [Roles de usuario y permisos](#7-roles-de-usuario-y-permisos)
8. [Modelo de permisos granular](#8-modelo-de-permisos-granular)
9. [Módulos de la parte pública](#9-módulos-de-la-parte-pública)
10. [Módulos de la intranet](#10-módulos-de-la-intranet)
11. [Portales funcionales de intranet](#11-portales-funcionales-de-intranet)
12. [Integraciones técnicas](#12-integraciones-técnicas)
13. [Modelo de datos funcional](#13-modelo-de-datos-funcional)
14. [Taxonomías y catálogos maestros](#14-taxonomías-y-catálogos-maestros)
15. [Workflows principales](#15-workflows-principales)
16. [Casos de uso funcionales](#16-casos-de-uso-funcionales)
17. [Reglas de negocio](#17-reglas-de-negocio)
18. [MVP y roadmap de releases](#18-mvp-y-roadmap-de-releases)
19. [Backlog funcional inicial](#19-backlog-funcional-inicial)
20. [Épicas, features e historias de usuario](#20-épicas-features-e-historias-de-usuario)
21. [Requisitos no funcionales](#21-requisitos-no-funcionales)
22. [Seguridad, RGPD y auditoría](#22-seguridad-rgpd-y-auditoría)
23. [Métricas y reporting](#23-métricas-y-reporting)
24. [Riesgos, dependencias y decisiones pendientes](#24-riesgos-dependencias-y-decisiones-pendientes)
25. [Resumen ejecutivo](#25-resumen-ejecutivo)
26. [Anexo A - Propuesta de estructura GitHub Projects](#26-anexo-a---propuesta-de-estructura-github-projects)
27. [Anexo B - Definition of Ready y Definition of Done](#27-anexo-b---definition-of-ready-y-definition-of-done)

---

# 1. Descripción general del proyecto

## 1.1 Objetivo

Construir una plataforma web integral para **Tech Riders**, una comunidad tecnológica orientada a conectar educación, talento, profesionales, empresas, centros formativos y personas con interés en el sector tech.

La plataforma no debe entenderse solo como una web informativa, sino como el **sistema operativo digital de Tech Riders**: el espacio donde la comunidad se presenta, crece, organiza actividades, gestiona sesiones, coordina miembros, comparte conocimiento y amplifica su impacto.

## 1.2 Enfoque principal

El foco principal del producto es **Tech Riders como comunidad**.

Tajamar aparece como origen, entorno de referencia, centro impulsor o colaborador clave, pero la plataforma debe estar diseñada para que Tech Riders pueda crecer más allá de un único centro, integrando estudiantes, profesores, profesionales, empresas, Community Leaders, ambassadors, centros y comunidades afines.

## 1.3 Alcance general

El alcance incluye dos grandes áreas:

### Parte pública

Portal público de Tech Riders, orientado a:

- Explicar quiénes somos.
- Mostrar propósito, valores y propuesta de comunidad.
- Atraer nuevos miembros.
- Visibilizar sesiones, eventos, actividades y contenidos.
- Orientar a personas interesadas en tecnología.
- Conectar con empresas, centros y profesionales.

### Intranet

Área privada para miembros Tech Riders, Staff, Community Leaders, Ambassadors, profesores, estudiantes, empresas, centros y administradores.

Incluye:

- Gestión de miembros.
- Gestión del programa Ambassadors.
- Gestión de sesiones.
- Gestión de actividades, eventos y formaciones.
- Gestión de disponibilidad y participación.
- Call for Speakers / Call for Sessions.
- Banco de conocimiento.
- Administración.
- Métricas y reporting.

# 2. Quiénes somos

## 2.1 Definición

**Tech Riders** es una comunidad tecnológica que nace con el propósito de acercar el mundo educativo y el mundo profesional, generando espacios reales de aprendizaje, conexión, orientación, inspiración y crecimiento.

Tech Riders conecta personas que quieren aprender, enseñar, compartir experiencias, impulsar talento y construir comunidad alrededor de la tecnología.

## 2.2 Propósito

El propósito de Tech Riders es **reducir la distancia entre la formación y la realidad profesional del sector tecnológico**.

Para ello, la comunidad facilita:

- Sesiones técnicas y de orientación.
- Encuentros con profesionales.
- Actividades formativas.
- Eventos de comunidad.
- Testimonios reales.
- Acompañamiento a estudiantes y perfiles junior.
- Conexión entre centros, empresas y talento.
- Espacios donde cualquier persona pueda aportar desde su experiencia.

## 2.3 Misión

Impulsar una comunidad tecnológica abierta, cercana y práctica, donde estudiantes, profesionales, docentes, empresas y centros puedan conectar, aprender y construir oportunidades reales alrededor de la tecnología.

## 2.4 Visión

Convertir Tech Riders en una comunidad de referencia para conectar talento tech, educación y empresa, creando un ecosistema sostenible donde las personas puedan crecer, participar, compartir conocimiento y abrir nuevas oportunidades.

## 2.5 Valores

| Valor | Descripción |
|---|---|
| Comunidad | Tech Riders existe para conectar personas y crear vínculos reales. |
| Aprendizaje práctico | La tecnología se aprende mejor con ejemplos, experiencias y proyectos reales. |
| Generosidad | Compartir conocimiento y experiencia ayuda a que otros crezcan. |
| Cercanía | La comunidad debe ser accesible, humana y sin barreras innecesarias. |
| Diversidad | Cualquier persona con interés en tech puede encontrar su espacio. |
| Impacto | Las actividades deben aportar valor real a estudiantes, profesionales, centros y empresas. |
| Evolución continua | La comunidad debe adaptarse al sector, a las personas y a las nuevas oportunidades. |

## 2.6 Qué hacemos

Tech Riders organiza, impulsa y facilita:

- Sesiones técnicas.
- Sesiones de orientación.
- Sesiones de empleabilidad.
- Sesiones de soft skills.
- Eventos de comunidad.
- Formación complementaria.
- Podcasts.
- Mentoring o acompañamiento.
- Actividades con centros educativos.
- Actividades con empresas.
- Banco de conocimiento.
- Contenido divulgativo.

## 2.7 Para quién es Tech Riders

Tech Riders está orientado a:

- Estudiantes.
- Profesores.
- Profesionales junior.
- Profesionales senior.
- Empresas.
- Centros formativos.
- Orientadores.
- Personas interesadas en iniciarse en tecnología.
- Comunidades colaboradoras.
- Personas que quieren aportar conocimiento o experiencia.

## 2.8 Cómo se participa

Una persona puede participar en Tech Riders de distintas formas:

- Uniéndose como miembro de la comunidad.
- Asistiendo a sesiones y eventos.
- Compartiendo conocimiento como Ambassador.
- Ayudando a organizar actividades como Community Leaders.
- Proponiendo sesiones.
- Solicitando sesiones para un centro o grupo.
- Creando contenido.
- Participando en podcasts, talleres o formaciones.
- Conectando empresas, centros y talento.

---

# 3. Visión de producto

## 3.1 Propuesta de valor

La plataforma Tech Riders debe ser el punto de encuentro digital de la comunidad.

Debe permitir que cualquier persona entienda qué es Tech Riders, cómo puede participar, qué actividades existen, qué conocimiento se está generando y cómo puede contribuir al crecimiento de la comunidad.

## 3.2 Principios de diseño funcional

1. **Tech Riders como centro del producto.**  
   Todo debe girar alrededor de la comunidad, sus miembros, sus sesiones, sus actividades y su capacidad de conectar personas.

2. **Miembros antes que usuarios.**  
   La plataforma no debe tratar a las personas solo como cuentas, sino como miembros con una relación viva con la comunidad.

3. **Sesiones como unidad principal de actividad.**  
   Las sesiones sustituyen el concepto de “charla” y representan cualquier actividad estructurada donde se comparte conocimiento, experiencia u orientación.

4. **Ambassadors como motor de extensión.**  
   Los Ambassadors son personas activas que ayudan a construir, representar y extender la comunidad.

5. **Staff y Community Leaders como estructura operativa.**  
   Staff y Community Leaders dan forma, continuidad y dirección a Tech Riders.

6. **Abierto, escalable y multi-centro.**  
   Aunque Tajamar sea un origen o nodo relevante, la plataforma debe poder crecer con otros centros, empresas y comunidades.

7. **Práctico y sin humo.**  
   La plataforma debe facilitar actividades reales, útiles y accionables.

---

# 4. Objetivos funcionales

## 4.1 Objetivos de comunidad

- Dar visibilidad a Tech Riders como comunidad.
- Facilitar la incorporación de nuevos miembros.
- Centralizar sesiones, eventos, formaciones y actividades.
- Facilitar que centros, profesores y orientadores soliciten sesiones.
- Facilitar que Ambassadors participen activamente en actividades.
- Impulsar contenido y conocimiento generado por la comunidad.
- Conectar estudiantes, profesionales, empresas y centros.

## 4.2 Objetivos de producto

- Crear una parte pública clara, atractiva y orientada a comunidad.
- Crear una intranet operativa para miembros Tech Riders.
- Gestionar membresía, roles y permisos.
- Gestionar sesiones de forma trazable.
- Gestionar Call for Sessions / Call for Speakers.
- Gestionar actividades y eventos.
- Gestionar contenidos.
- Medir actividad e impacto.

## 4.3 Objetivos operativos

- Reducir la gestión manual de sesiones.
- Mejorar la coordinación entre Staff, Community Leaders y Ambassadors.
- Evitar duplicidad de datos con GPF y SharePoint.
- Automatizar notificaciones clave.
- Mantener histórico de participación.
- Facilitar una planificación por releases.

---

# 5. Alcance

## 5.1 Incluido en alcance

### Parte pública

- Home pública de Tech Riders.
- Sección “Quiénes somos”.
- Sección “Únete a Tech Riders”.
- Explicación de Member, Staff, Community Leaders y Ambassadors.
- Sesiones y actividades destacadas.
- Calendario público.
- Banco de conocimiento.
- Perfiles tech y orientación.
- Sección para centros.
- Sección para empresas.
- Woman Tech.
- Ofertas y oportunidades.
- Contacto por audiencia.

### Intranet

- Login social.
- Alta como miembro Tech Riders.
- Gestión de perfil de miembro.
- Solicitud para ser Ambassador.
- Gestión de Staff y Community Leaders.
- Gestión de sesiones.
- Solicitudes de sesiones.
- Asignación de Ambassadors a sesiones.
- Gestión de disponibilidad.
- Call for Sessions / Call for Speakers.
- Gestión de eventos y formaciones.
- Banco de conocimiento.
- Panel de administración.
- Informes y métricas.

## 5.2 Fuera de alcance inicial

- Gamificación completa.
- Recomendaciones IA.
- Marketplace avanzado de empleo.
- Ranking público automático de Ambassadors.
- Mentorías complejas.

---

# 6. Modelo de comunidad Tech Riders

## 6.1 Concepto de Member Tech Riders

Un **Member Tech Riders** es cualquier persona que se une a la comunidad y quiere formar parte de ella.

Ser miembro no implica necesariamente organizar, impartir sesiones o participar de forma activa en todas las iniciativas. Es el nivel base de pertenencia a la comunidad.

## 6.2 Tipos de participación dentro de Tech Riders

| Tipo | Descripción | Nivel de implicación | Ejemplos de actividad |
|---|---|---|---|
| Staff | Personas que lideran, coordinan o toman decisiones sobre la comunidad | Alto | Estrategia, coordinación, gobierno, planificación, validación |
| Community Leaders | Personas que ayudan a dar forma y operar iniciativas de Tech Riders | Medio/Alto | Organización de eventos, apoyo en sesiones, contenidos, dinamización |
| Ambassador | Personas que participan activamente en actividades y ayudan a construir y extender comunidad | Alto | Impartir sesiones, participar en podcasts, mentorías, formaciones, eventos, representación comunitaria |
| Member | Persona que se une a la comunidad | Base | Asistir a sesiones, recibir información, participar puntualmente |

## 6.3 Staff

El **Staff** representa el núcleo de coordinación de Tech Riders.

### Responsabilidades

- Definir visión y prioridades.
- Validar iniciativas.
- Coordinar calendario de actividades.
- Gestionar comunidad.
- Aprobar incorporaciones a roles activos cuando aplique.
- Supervisar métricas e impacto.
- Cuidar la coherencia del proyecto.

### Capacidades en plataforma

- Acceso a panel de administración.
- Gestión de miembros.
- Gestión de sesiones.
- Gestión de eventos.
- Gestión de Ambassadors.
- Gestión de Community Leaders.
- Consulta de métricas.
- Gestión de contenido.

## 6.4 Community Leaders

El **Community Leaders** es un miembro que ayuda activamente a dar forma a Tech Riders, sin necesariamente pertenecer al núcleo de Staff.

### Responsabilidades posibles

- Apoyar en organización de sesiones.
- Proponer actividades.
- Ayudar en comunicación.
- Crear o revisar contenido.
- Dinamizar comunidad.
- Apoyar eventos.
- Ayudar a conectar personas, centros o empresas.

### Capacidades en plataforma

- Proponer sesiones.
- Ayudar a gestionar actividades asignadas.
- Acceder a espacios internos.
- Crear contenido si tiene permiso.
- Consultar planificación.

## 6.5 Ambassador

El **Ambassador** es una persona que participa activamente en actividades de Tech Riders y ayuda a construir y extender la comunidad.

Puede ser:

- Profesional.
- Alumno.
- Profesor.
- Antiguo alumno.
- Persona de empresa.
- Persona de otra comunidad.
- Perfil junior.
- Perfil senior.
- Especialista técnico.
- Persona que aporta experiencia, orientación o inspiración.

### Responsabilidades posibles

- Impartir sesiones.
- Participar en podcasts.
- Participar en formaciones.
- Participar en mentorías.
- Compartir experiencia profesional.
- Representar Tech Riders en actividades.
- Proponer contenido.
- Ayudar a extender comunidad.

### Capacidades en plataforma

- Gestionar perfil público o interno de Ambassador.
- Indicar áreas de conocimiento.
- Gestionar disponibilidad.
- Recibir propuestas de sesiones.
- Postularse a Call for Sessions.
- Consultar histórico de participación.
- Proponer actividades.

## 6.6 Relación entre roles de comunidad y perfiles funcionales

Una persona puede ser miembro Tech Riders y además tener un perfil funcional.

Ejemplos:

- Alumno + Member
- Alumno + Ambassador.
- Profesor + Community Leaders.
- Profesional Senior + Ambassador.
- Empresa + Community Leaders.
- Staff + Ambassador.
- Profesional Junior + Member + Ambassador.

Esto implica que el sistema debe separar:

1. **Relación con la comunidad**: Member, Staff, Community Leaders, Ambassador.
2. **Perfil de la persona**: alumno, profesor, profesional, empresa, orientador, visitante, etc.
3. **Permisos reales**: acciones que puede realizar en plataforma.

---

# 7. Roles de usuario y permisos

## 7.1 Roles de comunidad

| Rol comunidad | Descripción | Acceso público | Acceso intranet | Funcionalidades principales | Registro GPF |
|---|---|---:|---:|---|---|
| Staff | Núcleo que coordina y gobierna Tech Riders | N/A | Sí | Administración, planificación, gestión de miembros, sesiones, eventos y reporting | Sí, etiqueta Staff |
| Community Leaders | Miembro que ayuda a construir y operar iniciativas | Sí | Sí | Proponer actividades, apoyar sesiones, crear contenido, dinamizar comunidad | Sí, etiqueta Community Leaders |
| Ambassador | Persona activa que participa en actividades y extiende comunidad | Sí | Sí | Impartir sesiones, participar en eventos, podcasts, formaciones, mentorías y Call for Sessions | Sí, etiqueta Ambassador |
| Member | Persona que se une a la comunidad | Sí | Sí | Perfil, eventos, sesiones, contenidos, intereses, participación | Sí |

## 7.2 Perfiles funcionales complementarios

| Perfil | Descripción | Funcionalidades principales |
|---|---|---|
| Estudiante | Alumno actual o persona en formación | Ver eventos, sesiones, formaciones, orientación, oportunidades |
| Profesor | Docente de centro formativo | Solicitar sesiones, proponer actividades, acceder a recursos |
| Profesional Junior | Persona en primeros años de carrera | Participar, aprender, convertirse en Ambassador, acceder a oportunidades |
| Profesional Senior | Profesional con experiencia | Participar como Ambassador, mentor, speaker o Community Leaders |
| Empresa | Organización del sector tech | Proponer sesiones, participar en eventos, conectar con talento |
| Orientador | Persona que orienta estudiantes o familias | Solicitar sesiones de orientación y recursos |
| Centro | Centro educativo o formativo | Solicitar sesiones, gestionar actividad asociada |
| Visitante | Usuario sin autenticación | Consultar contenido público e iniciar registro |

## 7.3 Multi-rol

La plataforma debe permitir multi-rol real.

Ejemplo:

```text
Persona A:
- Member
- Ambassador
- Profesional Senior

Persona B:
- Member
- Community Leaders
- Profesor

Persona C:
- Staff
- Ambassador
- Profesional Senior
```

## 7.4 Principio clave

Los roles de comunidad explican **cómo participa una persona en Tech Riders**.  
Los perfiles funcionales explican **qué tipo de persona o entidad es**.  
Los permisos explican **qué puede hacer realmente en la plataforma**.

---

# 8. Modelo de permisos granular

## 8.1 Permisos de membresía

| Permiso | Descripción |
|---|---|
| members.join | Solicitar alta como miembro Tech Riders |
| members.read.self | Ver perfil propio |
| members.update.self | Editar perfil propio |
| members.read.all | Ver miembros |
| members.manage | Gestionar miembros |
| members.assign.communityRole | Asignar roles de comunidad |
| members.deactivate | Desactivar miembro |

## 8.2 Permisos Staff y Community Leaders

| Permiso | Descripción |
|---|---|
| staff.access | Acceder a funcionalidades de Staff |
| staff.manage.strategy | Gestionar configuración estratégica |
| community-leaders.manage | Gestionar Community Leaders |
| community-leaders.activities.support | Apoyar actividades asignadas |
| community.planning.read | Ver planificación interna |
| community.planning.manage | Gestionar planificación interna |

## 8.3 Permisos Ambassadors

| Permiso | Descripción |
|---|---|
| ambassadors.apply | Solicitar ser Ambassador |
| ambassadors.profile.manage | Gestionar perfil Ambassador |
| ambassadors.availability.manage | Gestionar disponibilidad |
| ambassadors.read.all | Ver Ambassadors |
| ambassadors.manage | Gestionar Ambassadors |
| ambassadors.assign.session | Asignar Ambassador a sesión |
| ambassadors.history.read.self | Ver histórico propio |
| ambassadors.history.read.all | Ver histórico general |

## 8.4 Permisos de sesiones

| Permiso | Descripción |
|---|---|
| sessions.request | Solicitar sesión |
| sessions.propose | Proponer sesión |
| sessions.read.public | Ver sesiones públicas |
| sessions.read.private | Ver sesiones internas |
| sessions.read.own | Ver sesiones propias |
| sessions.read.all | Ver todas las sesiones |
| sessions.create | Crear sesión |
| sessions.update | Modificar sesión |
| sessions.cancel | Cancelar sesión |
| sessions.assign.ambassador | Asignar Ambassador |
| sessions.manage.rooms | Gestionar sala o enlace |
| sessions.confirm | Confirmar sesión |
| sessions.close | Cerrar sesión realizada |

## 8.5 Permisos de eventos y formaciones

| Permiso | Descripción |
|---|---|
| events.read.public | Ver eventos públicos |
| events.read.private | Ver eventos privados |
| events.create | Crear eventos |
| events.update | Editar eventos |
| events.publish | Publicar eventos |
| events.register | Inscribirse en eventos |
| trainings.read | Ver formaciones |
| trainings.manage | Gestionar formaciones |
| trainings.register | Inscribirse en formaciones |

## 8.6 Permisos de Call for Sessions

| Permiso | Descripción |
|---|---|
| callsessions.read | Ver convocatorias abiertas |
| callsessions.create | Crear Call for Sessions |
| callsessions.apply | Postularse a una convocatoria |
| callsessions.manage | Gestionar postulaciones |
| callsessions.select | Seleccionar propuestas |

## 8.7 Mapeo inicial rol-permisos

| Rol | Permisos clave |
|---|---|
| Visitante | sessions.read.public, events.read.public, content.read.public |
| Member | members.read.self, members.update.self, events.register, trainings.register, sessions.read.private |
| Community Leaders | sessions.propose, community.planning.read, community-leaders.activities.support, content.create |
| Ambassador | ambassadors.profile.manage, ambassadors.availability.manage, sessions.read.own, callsessions.apply, ambassadors.history.read.self |
| Staff | staff.access, members.manage, ambassadors.manage, community-leaders.manage, sessions.read.all, sessions.assign.ambassador, events.create, reports.read |
| Tetxito | Modo dios. Todos los permisos de administración, auditoría, configuración e integración |

---

# 9. Módulos de la parte pública

## 9.1 Home pública Tech Riders

### Objetivo

Presentar Tech Riders como comunidad, transmitir propósito y guiar a cada audiencia hacia la acción adecuada.

### Contenido mínimo

- Hero principal: “Somos Tech Riders”.
- Qué es Tech Riders.
- Quiénes somos.
- Qué hacemos.
- Cómo unirse.
- Próximas sesiones y eventos.
- Ambassadors destacados o testimonios.
- Accesos para estudiantes, profesores, profesionales, empresas y centros.
- Banco de conocimiento.
- Redes sociales.

### CTAs principales

- Únete a Tech Riders.
- Quiero ser Ambassador.
- Solicitar una sesión.
- Proponer una sesión.
- Ver próximas actividades.
- Contactar con Tech Riders.

## 9.2 Quiénes somos

Página pública dedicada a explicar:

- Origen de Tech Riders.
- Propósito.
- Misión.
- Visión.
- Valores.
- Qué hacemos.
- Cómo se puede participar.
- Qué significa ser miembro.
- Qué son Staff, Community Leaders y Ambassadors.

## 9.3 Únete a Tech Riders

### Funcionalidades

- Explicación de membresía.
- Formulario o CTA de alta.
- Beneficios de unirse.
- Niveles de participación.
- Pregunta opcional: “¿Quieres participar activamente como Ambassador?”.

## 9.4 Ambassadors

### Contenido

- Qué es un Ambassador.
- Quién puede ser Ambassador.
- Qué tipo de actividades puede realizar.
- Cómo solicitarlo.
- Testimonios.
- Ambassadors destacados, si se decide mostrar.

## 9.5 Sesiones

### Objetivo

Mostrar el catálogo de sesiones pasadas, futuras o solicitables.

### Tipos de sesión

- Técnica.
- Orientación.
- Empleabilidad.
- Soft Skills.
- Comunidad.
- Empresa.
- Inspiracional.
- Woman Tech.
- Workshop.
- Podcast.
- Mentoría.

### Funcionalidades

- Ver sesiones programadas.
- Filtrar por temática.
- Filtrar por modalidad.
- Filtrar por nivel.
- Solicitar sesión.
- Proponer sesión.
- Ver Ambassadors asociados si aplica.

## 9.6 Perfiles Tech

Módulo orientativo para explicar perfiles profesionales del sector tecnológico.

Categorías iniciales:

- Desarrollo.
- Sistemas.
- Data.
- Ciberseguridad.
- DevOps.
- QA.
- IoT.
- Blockchain.
- Consultoría.
- ERP / CRM.
- Arquitectura.
- UX/UI.

## 9.7 Qué estudiar

Secciones:

- FP.
- Másteres FP.
- Certificados profesionales.
- Grados y másteres.
- Certificaciones.
- Rutas recomendadas por perfil, siempre como información orientativa, no como prescripción cerrada.

## 9.8 Centros y dónde estudiar

Funcionalidades:

- Buscador por provincia.
- Listado de centros.
- Ficha de centro.
- Estudios ofrecidos.
- Solicitud de sesión para centro.

## 9.9 Empresas

Secciones:

- Participa con Tech Riders.
- Propón una sesión.
- Colabora con eventos.
- Conecta con talento.
- Comparte casos reales.
- Publica oportunidades, si se habilita.

## 9.10 Woman Tech

Sección específica para visibilizar mujeres en tecnología, testimonios, recursos, asociaciones, ayudas, becas y sesiones específicas.

## 9.11 Banco de conocimiento

Incluye:

- Tutoriales.
- Podcast grabados.
- Artículos.
- Casos de éxito.
- Recursos descargables.
- Materiales de sesiones.

## 9.12 Contacto

Formularios por audiencia:

- Quiero unirme.
- Soy estudiante.
- Soy profesor.
- Soy profesional.
- Soy empresa.
- Soy centro.
- Quiero solicitar una sesión.
- Quiero proponer una colaboración.

---

# 10. Módulos de la intranet

## 10.1 Sistema de autenticación

| Funcionalidad | Descripción | Integración | Datos capturados | Validación | Prioridad |
|---|---|---|---|---|---|
| Login social | Acceso mediante Google, LinkedIn o Microsoft | OAuth 2.0 | Email, nombre, foto | Email verificado | Alta |
| Alta miembro | Alta como miembro Tech Riders | Forms / App + GPF | Nombre, email, intereses, perfil, centro | Validación GPF | Alta |
| Solicitud Ambassador | Solicitud para participar activamente | GPF / SharePoint | Bio, especialidades, disponibilidad | Aprobación Staff/Admin | Alta |
| Gestión perfil | Actualización de datos del miembro | GPF | Datos editables | Actualización etiquetas | Alta |
| Cierre de sesión | Salida segura | OAuth / App | N/A | Invalidación sesión | Alta |

## 10.2 Perfil de miembro Tech Riders

Datos editables:

- Nombre.
- Apellidos.
- Foto.
- Bio corta.
- Perfil funcional.
- Intereses.
- Temáticas.
- Centro o empresa.
- Redes sociales.
- Preferencias de notificación.
- Visibilidad del perfil.

Datos internos:

- ID.
- Roles de comunidad.
- Estado de membresía.
- Estado Ambassador.
- Histórico de sesiones.
- Auditoría.

---

# 11. Portales funcionales de intranet

## 11.1 Portal Miembro Tech Riders

| Funcionalidad | Descripción | Prioridad |
|---|---|---|
| Mi perfil | Consultar y editar datos personales | Alta |
| Mis intereses | Gestionar temáticas de interés | Alta |
| Próximas actividades | Ver eventos, sesiones y formaciones | Alta |
| Inscripciones | Consultar actividades en las que participa | Alta |
| Banco de conocimiento | Acceder a recursos internos | Media |
| Solicitar ser Ambassador | Iniciar solicitud de participación activa | Alta |
| Proponer sesión | Proponer una sesión como miembro | Media |

## 11.2 Portal Ambassador

| Funcionalidad | Descripción | Tipo | Workflow | Integración | Prioridad |
|---|---|---|---|---|---|
| Mi perfil Ambassador | Gestionar bio, foto, especialidades y RRSS | Formulario | Edición → Validación → Publicación interna | GPF | Alta |
| Gestionar disponibilidad | Indicar días/horas disponibles | Calendario | Selección → Guardado | SharePoint | Alta |
| Ver sesiones propuestas | Listado de sesiones pendientes de respuesta | Vista lista | Ver → Aceptar/Rechazar | SharePoint | Alta |
| Mis sesiones asignadas | Calendario con sesiones confirmadas | Vista calendario | Visualización → Detalles | SharePoint | Alta |
| Call for Sessions | Postularse para sesiones/eventos abiertos | Formulario | Postulación → Selección → Confirmación | SharePoint | Alta |
| Histórico de participación | Registro de sesiones y actividades realizadas | Vista lista | Consulta histórico | SharePoint | Media |

## 11.3 Portal Staff

| Funcionalidad | Descripción | Prioridad |
|---|---|---|
| Gestión comunidad | Consultar y gestionar miembros | Alta |
| Gestión Ambassadors | Aprobar, editar y revisar Ambassadors | Alta |
| Gestión Community Leaders | Gestionar personas que apoyan iniciativas | Alta |
| Gestión sesiones | Revisar, asignar, confirmar y cerrar sesiones | Alta |
| Planificación | Ver calendario global de actividad | Alta |
| Métricas | Consultar actividad e impacto | Media |
| Configuración | Catálogos, temáticas, estados y permisos | Media |

## 11.4 Portal Community Leaders

| Funcionalidad | Descripción | Prioridad |
|---|---|---|
| Actividades asignadas | Ver iniciativas donde colabora | Alta |
| Proponer actividad | Proponer eventos, sesiones o contenidos | Alta |
| Apoyar sesiones | Consultar necesidades de apoyo | Media |
| Crear contenido | Crear borradores o recursos si tiene permiso | Media |
| Planificación comunitaria | Ver calendario interno o parcial | Media |

## 11.5 Portal Centros / Profesores / Orientadores

| Funcionalidad | Descripción | Prioridad |
|---|---|---|
| Solicitar sesión | Pedir una sesión para alumnos o grupo concreto | Alta |
| Consultar estado | Ver estado de solicitudes realizadas | Alta |
| Ver recursos | Acceder a materiales de orientación | Media |
| Proponer colaboración | Proponer actividad conjunta | Media |

## 11.6 Portal Empresa

| Funcionalidad | Descripción | Prioridad |
|---|---|---|
| Perfil empresa | Datos básicos y contacto | Media |
| Proponer sesión | Proponer sesión técnica, experiencia o empleabilidad | Alta |
| Participar en eventos | Solicitar participación en actividades | Media |
| Publicar oportunidades | Publicar oportunidades si se habilita | Media |

## 11.7 Panel de administración

| Funcionalidad | Descripción | Tipo | Workflow | Integración | Prioridad |
|---|---|---|---|---|---|
| Gestión miembros | Alta, baja, modificación y roles | CRUD | Acción → GPF → Confirmación | GPF | Alta |
| Gestión roles comunidad | Staff, Community Leaders, Ambassador, Miembro | CRUD | Crear/Editar/Asignar | GPF | Alta |
| Gestión sesiones | Crear, revisar, asignar y cerrar sesiones | CRUD/Workflow | Solicitud → Asignación → Confirmación → Cierre | SharePoint | Alta |
| Asignación Ambassadors | Asignar personas activas a sesiones | Matching manual/asistido | Ver solicitud → Asignar → Notificar | SharePoint + Email | Alta |
| Gestión salas/enlaces | Reservar sala o generar enlace | Calendario | Buscar disponibilidad → Reservar | Outlook/Graph | Alta |
| Gestión eventos | CRUD de eventos | CRUD | Crear → Publicar → Notificar | SharePoint | Alta |
| Gestión contenido | Blog, tutoriales, talks y recursos | CMS | Borrador → Publicación | CMS/SharePoint | Alta |
| Call for Sessions | Crear convocatorias abiertas | Formulario | Crear → Publicar → Recibir → Seleccionar | SharePoint | Alta |

---

# 12. Integraciones técnicas

| Sistema | Tipo | Propósito | Método integración | Datos intercambiados | Frecuencia |
|---|---|---|---|---|---|
| GPF | Base de datos SQL | Gestión centralizada de miembros, perfiles, roles y etiquetas | Procedimientos almacenados SQL | Miembros, roles, etiquetas, intereses, centros | Tiempo real / near real-time |
| SharePoint Lists | Lista SharePoint | Almacenamiento operativo de sesiones, eventos, formaciones e inscripciones | REST API / Graph API | Sesiones, eventos, formaciones, inscripciones, solicitudes | Tiempo real |
| Microsoft Forms | Formularios | Captura inicial o formularios sencillos | Power Automate / API | Datos de alta, contacto y solicitudes | Por evento |
| OAuth Providers | Autenticación | Login social | OAuth 2.0 | Token, email, nombre, foto | Por login |
| Power Automate | Automatización | Workflows y notificaciones | Conectores nativos | Triggers, acciones y emails | Por evento |
| Outlook / Exchange | Calendario | Reserva de salas y gestión de agenda | Graph API | Disponibilidad, reservas, convocatorias | Tiempo real |
| YouTube | Video hosting | Talks, testimonios, podcasts y vídeos | Embed / API | URLs de vídeos | Manual / programado |

---

# 13. Modelo de datos funcional

## 13.1 Entidades principales

| Entidad | Campos principales | Origen | Relaciones | Etiquetas GPF | Notas |
|---|---|---|---|---|---|
| Miembro | ID, nombre, apellido, email, foto, bio, intereses, perfil funcional, estado | GPF + OAuth | Roles comunidad N:M, sesiones N:M, eventos N:M | Miembro, Staff, Community Leaders, Ambassador | Entidad principal |
| RolComunidad | ID, nombre, descripción | GPF / App | Miembros N:M | Miembro, Staff, Community Leaders, Ambassador | Define relación con Tech Riders |
| PerfilFuncional | ID, nombre, descripción | App / GPF | Miembros N:M | Alumno, Profesor, Profesional, Empresa, Centro | Define naturaleza del miembro |
| Permiso | ID, código, descripción, módulo | App | Roles N:M | N/A | Claims granulares |
| AmbassadorProfile | ID, miembroId, bioSpeaker, especialidades, RRSS, disponibilidad, estado | GPF / SharePoint | Miembro 1:1, sesiones N:M | Ambassador | Perfil activo de participación |
| Sesión | ID, título, descripción, tipo, temática, fecha, modalidad, estado, solicitante, ambassador | SharePoint List | Miembros N:M, Ambassador N:M, Centro N:1 | Tipo sesión | Sustituye a charla |
| Evento | ID, título, fecha, ubicación, descripción, tipo, temática | SharePoint List | Miembros inscritos N:M, sesiones N:M | Propio, comunidad, partner | Actividad de mayor escala |
| Formación | ID, título, fecha, modalidad, descripción, nivel | SharePoint | Miembros inscritos N:M | Por temática | Puede ser ActivityType |
| Centro | ID, nombre, dirección, provincia, tipo | GPF / CMS | Miembros N:M, sesiones N:M | Centro Tech, Centro Escolar | Multi-centro |
| Empresa | ID, nombre, sector, contacto, web | GPF / CMS | Sesiones N:M, eventos N:M | Partner, Empresa | Puede proponer actividades |
| Interés/Temática | ID, nombre, categoría | Tabla común | Miembros N:M, sesiones N:M, eventos N:M, contenido N:M | N/A | Personalización |
| Contenido | ID, título, contenido, autor, fecha, tipo, temáticas, estado | CMS / SharePoint | Temáticas N:M, autor N:1 | Por tipo | Tutorial, talk, blog, podcast |
| Inscripción | ID, miembroId, activityId, estado, fechaAlta | SharePoint / App | Miembro N:1, Activity N:1 | N/A | Eventos/formaciones |
| CallForSessions | ID, activityId, fechaInicio, fechaFin, estado, temáticas | SharePoint | Postulaciones 1:N | N/A | Convocatorias abiertas |
| PostulaciónSession | ID, callId, ambassadorId, propuesta, estado | SharePoint | Ambassador N:1 | N/A | Selección Staff/Admin |
| Auditoría | ID, usuario, acción, entidad, fecha, detalle | App | N/A | N/A | Trazabilidad |

## 13.2 Entidad Activity recomendada

Se recomienda unificar eventos, formaciones, sesiones, workshops, podcasts y mentorías bajo una entidad funcional común.

```text
Activity
- ActivityId
- Title
- Description
- ActivityType
- ActivitySubtype
- StartDate
- EndDate
- Location
- Modality
- Level
- TopicIds
- AmbassadorIds
- RegistrationEnabled
- Capacity
- Status
- SourceSystem
```

### ActivityType inicial

- Sesión.
- Evento.
- Formación.
- Workshop.
- Podcast.
- Mentoría.
- Encuentro comunidad.
- Orientación.

---

# 14. Taxonomías y catálogos maestros

## 14.1 Roles comunidad

- Members.
- Staff.
- Community Leaders.
- Ambassadors.

## 14.2 Perfiles funcionales

- Estudiante.
- Profesor.
- Profesional Junior.
- Profesional Senior.
- Empresa.
- Centro.
- Orientador.
- Visitante.

## 14.3 Tipos de sesión

- Técnica.
- Orientación.
- Empleabilidad.
- Soft Skills.
- Inspiracional.
- Empresa.
- Woman Tech.
- Comunidad.
- Workshop.
- Podcast.
- Mentoría.

## 14.4 Estados de sesión

- Borrador.
- Solicitada.
- En revisión.
- Pendiente de Ambassador.
- Ambassador propuesto.
- Aceptada por Ambassador.
- Rechazada por Ambassador.
- Pendiente de sala/enlace.
- Confirmada.
- Realizada.
- Cancelada.
- Cerrada.

## 14.5 Temáticas tech

- Desarrollo Frontend.
- Desarrollo Backend.
- Full Stack.
- Cloud.
- Azure.
- DevOps.
- Data.
- IA.
- Machine Learning.
- Ciberseguridad.
- Sistemas.
- Redes.
- QA Testing.
- Arquitectura de software.
- Power Platform.
- IoT.
- Blockchain.
- UX/UI.
- Productividad.
- Soft Skills.
- Empleabilidad.

## 14.6 Modalidades

- Presencial.
- Online.
- Híbrido.

## 14.7 Niveles

- Introductorio.
- Básico.
- Intermedio.
- Avanzado.
- Experto.

---

# 15. Workflows principales

## 15.1 Alta como miembro Tech Riders

| Elemento | Detalle |
|---|---|
| Actor | Visitante |
| Pasos | 1. Login social → 2. Completar alta → 3. Validación GPF → 4. Asignación rol Miembro → 5. Confirmación |
| Sistemas | OAuth + Forms/App + GPF |
| Notificación | Email confirmación alta |
| Resultado | Miembro Tech Riders registrado |

## 15.2 Solicitud para ser Ambassador

| Elemento | Detalle |
|---|---|
| Actor | Miembro Tech Riders |
| Pasos | 1. Solicitar ser Ambassador → 2. Completar perfil → 3. Revisión Staff/Admin → 4. Aprobación → 5. Etiqueta Ambassador |
| Sistemas | App + GPF + SharePoint |
| Notificación | Email aprobación/rechazo |
| Resultado | Ambassador activo |

## 15.3 Solicitud de sesión

| Elemento | Detalle |
|---|---|
| Actor | Profesor / Centro / Orientador / Empresa / Miembro |
| Pasos | 1. Rellenar solicitud → 2. Staff revisa → 3. Asigna Ambassador → 4. Confirma sala/enlace → 5. Notifica partes |
| Sistemas | GPF + Outlook |
| Notificación | Email a solicitante y Ambassador |
| Resultado | Sesión programada |

## 15.4 Propuesta de sesión por Ambassador o Community Leaders

| Elemento | Detalle |
|---|---|
| Actor | Ambassador / Community Leaders |
| Pasos | 1. Propone sesión → 2. Staff revisa → 3. Se aprueba o ajusta → 4. Se programa → 5. Se publica |
| Sistemas | App + GPF |
| Notificación | Confirmación de propuesta |
| Resultado | Sesión disponible en calendario |

## 15.5 Call for Sessions

| Elemento | Detalle |
|---|---|
| Actor | Staff + Ambassadors |
| Pasos | 1. Staff crea convocatoria → 2. Ambassadors postulan → 3. Staff selecciona → 4. Notifica seleccionados |
| Sistemas | GPF + Email |
| Notificación | Convocatoria y selección |
| Resultado | Ambassadors asignados a actividad |

## 15.6 Inscripción a actividad

| Elemento | Detalle |
|---|---|
| Actor | Miembro / Visitante si se permite |
| Pasos | 1. Ver calendario → 2. Seleccionar actividad → 3. Inscribirse → 4. Confirmación |
| Sistemas | GPF |
| Notificación | Email confirmación inscripción |
| Resultado | Persona inscrita |

---

# 16. Casos de uso funcionales

## UC-001 - Unirse a Tech Riders

**Actor principal:** Visitante  
**Objetivo:** Registrarse como miembro de la comunidad Tech Riders.

### Flujo principal

1. El visitante accede a “Únete a Tech Riders”.
2. El sistema explica qué significa ser miembro.
3. El visitante inicia sesión con proveedor social.
4. Completa datos básicos.
5. Selecciona intereses y perfil funcional.
6. El sistema valida email y registra en GPF.
7. Se asigna rol Miembro Tech Riders.
8. El sistema muestra confirmación.

### Resultado

Nuevo miembro Tech Riders registrado.

---

## UC-002 - Solicitar ser Ambassador

**Actor principal:** Miembro Tech Riders  
**Objetivo:** Participar activamente en actividades de comunidad.

### Flujo principal

1. El miembro accede a su perfil.
2. Selecciona “Quiero ser Ambassador”.
3. Completa bio, especialidades, experiencia e intereses.
4. Indica disponibilidad si aplica.
5. Envía solicitud.
6. Staff/Admin revisa.
7. Staff/Admin aprueba o rechaza.
8. Si aprueba, el sistema asigna rol Ambassador.

### Resultado

Miembro convertido en Ambassador activo o solicitud rechazada.

---

## UC-003 - Solicitar una sesión

**Actor principal:** Profesor / Centro / Orientador / Empresa / Miembro  
**Objetivo:** Solicitar una sesión de Tech Riders.

### Flujo principal

1. El actor accede a “Solicitar sesión”.
2. Indica tipo de sesión.
3. Indica temática, audiencia, fecha aproximada y modalidad.
4. Añade contexto y objetivos.
5. Envía solicitud.
6. El sistema registra solicitud.
7. Staff revisa y asigna Ambassador si procede.
8. Se confirma sala o enlace.
9. Se notifica a las partes.

### Resultado

Sesión solicitada, revisada y planificable.

---

## UC-004 - Gestionar disponibilidad Ambassador

**Actor principal:** Ambassador  
**Objetivo:** Indicar disponibilidad para participar en sesiones.

### Flujo principal

1. El Ambassador accede a su portal.
2. Abre calendario de disponibilidad.
3. Marca franjas disponibles.
4. Guarda cambios.
5. El sistema usa esa disponibilidad para asignaciones.

### Resultado

Disponibilidad actualizada.

---

## UC-005 - Asignar Ambassador a sesión

**Actor principal:** Staff / Administrador  
**Objetivo:** Coordinar una sesión con la persona adecuada.

### Flujo principal

1. Staff consulta sesiones pendientes.
2. Abre detalle.
3. Consulta Ambassadors por temática, perfil y disponibilidad.
4. Selecciona Ambassador.
5. El sistema notifica propuesta.
6. Ambassador acepta o rechaza.
7. Se actualiza estado.

### Resultado

Sesión asignada o pendiente de nueva asignación.

---

## UC-006 - Proponer sesión

**Actor principal:** Ambassador / Community Leaders  
**Objetivo:** Proponer una nueva actividad para la comunidad.

### Flujo principal

1. El actor accede a “Proponer sesión”.
2. Introduce título, descripción, objetivos, temática, nivel y modalidad.
3. Envía propuesta.
4. Staff revisa.
5. Staff aprueba, ajusta o rechaza.
6. Si aprueba, se programa y publica.

### Resultado

Sesión propuesta y gestionada.

---

## UC-007 - Call for Sessions

**Actor principal:** Staff  
**Objetivo:** Abrir una convocatoria para que Ambassadors participen en una actividad.

### Flujo principal

1. Staff crea convocatoria.
2. Define temática, evento asociado, fechas y condiciones.
3. Publica convocatoria.
4. Ambassadors postulan.
5. Staff revisa postulaciones.
6. Staff selecciona participantes.
7. El sistema notifica resultado.

### Resultado

Ambassadors seleccionados para una actividad.

---

## UC-008 - Publicar contenido de comunidad

**Actor principal:** Staff / Community Leaders / Ambassador con permiso  
**Objetivo:** Alimentar el banco de conocimiento de Tech Riders.

### Flujo principal

1. El actor crea contenido.
2. Selecciona tipo: artículo, tutorial, talk, podcast o recurso.
3. Asigna temática, nivel y visibilidad.
4. Guarda como borrador.
5. Staff/editor valida.
6. Se publica.

### Resultado

Contenido disponible en la plataforma.

---

# 17. Reglas de negocio

## 17.1 Membresía

- Toda persona registrada en la comunidad tendrá el rol base **Miembro Tech Riders**.
- Ser miembro no implica ser Ambassador, Staff o Community Leaders.
- Un miembro puede solicitar evolucionar a Ambassador o Community Leaders.
- Staff puede asignar roles de comunidad.
- El sistema debe permitir multi-rol.

## 17.2 Staff

- Staff puede gestionar miembros, roles, sesiones, eventos y contenido.
- Staff valida solicitudes activas de Ambassador y Community Leaders.
- Staff mantiene la coherencia funcional de la comunidad.

## 17.3 Community Leaders

- Un Community Leaders puede apoyar iniciativas sin tener todos los permisos de Staff.
- Un Community Leaders puede proponer sesiones y contenido.
- Sus permisos dependerán de la confianza/ámbito asignado.

## 17.4 Ambassadors

- Un Ambassador debe ser miembro Tech Riders.
- Un Ambassador puede ser profesional, alumno, profesor, empresa o cualquier persona que participe activamente.
- Un Ambassador puede impartir sesiones, participar en podcasts, formaciones, mentorías u otras actividades.
- La asignación a sesiones debe quedar trazada.
- El histórico de participación debe mantenerse.

## 17.5 Sesiones

- Toda sesión debe tener tipo, temática, audiencia, modalidad y estado.
- Una sesión puede ser solicitada por centros, profesores, orientadores, empresas o miembros.
- Una sesión puede tener uno o varios Ambassadors.
- Una sesión realizada debe pasar a histórico.
- El término funcional oficial será **sesión**, no “charla”.

## 17.6 Eventos y actividades

- Un evento puede contener varias sesiones.
- Una formación puede tratarse como actividad programada.
- Las actividades pueden ser públicas o privadas.
- Las inscripciones deben evitar duplicados.

---

# 18. MVP y roadmap de releases

## 18.1 Release 1.0 - MVP Comunidad Tech Riders

### Objetivo

Tener una plataforma mínima funcional centrada en Tech Riders, sus miembros, ambassadors y sesiones.

### Incluye

- Home pública Tech Riders.
- Quiénes somos.
- Únete a Tech Riders.
- Login social.
- Alta miembro.
- Perfil miembro.
- Roles comunidad: Miembro, Staff, Community Leaders, Ambassador.
- Solicitud para ser Ambassador.
- Solicitud de sesión.
- Gestión básica de sesiones.
- Asignación manual de Ambassadors.
- Portal Ambassador básico.
- Portal Staff básico.
- Calendario de actividades.
- Integración GPF inicial.
- Integración SharePoint Lists.
- Notificaciones email básicas.

## 18.2 Release 1.1 - Comunidad y contenido

- Banco de conocimiento.
- Blog.
- Tutoriales.
- Talks y podcasts.
- Propuesta de sesiones por Ambassadors/Community Leaders.
- Call for Sessions.
- Perfiles públicos o internos de Ambassadors.

## 18.3 Release 1.2 - Centros, empresas y orientación

- Portal centros.
- Portal empresas.
- Orientación tech.
- Perfiles profesionales tech.
- Ofertas y oportunidades.
- Woman Tech.

## 18.4 Release 1.3 - Multi-centro y reporting

- Administración por centro.
- Métricas de comunidad.
- Métricas de sesiones.
- Exportación.
- Auditoría avanzada.

## 18.5 Release 2.0 - Inteligencia y gamificación

- Matching asistido Ambassador-sesión.
- Recomendaciones de contenido y actividades.
- Gamificación.
- Insignias.
- Niveles de participación.
- Reconocimiento comunitario.

---

# 19. Backlog funcional inicial

## 19.1 P0 - Imprescindible MVP

| ID | Item | Descripción | Prioridad |
|---|---|---|---|
| BF-001 | Home Tech Riders | Página pública centrada en comunidad | Alta |
| BF-002 | Quiénes somos | Sección pública de identidad, propósito y valores | Alta |
| BF-003 | Alta miembro | Registro como miembro Tech Riders | Alta |
| BF-004 | Roles comunidad | Miembro, Staff, Community Leaders, Ambassador | Alta |
| BF-005 | Perfil miembro | Gestión de datos, intereses y perfil funcional | Alta |
| BF-006 | Solicitud Ambassador | Flujo para solicitar participación activa | Alta |
| BF-007 | Solicitud sesión | Formulario y workflow de solicitud | Alta |
| BF-008 | Gestión sesiones | Estados, asignación y seguimiento | Alta |
| BF-009 | Portal Ambassador | Perfil, disponibilidad y sesiones asignadas | Alta |
| BF-010 | Portal Staff | Gestión básica de miembros y sesiones | Alta |
| BF-011 | Calendario actividades | Visibilidad de sesiones/eventos/formaciones | Alta |
| BF-012 | Notificaciones básicas | Emails por altas, solicitudes y asignaciones | Alta |

## 19.2 P1 - Alto valor post-MVP

| ID | Item | Descripción | Prioridad |
|---|---|---|---|
| BF-013 | Call for Sessions | Convocatorias abiertas para actividades | Alta |
| BF-014 | Banco conocimiento | Blog, tutoriales, talks, podcasts | Alta |
| BF-015 | Propuesta sesiones | Ambassadors y Community Leaders proponen sesiones | Alta |
| BF-016 | Portal Community Leaders | Espacio para apoyar iniciativas | Media |
| BF-017 | Portal Centros | Solicitud y seguimiento de sesiones | Media |
| BF-018 | Portal Empresas | Propuestas y colaboración | Media |

## 19.3 P2 - Evolutivo

| ID | Item | Descripción | Prioridad |
|---|---|---|---|
| BF-019 | Matching asistido | Recomendación Ambassador-sesión | Media |
| BF-020 | Reporting | Dashboards de actividad e impacto | Media |
| BF-021 | Gamificación | Badges, niveles y reconocimiento | Baja |
| BF-022 | Recomendaciones IA | Contenido y actividades personalizadas | Baja |

---

# 20. Épicas, features e historias de usuario

## EPIC-01 - Identidad Tech Riders

### US-001 - Ver quiénes somos

Como visitante, quiero entender qué es Tech Riders, cuál es su propósito y cómo puedo participar.

**Criterios de aceptación**

- Existe una página “Quiénes somos”.
- Explica misión, visión y valores.
- Explica qué hacemos.
- Explica qué significa ser miembro.
- Explica Staff, Community Leaders y Ambassadors.

### US-002 - Unirme como Member

Como visitante, quiero unirme a Tech Riders para formar parte de la comunidad.

**Criterios de aceptación**

- Existe CTA “Únete a Tech Riders”.
- Se puede iniciar sesión con proveedor social.
- Se completa formulario básico.
- Se asigna rol MEber.
- Se confirma el alta.

---

## EPIC-02 - Comunidad y roles

### US-003 - Gestionar roles de comunidad

Como Staff, quiero asignar roles de comunidad para organizar la participación de miembros.

**Criterios de aceptación**

- Se pueden asignar roles Member, Staff, Community Leaders y Ambassador.
- Un miembro puede tener varios roles.
- Los cambios quedan auditados.

### US-004 - Solicitar ser Ambassador

Como miembro, quiero solicitar ser Ambassador para participar activamente en actividades de Tech Riders.

**Criterios de aceptación**

- Existe formulario de solicitud.
- Se capturan especialidades, bio y disponibilidad.
- Staff puede aprobar o rechazar.
- Si aprueba, se asigna rol Ambassador.

---

## EPIC-03 - Sesiones Tech Riders

### US-005 - Solicitar sesión

Como profesor, centro, orientador, empresa o miembro, quiero solicitar una sesión para una audiencia concreta.

**Criterios de aceptación**

- El formulario usa el término sesión.
- Captura tipo, temática, audiencia, modalidad y fecha aproximada.
- La solicitud queda registrada.
- Staff puede revisarla.

### US-006 - Asignar Ambassador a sesión

Como Staff, quiero asignar uno o varios Ambassadors a una sesión para coordinar la actividad.

**Criterios de aceptación**

- Staff ve sesiones pendientes.
- Puede seleccionar Ambassadors.
- Se notifica la asignación.
- El Ambassador puede aceptar o rechazar.

### US-007 - Gestionar mis sesiones como Ambassador

Como Ambassador, quiero ver mis sesiones asignadas para prepararlas y confirmar participación.

**Criterios de aceptación**

- El Ambassador ve sesiones pendientes y confirmadas.
- Puede aceptar o rechazar propuestas.
- Puede consultar detalles.
- El histórico queda registrado.

---

## EPIC-04 - Call for Sessions

### US-008 - Crear Call for Sessions

Como Staff, quiero abrir convocatorias para que Ambassadors propongan o participen en sesiones.

**Criterios de aceptación**

- Se puede crear convocatoria.
- Tiene fechas, temática y descripción.
- Se publica para Ambassadors.

### US-009 - Postularme a Call for Sessions

Como Ambassador, quiero postularme a una convocatoria para participar en una actividad.

**Criterios de aceptación**

- Ve convocatorias abiertas.
- Puede enviar propuesta.
- Staff puede seleccionar o rechazar.
- Se notifica resultado.

---

## EPIC-05 - Contenido y conocimiento

### US-010 - Consultar banco de conocimiento

Como miembro o visitante, quiero consultar recursos de Tech Riders para aprender y descubrir contenido de comunidad.

**Criterios de aceptación**

- Hay listado de contenidos.
- Se puede filtrar por temática, tipo y nivel.
- Respeta visibilidad pública o privada.

### US-011 - Publicar contenido

Como Staff, Community Leaders o Ambassador con permiso, quiero publicar contenido para alimentar la comunidad.

**Criterios de aceptación**

- Puede crear borrador.
- Puede asignar temática y tipo.
- Puede enviarlo a revisión.
- Staff/editor puede publicarlo.

---

## EPIC-06 - Administración y reporting

### US-012 - Ver métricas de comunidad

Como Staff, quiero ver métricas de actividad para entender cómo crece e impacta Tech Riders.

**Criterios de aceptación**

- Muestra miembros por rol.
- Muestra sesiones solicitadas, confirmadas y realizadas.
- Muestra actividad por temática.
- Permite filtrar por periodo.

---

# 21. Requisitos no funcionales

## 21.1 Rendimiento

- La parte pública debe cargar rápido.
- El calendario debe ser fluido.
- Los filtros deben responder correctamente.
- El banco de conocimiento debe permitir búsqueda eficiente.

## 21.2 Escalabilidad

- Debe permitir nuevos roles de comunidad.
- Debe permitir nuevos tipos de sesión.
- Debe permitir nuevos centros y empresas.
- Debe permitir nuevas comunidades o partners.

## 21.3 Accesibilidad

- Navegación accesible.
- Formularios claros.
- Contrastes adecuados.
- Vídeos con subtítulos cuando sea posible.

## 21.4 Mantenibilidad

- Separar dominio de miembros, sesiones, contenido, eventos e integraciones.
- Evitar acoplar la lógica de comunidad a una sola fuente de datos.
- Centralizar reglas de negocio.

---

# 22. Seguridad, RGPD y auditoría

## 22.1 Seguridad

- Autenticación mediante proveedores sociales.
- Autorización por permisos.
- Control de acceso por rol de comunidad.
- Validación de formularios.
- Protección de datos personales.

## 22.2 RGPD

Debe contemplarse:

- Consentimiento de tratamiento de datos.
- Finalidad del tratamiento.
- Solicitud de baja.
- Control de visibilidad de perfil.
- Especial cuidado con estudiantes o menores si aplica.

## 22.3 Auditoría

Deben auditarse:

- Altas de miembros.
- Cambios de rol.
- Aprobaciones de Ambassador.
- Asignaciones de sesiones.
- Cambios de estado de sesión.
- Publicación de contenido.
- Exportación de informes.

---

# 23. Métricas y reporting

## 23.1 Métricas de comunidad

- Miembros registrados.
- Miembros activos.
- Ambassadors activos.
- Community Leaders activos.
- Staff.
- Nuevas altas por periodo.

## 23.2 Métricas de sesiones

- Sesiones solicitadas.
- Sesiones propuestas.
- Sesiones confirmadas.
- Sesiones realizadas.
- Sesiones canceladas.
- Sesiones por temática.
- Sesiones por modalidad.
- Sesiones por audiencia.

## 23.3 Métricas de participación

- Inscripciones a actividades.
- Participación de Ambassadors.
- Participación de Community Leaders.
- Contenidos publicados.
- Visualizaciones de contenido.

## 23.4 Nota sobre reconocimiento

El reconocimiento de Ambassadors debe plantearse como visibilidad positiva de comunidad, no como evaluación descontextualizada de personas. Si se usan rankings, deben ser configurables, transparentes y preferiblemente internos en fases iniciales.

---

# 24. Riesgos, dependencias y decisiones pendientes

## 24.1 Riesgos

| Riesgo | Impacto | Mitigación |
|---|---|---|
| Que la plataforma parezca de Tajamar y no de Tech Riders | Alto | Reforzar identidad Tech Riders en naming, home y navegación |
| Confundir Tech Riders con solo speakers | Alto | Separar Miembro, Staff, Community Leaders y Ambassador |
| Usar “charlas” como concepto limitado | Medio | Usar “sesiones” en todo el producto |
| Multi-rol mal modelado | Alto | Separar rol comunidad, perfil funcional y permisos |
| Gestión manual excesiva | Medio | Workflows, estados y notificaciones |
| Dependencia de GPF/SharePoint | Alto | Definir contratos de integración |

## 24.2 Decisiones pendientes

| ID | Decisión | Recomendación |
|---|---|---|
| DP-001 | ¿Nombre final del producto? | Tech Riders |
| DP-002 | ¿Tajamar aparece en naming principal? | No como foco principal; sí como origen/nodo/partner |
| DP-003 | ¿Término oficial para actividades tipo charla? | Sesiones |
| DP-004 | ¿Tech Riders equivale a speakers? | No, Tech Riders son todos los miembros |
| DP-005 | ¿Ambassador es rol activo? | Sí, para personas que participan activamente y extienden comunidad |
| DP-006 | ¿Staff y Community Leaders son roles de comunidad? | Sí |
| DP-007 | ¿Multi-rol desde MVP? | Sí |
| DP-008 | ¿Matching automático en MVP? | No, manual/asistido después |

---

## Conclusión ejecutiva

La plataforma debe reposicionarse claramente como **Plataforma Web Tech Riders**. El centro no es Tajamar como institución, ni las charlas como actividad aislada, sino Tech Riders como comunidad viva.

El producto debe permitir que cualquier persona se una como miembro, que Staff y Community Leaders den forma y continuidad a la comunidad, y que Ambassadors participen activamente en sesiones, eventos, podcasts, formaciones, mentorías y actividades que ayudan a construir y extender Tech Riders.

---

# 26. Anexo A - Propuesta de estructura GitHub Projects

## 26.1 Milestones sugeridos

| Milestone | Objetivo |
|---|---|
| v1.0.0 - MVP Comunidad Tech Riders | Home, quiénes somos, miembros, roles comunidad, sesiones, Ambassadors y Staff básico |
| v1.1.0 - Contenido y Call for Sessions | Banco conocimiento, contenidos, propuesta sesiones y convocatorias |
| v1.2.0 - Centros, Empresas y Orientación | Portales de centros, empresas, orientación y Woman Tech |
| v1.3.0 - Reporting y Multi-centro | Métricas, dashboards, administración por centro |
| v2.0.0 - Inteligencia y Gamificación | Matching asistido, recomendaciones y reconocimiento comunitario |

## 26.2 Labels sugeridas

### Tipo

- type: epic
- type: feature
- type: user-story
- type: task
- type: bug
- type: spike
- type: decision

### Área funcional

- area: public-web
- area: identity
- area: community
- area: members
- area: staff
- area: community-leaders
- area: ambassadors
- area: sessions
- area: call-for-sessions
- area: events
- area: trainings
- area: content
- area: centers
- area: companies
- area: woman-tech
- area: orientation
- area: admin
- area: reporting
- area: integrations
- area: security

### Prioridad

- priority: critical
- priority: high
- priority: medium
- priority: low

### Release

- release: 1.0.0
- release: 1.1.0
- release: 1.2.0
- release: 1.3.0
- release: 2.0.0

## 26.3 Campos personalizados recomendados

| Campo | Tipo | Uso |
|---|---|---|
| Target Release | Texto o single select | Versión objetivo |
| Functional Area | Single select | Área principal |
| Community Role | Single select | Meber, Staff, Community Leaders, Ambassador |
| Priority | Single select | Prioridad |
| Effort | Number / size | Estimación relativa |
| Owner | Person | Responsable |
| Dependency | Text | Dependencias |
| Ready for Dev | Checkbox | Cumple Definition of Ready |
| MVP | Checkbox | Entra en MVP |

## 26.4 Vistas sugeridas

### Vista MVP Comunidad

```text
is:issue is:open milestone:"v1.0.0 - MVP Comunidad Tech Riders"
```

Agrupación recomendada:

- Status.
- Functional Area.

### Vista Sesiones

```text
is:issue is:open label:"area: sessions"
```

### Vista Ambassadors

```text
is:issue is:open label:"area: ambassadors"
```

### Vista Comunidad

```text
is:issue is:open label:"area: community"
```

### Vista Decisiones

```text
is:issue is:open label:"type: decision"
```

---

# 27. Anexo B - Definition of Ready y Definition of Done

## 27.1 Definition of Ready

Una issue está lista para desarrollo cuando:

- Tiene descripción funcional clara.
- Usa terminología correcta de Tech Riders.
- Indica si afecta a miembros, Staff, Community Leaders o Ambassadors.
- Tiene criterios de aceptación.
- Tiene prioridad.
- Tiene release objetivo.
- Tiene dependencias identificadas.
- Tiene reglas de negocio conocidas.
- Tiene permisos definidos.
- Tiene origen de datos definido.

## 27.2 Definition of Done

Una issue se considera terminada cuando:

- Está implementada.
- Cumple criterios de aceptación.
- Usa “sesiones” y no “charlas” donde corresponda.
- Respeta roles comunidad.
- Respeta permisos.
- Gestiona errores principales.
- Está integrada con la fuente de datos correspondiente.
- Está revisada.
- Está documentada si aplica.
- No rompe flujos existentes.

---

# Cierre

Este documento queda ajustado para que la plataforma tenga como centro real a **Tech Riders**, entendida como comunidad abierta de miembros, Staff, Community Leaders y Ambassadors.

La siguiente evolución natural es convertir este documento en issues de GitHub, separando épicas, features, historias y decisiones, manteniendo siempre la terminología corregida:

- Tech Riders = comunidad completa.
- Miembro Tech Riders = cualquier persona que se une.
- Staff / Community Leaders = miembros que dan forma y operan la comunidad.
- Ambassador = persona activa que participa y ayuda a construir y extender comunidad.
- Sesiones = término oficial para actividades antes llamadas charlas.
