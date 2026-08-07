# 🔍 SCSS Refactoring & Audit Plan

**Objetivo**: Migrar todos los SCSS a usar componentes centralizados de `_components.scss`  
**Estado**: En progreso  
**Última actualización**: May 24, 2026

---

## Principios de Refactorización

### ANTES (Duplicación)
```scss
.card {
  background: var(--bg-elevated);
  border-radius: var(--radius-lg);
  border: 1px solid var(--border-color);
  box-shadow: var(--shadow-sm);
}

.my-special-card {
  background: var(--bg-elevated);
  border-radius: var(--radius-lg);
  border: 1px solid var(--border-color);
  box-shadow: var(--shadow-sm);
  // Solo con pequeña diferencia
  border-top: 4px solid var(--tr-blue);
}
```

### DESPUÉS (Componentes centralizados)
```scss
.my-special-card {
  @extend .card;
  border-top: 4px solid var(--tr-blue);
}
```

---

## Archivos a Refactorizar

### ✅ COMPLETADOS

- [x] `admin-dashboard.scss` — Refactorizado (80 líneas → 50 líneas)
- [x] `design-tokens.scss` — Aliases & badge tokens añadidos
- [x] `_components.scss` — Creado (sistema completo)

### ⏳ PENDIENTES

| Archivo | Componentes Usados | Prioridad | Estimado |
|---------|-------------------|-----------|----------|
| `admin-staff.scss` | `.card`, `.table`, `.badge-*` | ALTA | 10min |
| `admin-colaboradores.scss` | `.card`, `.table`, `.badge-*` | ALTA | 10min |
| `admin-embajadores.scss` | `.card`, `.table`, `.badge-*`, `.grid-auto-fit` | ALTA | 15min |
| `intranet-home.scss` | `.card`, `.grid-2`, `.btn-primary` | MEDIA | 10min |
| `dashboard-empresa.scss` | `.stat-card`, `.card`, `.grid-auto-fit` | MEDIA | 15min |
| `ver-candidatos.scss` | `.card`, `.badge-*`, `.grid-auto-fit` | MEDIA | 15min |
| `mis-ofertas.scss` | `.card`, `.badge-*`, `.grid-auto-fill`, `.pagination` | MEDIA | 20min |
| `mis-cursos.scss` | `.card`, `.badge-*`, `.progress`, `.grid-auto-fill` | MEDIA | 20min |
| `editar-perfil.scss` | `.input-field`, `.card`, `.button-group` | MEDIA | 15min |
| `perfil-candidato.scss` | `.card`, `.stat-card`, `.feature-card`, `.grid-2` | MEDIA | 15min |
| `orienta-tech.scss` | `.card`, `.feature-card`, `.grid-auto-fit` | BAJA | 20min |
| `perfil-usuario.scss` | `.card`, `.input-field`, `.badge-*` | MEDIA | 10min |
| `login.scss` | `.input-field`, `.btn-primary`, `.card` | MEDIA | 5min |
| `conocimiento.scss` | `.card`, `.grid-auto-fill`, `.badge-info`, `.pagination` | MEDIA | 20min |
| `contacto.scss` | `.input-field`, `.btn-primary`, `.alert-*` | BAJA | 10min |
| `solicita.scss` | `.card`, `.btn-primary`, `.grid-2` | BAJA | 5min |
| `candidato.scss` | `.card`, `.feedback-card`, `.btn-primary` | BAJA | 10min |
| `quienes-somos.scss` | `.card`, `.grid-auto-fit`, `.feature-card` | BAJA | 15min |
| `home.scss` | `.card`, `.btn-primary`, `.grid-auto-fit`, `.stat-card` | BAJA | 25min |
| `sesiones.scss` | `.table`, `.input-field`, `.btn-primary` | BAJA | 10min |
| `staff.scss` (embajadores) | `.stat-card`, `.table`, `.card` | BAJA | 15min |

**Total**: ~270 minutos (~4.5 horas)

---

## Patrón de Refactorización

### Paso 1: Identificar componentes duplicados
```bash
# Buscar .card redefiniciones
grep -n "^\.card\|^\.stat-card\|^\.btn-" archivo.scss
```

### Paso 2: Consolidar
```scss
// ANTES
.my-card {
  background: var(--bg-elevated);
  border-radius: var(--radius-lg);
  border: 1px solid var(--border-color);
  box-shadow: var(--shadow-md);
  padding: 2rem;
}

// DESPUÉS
.my-card {
  @extend .card;
  padding: 2rem;
}
```

### Paso 3: Verificar que NO hay hardcoding
```bash
# Buscar hex colors
grep -n "#[0-9a-fA-F]\{3,6\}" archivo.scss

# Buscar 'white', 'black', etc.
grep -n "background:\s*white\|color:\s*black" archivo.scss

# Buscar hardcoded sombras
grep -n "box-shadow:\s*0" archivo.scss

# Buscar hardcoded espacios
grep -n "padding:\s*[0-9].*[px|rem]\|margin:\s*[0-9].*[px|rem]" archivo.scss
```

### Paso 4: Validar
- [ ] No hay `.card` redefinido
- [ ] No hay `.btn-*` redefinido
- [ ] No hay `background: white`
- [ ] No hay hex colors
- [ ] No hay hardcoded `box-shadow`
- [ ] No hay hardcoded `border-radius`
- [ ] Todo usa `var(--*)`

---

## Checklist por Archivo

### admin-staff.scss
```scss
// Reemplazar todo esto:
.stat { ... }          → @extend .stat-card;
.card { ... }          → @extend .card;
.staff-table { ... }   → @extend .table;
.badge { ... }         → @extend .badge-success/error/info;
```

### dashboard-empresa.scss
```scss
.stat-card { ... }     → @extend .stat-card; (+ border-left override)
.dashboard-section {   → @extend .card;
.badge { ... }         → @extend .badge-success/warning;
```

### mis-ofertas.scss
```scss
.ofertas-grid { ... }  → @extend .grid-auto-fill;
.oferta-card { ... }   → @extend .card-elevated;
.badge { ... }         → @extend .badge-success/warning/error;
.pagination { ... }    → @extend .pagination;
```

---

## Beneficios Post-Refactorización

| Métrica | ANTES | DESPUÉS | Mejora |
|---------|-------|---------|--------|
| Líneas SCSS totales | ~5,000 | ~2,500 | -50% |
| Duplicación de `.card` | 15+ | 1 | -99% |
| Duplicación de `.btn` | 12+ | 1 | -99% |
| Cambios para nuevo color | 20+ lugares | 1 lugar | -95% |
| Consistencia visual | Baja | Alta | 100% |
| Mantenibilidad | Difícil | Fácil | ✅ |

---

## Automático mediante Linting

Se puede configurar ESLint + SCSS linter para:
- ✅ Detectar `background: white` → warning
- ✅ Detectar `#fff` / hex → warning
- ✅ Detectar `box-shadow: 0` → requerir `var(--shadow-*)`
- ✅ Detectar `border-radius: [num]` → requerir `var(--radius-*)`

---

## Next Steps

1. **Refactorizar archivos de HIGH prioridad** (admin-* files)
2. **Implementar linting rules** para prevenir regresiones
3. **Auditar componentes** nuevos que entren al codebase
4. **Documentación para devs** (ya completado en `COMPONENT_LIBRARY.md`)

---

**Marcado como COMPLETADO cuando**: 
- [ ] Todos los archivos SCSS refactorizados
- [ ] 0 hardcoded colors/shadows/spaces
- [ ] 100% uso de `design-tokens`
- [ ] Linting rules implementadas
