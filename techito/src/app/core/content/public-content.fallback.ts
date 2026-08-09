import { PublicContentPayload } from './public-content.models';

export const PUBLIC_CONTENT_FALLBACK: PublicContentPayload = {
  home: {
    stats: [
      { value: '13', label: 'Años de comunidad', icon: '📅' },
      { value: '1300+', label: 'Tutoriales publicados', icon: '📚' },
      { value: '50+', label: 'Centros inscritos #FPTOUR', icon: '🏫' },
      { value: '1500+', label: 'Alumnos #FPTOUR', icon: '👥' },
      { value: '80+', label: 'Sesiones #FPTOUR', icon: '🎤' },
      { value: '67', label: 'Sesiones en Tajamar Tech', icon: '🎤' },
      { value: '20+', label: 'Colaboraciones realizadas con otras comunidades', icon: '🫂' },
      { value: '5', label: 'Eventos propios', icon: '🎉' },
    ],
    profilePanelCards: [
      { title: 'Docentes', description: 'Impulsa el talento tecnológico de tu alumnado. Comparte conocimiento y accede a recursos para el aula.', icon: '🎓', cta: 'Explorar', link: '/centers', accent: 'violet' },
      { title: 'Estudiantes', description: 'Aprende tecnologías, descubre formación y accede a oportunidades para desarrollar tu futuro.', icon: '🧑‍💻', cta: 'Explorar', link: '/orienta-tech', accent: 'cyan' },
      { title: 'Profesionales', description: 'Impulsa tu carrera en tecnología, comparte experiencia y amplía tu red de contactos.', icon: '💼', cta: 'Explorar', link: '/events', accent: 'blue' },
      { title: 'Empresas', description: 'Conecta con el talento, participa en eventos y comparte conocimiento real con la comunidad.', icon: '🏢', cta: 'Explorar', link: '/companies', accent: 'amber' },
      { title: 'Orientadores', description: 'Accede a recursos y actividades tecnológicas para tu alumnado y descubre iniciativas STEM.', icon: '🧭', cta: 'Explorar', link: '/orienta-tech', accent: 'pink' },
      { title: 'Starters', description: 'Descubre profesiones, formaciones y tus primeros pasos en el mundo tech. No necesitas experiencia.', icon: '🚀', cta: 'Explorar', link: '/tutorials', accent: 'teal' },
      { title: 'Women in Tech', description: 'Referentes, ayudas, comunidad y oportunidades para mujeres que quieren crecer en tecnología.', icon: '♀️', cta: 'Explorar', link: '/woman-tech', accent: 'fuchsia' },
      { title: 'Conócenos', description: 'Descubre quiénes somos, nuestra misión, valores y cómo trabajamos para impulsar el talento tech.', icon: '👥', cta: 'Explorar', link: '/about-us', accent: 'sky' },
    ],
    pastEventPhotos: [
      { src: 'assets/techito_salero_ming.jpg', alt: 'Talk de TechRiders', label: 'Talks' },
      { src: 'assets/techito_karmela.jpg', alt: 'Encuentro #FPTour', label: '#FPTour' },
      { src: 'assets/techito_bici_tajamar.jpg', alt: 'Evento en Tajamar Tech', label: 'Tajamar Tech' },
      { src: 'assets/techito_piscineo.jpg', alt: 'Evento externo de comunidad', label: 'Eventos externos' },
    ],
  },
  events: {
    participationModes: [
      { title: 'Asistir', detail: 'Reserva plaza en próximos encuentros y participa en sesiones prácticas.' },
      { title: 'Ponente', detail: 'Comparte una charla técnica o una experiencia real en formato comunidad.' },
      { title: 'Colaborar', detail: 'Activa alianzas entre centros, empresas y perfiles técnicos de Tech Riders.' },
    ],
    galleryGroups: [
      {
        title: 'Talks',
        subtitle: 'Charlas y encuentros de la comunidad técnica.',
        items: [
          { src: 'assets/techito_salero_ming.jpg', alt: 'Talk en evento TechRiders' },
          { src: 'assets/techito_salero_ming.jpg', alt: 'Comunidad participando en una charla' },
          { src: 'assets/techito_salero_ming.jpg', alt: 'Ponencia técnica en TechRiders' },
        ],
      },
      {
        title: '#FPTour',
        subtitle: 'Meetups y sesiones en centros de formación.',
        items: [
          { src: 'assets/techito_karmela.jpg', alt: 'Evento #FPTour en aula' },
          { src: 'assets/techito_karmela.jpg', alt: 'Networking durante #FPTour' },
          { src: 'assets/techito_karmela.jpg', alt: 'Participantes de #FPTour' },
        ],
      },
      {
        title: 'Eventos externos',
        subtitle: 'Conferencias, webinars y colaboraciones con otras comunidades.',
        items: [
          { src: 'assets/techito_piscineo.jpg', alt: 'Evento externo de la comunidad TechRiders' },
          { src: 'assets/techito_bici_tajamar.jpg', alt: 'Conferencia y networking de TechRiders' },
          { src: 'assets/techito_piscineo.jpg', alt: 'Participación de TechRiders en evento externo' },
        ],
      },
    ],
    talksFallback: [
      { title: 'Comunidad, aprendizaje y cerrar ciclos: Tech Riders Talks | Salero de Ming', src: 'https://www.youtube-nocookie.com/embed/YekC-fVM3Ig' },
      { title: 'Liderazgo técnico, comunidad y crecimiento profesional | Sergio Hernández', src: 'https://www.youtube-nocookie.com/embed/NHkw3rh1BO8' },
      { title: 'IA, liderazgo y comunidad: experiencia sin filtros | Javier Pallo', src: 'https://www.youtube-nocookie.com/embed/qJUUlvvH3_g' },
      { title: 'Ciberseguridad real: pentesting, red team y LockShields | Marco Carrasco', src: 'https://www.youtube-nocookie.com/embed/IOi91LjE0m4' },
      { title: 'De junior a senior: claves reales para crecer en tecnología | María & Elías', src: 'https://www.youtube-nocookie.com/embed/o6bGKi8y2eY' },
    ],
  },
  centers: {
    metrics: [
      { icon: '🏫', value: 'Centros', label: 'Red educativa abierta' },
      { icon: '🗺️', value: 'Multi-zona', label: 'Cobertura territorial' },
      { icon: '🎓', value: 'FP + Grado', label: 'Perfiles formativos' },
    ],
    cards: [
      { icon: '📍', title: 'Dónde estudiar', description: 'Explora centros por zona y descubre rutas formativas orientadas a perfiles tech.', points: ['Búsqueda por provincia', 'Ficha de centro', 'Programas destacados'] },
      { icon: '🧑‍🏫', title: 'Sesiones para aulas', description: 'Solicita sesiones para estudiantes con foco en empleabilidad, especialización y realidad sectorial.', points: ['Formato presencial/online', 'Temáticas por nivel', 'Coordinación con Staff'] },
      { icon: '🤝', title: 'Colaboración educativa', description: 'Conecta con la comunidad para actividades, talleres y propuestas conjuntas de alto impacto.', points: ['Diseño de iniciativas', 'Calendario coordinado', 'Seguimiento y continuidad'] },
    ],
  },
  companies: {
    valueCards: [
      { icon: '🎤', title: 'Propón una sesión', description: 'Comparte casos reales y aprendizajes prácticos desde tu organización.', points: ['Formato adaptable', 'Audiencias concretas', 'Coordinación operativa'] },
      { icon: '🧠', title: 'Participa en actividades', description: 'Impulsa workshops, retos y formatos de comunidad con impacto formativo.', points: ['Co-creación con Tech Riders', 'Visibilidad de marca técnica', 'Continuidad anual'] },
      { icon: '🚀', title: 'Conecta con talento', description: 'Activa itinerarios para detectar perfiles junior y senior alineados con tu stack.', points: ['Perfiles filtrados', 'Canales de contacto', 'Seguimiento de pipeline'] },
    ],
    processCards: [
      { title: 'Definición de colaboración', detail: 'Identificamos objetivo, formato y audiencia de la iniciativa.', progress: 100, status: 'Paso 1' },
      { title: 'Planificación y calendario', detail: 'Alineamos fechas, recursos y coordinación con la comunidad.', progress: 100, status: 'Paso 2' },
      { title: 'Ejecución y seguimiento', detail: 'Publicamos, ejecutamos y medimos resultados para repetir impacto.', progress: 100, status: 'Paso 3' },
    ],
  },
  opportunities: {
    tracks: [
      { title: 'Primer empleo tech', detail: 'Rutas para perfiles junior con foco en transición real al mercado.', progress: 78, status: 'Junior', ctaLabel: 'Ver guía', ctaLink: '/orienta-tech' },
      { title: 'Upskilling profesional', detail: 'Sesiones y recursos para evolución de perfil técnico y liderazgo.', progress: 65, status: 'Profesional', ctaLabel: 'Explorar recursos', ctaLink: '/tutorials' },
      { title: 'Conexión con empresas', detail: 'Canales de colaboración, sesiones y oportunidades compartidas con partners.', progress: 71, status: 'Empresa', ctaLabel: 'Ir a empresas', ctaLink: '/companies' },
    ],
    resources: [
      { mode: 'Comunidad', title: 'Banco de conocimiento Tech Riders', summary: 'Tutoriales, charlas y materiales prácticos para aprendizaje continuo.', tags: ['Tutoriales', 'Recursos', 'Aprendizaje'], meta: 'Actualización continua', ctaLabel: 'Ir a conocimiento', ctaLink: '/tutorials' },
      { mode: 'Actividad', title: 'Próximas sesiones y actividades', summary: 'Agenda pública con oportunidades para participar y hacer networking.', tags: ['Eventos', 'Sesiones', 'Networking'], meta: 'Calendario abierto', ctaLabel: 'Ver calendario', ctaLink: '/calendar' },
    ],
  },
  womanTech: {
    metrics: [
      { icon: '💜', value: 'Woman Tech', label: 'Línea de comunidad' },
      { icon: '🎙️', value: 'Sesiones', label: 'Referentes y experiencias' },
      { icon: '🤝', value: 'Red', label: 'Acompañamiento y visibilidad' },
    ],
    journey: [
      { step: '01', title: 'Inspiración', text: 'Historias y trayectorias de mujeres en tecnología con contexto real.' },
      { step: '02', title: 'Aprendizaje', text: 'Sesiones técnicas y recursos para fortalecer habilidades y confianza.' },
      { step: '03', title: 'Conexión', text: 'Vínculo con comunidad, redes profesionales y nuevas oportunidades.' },
    ],
  },
  join: {
    metrics: [
      { value: '13', label: 'Años de comunidad', icon: '📅' },
      { value: '1300+', label: 'Recursos compartidos', icon: '📚' },
      { value: '80+', label: 'Sesiones #FPTOUR', icon: '🎤' },
      { value: '1500+', label: 'Alumnos impactados', icon: '👥' },
    ],
    intakeOptions: [
      { label: 'Quiero unirme como miembro', value: 'member' },
      { label: 'Quiero solicitar ser Ambassador', value: 'ambassador' },
      { label: 'Quiero solicitar una sesión', value: 'session' },
    ],
  },
  orientaTech: {
    metrics: [
      { icon: '📚', value: '4', label: 'Programas base' },
      { icon: '💼', value: '20+', label: 'Empresas conectadas' },
      { icon: '🎯', value: '1:1', label: 'Mentoring personalizado' },
      { icon: '🚀', value: '360°', label: 'Orientación de carrera' },
    ],
    coreFeatures: [
      { icon: '📚', title: 'Formaciones regladas', description: 'Programas formales para iniciar o transformar tu carrera tecnológica.', points: ['Ciclos formativos', 'Bootcamps certificados', 'Rutas guiadas'] },
      { icon: '💼', title: 'Empleo Tech', description: 'Conexión con empresas reales que contratan talento junior y en transición.', points: ['Ofertas curadas', 'Prácticas', 'Networking de hiring'] },
      { icon: '🎯', title: 'Mentoría personalizada', description: 'Acompañamiento por profesionales en activo para acelerar tu evolución.', points: ['Mentor asignado', 'Sesiones periódicas', 'Seguimiento de objetivos'] },
      { icon: '🚀', title: 'Orientación estratégica', description: 'Plan de carrera con objetivos accionables y revisión continua.', points: ['Especialización', 'Roadmap', 'Revisión trimestral'] },
    ],
    participationTracks: [
      { title: 'Empresas colaboradoras', status: 'Activa', progress: 78, detail: 'Red de organizaciones que abren oportunidades reales de empleabilidad.', ctaLabel: 'Ver oportunidades', ctaLink: '/join' },
      { title: 'Recruiters y RRHH', status: 'Activa', progress: 74, detail: 'Sesiones de mercado laboral, procesos de selección y feedback estructurado.', ctaLabel: 'Participar', ctaLink: '/join' },
      { title: 'Recursos y contenidos', status: 'En crecimiento', progress: 69, detail: 'Videoteca, guías y casos para crecer en soft skills y carrera profesional.', ctaLabel: 'Explorar', ctaLink: '/tutorials' },
    ],
    studySections: [
      { icon: '', title: 'FP', description: 'Itinerarios base en desarrollo, sistemas, data y ciberseguridad para iniciar carrera tech.', points: ['SMR', 'ASIR', 'DAW', 'DAM'] },
      { icon: '', title: 'Másteres FP', description: 'Especialización en áreas con alta demanda y enfoque práctico para empleabilidad.', points: ['Big Data', 'Ciberseguridad', 'Cloud', 'IA aplicada'] },
      { icon: '', title: 'Certificados', description: 'Rutas cortas para validar competencias y acelerar inserción laboral.', points: ['Qué son', 'Cuándo elegirlos', 'FAQ'] },
      { icon: '', title: 'Grados y Másteres', description: 'Opciones universitarias orientadas a perfiles técnicos y de especialización avanzada.', points: ['Grados base', 'Másteres de especialidad', 'Comparativa por perfil'] },
      { icon: '', title: 'Certificaciones', description: 'Credenciales por fabricante para reforzar tu perfil profesional.', points: ['Ruta por proveedor', 'Nivel recomendado', 'Preparación guiada'] },
    ],
  },
  about: {
    metrics: [
      { icon: '🧭', value: '4', label: 'Líneas de comunidad' },
      { icon: '👥', value: '1500+', label: 'Participantes' },
      { icon: '🤝', value: '20+', label: 'Colaboraciones' },
      { icon: '🎤', value: '80+', label: 'Sesiones' },
    ],
    socialLinks: [
      { platform: 'linkedin', href: 'https://www.linkedin.com' },
      { platform: 'github', href: 'https://github.com' },
      { platform: 'x', href: 'https://x.com' },
      { platform: 'instagram', href: 'https://www.instagram.com' },
      { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
    ],
    teamZones: [
      {
        key: 'staff',
        title: 'Staff',
        description: 'Personas que lideran y coordinan la comunidad.',
        members: [
          { name: 'Sergio Hierro', role: 'Founder & Community Lead', photo: 'assets/staff/sergio-hierro.png', photoAlt: 'Foto de Sergio Hierro', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
          { name: 'Juan Bou', role: 'Program Coordinator', photo: 'assets/staff/Juan Bou.jpg', photoAlt: 'Foto de Juan Bou', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
          { name: 'Diego Zapico', role: 'Learning Initiatives', photo: 'assets/staff/diego-zapico.png', photoAlt: 'Foto de Diego Zapico', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
          { name: 'Ana Pereira', role: 'Operations & Community Programs', photo: 'assets/staff/ana-pereira.jpg', photoAlt: 'Foto de Ana Pereira', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
          { name: 'Borja Piris', role: 'Engineering Mentor', photo: 'assets/staff/borja-piris.jpg', photoAlt: 'Foto de Borja Piris', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
        ],
      },
      {
        key: 'community-leaders',
        title: 'Community Leaders',
        description: 'Personas que ayudan a dar forma y operar iniciativas de Tech Riders.',
        members: [
          { name: 'Mónica Delgado', role: 'Community Leader', photo: 'assets/community-leaders/Mónica Delgado.jpg', photoAlt: 'Foto de Mónica Delgado', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
          { name: 'Rodrigo Liberoff', role: 'Community Leader', photo: 'assets/community-leaders/Rodrigo Liberoff.jpg', photoAlt: 'Foto de Rodrigo Liberoff', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
        ],
      },
      {
        key: 'ambassador',
        title: 'Ambassador',
        description: 'Personas que participan activamente en actividades y ayudan a extender comunidad.',
        members: [
          { name: 'María Reina', role: 'Ambassador · Community Speaker', photo: 'assets/ambassadors/María Reina.jpg', photoAlt: 'Foto de María Reina', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
          { name: 'Estefany Duran', role: 'Ambassador · Career Talks', photo: 'assets/ambassadors/Estefany Duran.jpg', photoAlt: 'Foto de Estefany Duran', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
          { name: 'Celeste Sánchez', role: 'Ambassador · Learning Sessions', photo: 'assets/ambassadors/Celeste Sánchez.jpg', photoAlt: 'Foto de Celeste Sánchez', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
        ],
      },
      {
        key: 'member',
        title: 'Member',
        description: 'Personas que se unen y participan en sesiones, actividades y comunidad.',
        members: [
          { name: 'Marta Moreno', role: 'Member · Frontend Developer', photo: 'assets/member/Marta Moreno.png', photoAlt: 'Foto de Marta Moreno', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
          { name: 'Macarena Mamolar', role: 'Member · Product Design', photo: 'assets/member/Macarena Mamolar.jpg', photoAlt: 'Foto de Macarena Mamolar', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
          { name: 'Jorge Rodríguez', role: 'Member · Backend Engineer', photo: 'assets/member/Jorge Rodríguez.png', photoAlt: 'Foto de Jorge Rodríguez', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
          { name: 'Diego Pérez', role: 'Member · Data & AI', photo: 'assets/member/Diego Pérez.png', photoAlt: 'Foto de Diego Pérez', socials: [
            { platform: 'linkedin', href: 'https://www.linkedin.com' },
            { platform: 'github', href: 'https://github.com' },
            { platform: 'x', href: 'https://x.com' },
            { platform: 'instagram', href: 'https://www.instagram.com' },
            { platform: 'youtube', href: 'https://www.youtube.com/@TechRidersMedia' },
          ] },
        ],
      },
    ],
  },
  tutorials: {
    featuredCategories: ['Azure', '.NET', 'C#', 'Desarrollo', 'Windows Server', 'Docker', 'Kubernetes', 'Full Stack', 'Seguridad'],
  },
  intranet: {
    ambassadorStatusOptions: [
      { label: 'Todos', value: '' },
      { label: 'Activos', value: 'activo' },
      { label: 'Desactivados', value: 'desactivado' },
      { label: 'Pendientes', value: 'pendiente' },
    ],
    ambassadorAvailabilityOptions: [
      { label: 'Baja disponibilidad', value: '1 bloque semanal' },
      { label: 'Disponibilidad media', value: '2 o 3 bloques semanales' },
      { label: 'Alta disponibilidad', value: '4 o más bloques semanales' },
    ],
    staffPeriodOptions: [
      { label: 'Este mes', value: 'month' },
      { label: 'Este año', value: 'year' },
      { label: 'Todo', value: 'all' },
    ],
    memberCategoryOptions: ['FP Tour', 'Eventos', 'Mentorias', 'Podcast', 'Comunidad'],
    sessionStatusOptions: ['Pendiente', 'Realizada', 'Cancelada'],
    juniorSkillOptions: [
      'JavaScript', 'TypeScript', 'React', 'Angular', 'Vue.js',
      'Node.js', 'Python', 'Java', 'C++', 'HTML', 'CSS',
      'Sass', 'Bootstrap', 'Tailwind', 'Git', 'Docker',
      'SQL', 'MongoDB', 'REST APIs', 'GraphQL',
    ],
    juniorAvailabilityOptions: [
      { label: 'Inmediata', value: 'Inmediata' },
      { label: 'En 1 semana', value: 'En 1 semana' },
      { label: 'En 2 semanas', value: 'En 2 semanas' },
      { label: 'En 1 mes', value: 'En 1 mes' },
    ],
  },
};
