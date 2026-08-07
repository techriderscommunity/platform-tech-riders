# 🚀 Component System — Quick Start Guide

**Para devs que refactorizan SCSS**  
Tiempo estimado: 5 minutos  
Requisito: Leer `COMPONENT_LIBRARY.md` primero

---

## 🎯 Goal

Convertir tu SCSS de esto:
```scss
.mi-card {
  background: var(--bg-elevated);
  border-radius: var(--radius-lg);
  padding: 2rem;
  box-shadow: var(--shadow-md);
  border: 1px solid var(--border-color);
}
```

A esto:
```scss
.mi-card {
  @extend .card;
  // Solo overrides específicos aquí
}
```

---

## 📋 Paso a Paso

### Paso 1: Audita tu archivo (2 min)
```bash
node scripts/scss-auditor.js src/app/features/tu-pagina/tu-pagina.scss
```

**Salida esperada**:
```
❌ ERRORS (5):
  Line 42: [no-hardcoded-hex]
    Hardcoded hex color found: "background: #f5f7fa;"
    💡 Use design-token variable (e.g., var(--bg-elevated))

  Line 78: [no-hardcoded-shadow]
    Hardcoded box-shadow found: "box-shadow: 0 4px 16px rgba(...)"
    💡 Use design-token variable (e.g., var(--shadow-md))

⚠️  WARNINGS (3):
  [hardcoded-spacing]
    Hardcoded spacing value found: "padding: 1.5rem;"
    💡 Consider using design-token variables (e.g., var(--space-6))

📊 Summary: 5 errors, 3 warnings
```

### Paso 2: Identifica componentes base (2 min)

Busca patrones que correspondan a componentes:

```scss
// ❌ ANTES
.mi-card {
  background: var(--bg-elevated);
  border-radius: var(--radius-lg);
  padding: 2rem;
  box-shadow: var(--shadow-md);
  border: 1px solid var(--border-color);
  transition: all 0.3s ease;
}

// ✅ DESPUÉS
.mi-card {
  @extend .card;
}
```

### Comparación con _components.scss

```scss
// En _components.scss, .card se define así:
.card {
  background: var(--bg-elevated);
  border-radius: var(--radius-lg);
  padding: 2rem;
  box-shadow: var(--shadow-md);
  border: 1px solid var(--border-color);
  transition: all 0.3s ease;
  
  &:hover {
    box-shadow: var(--shadow-lg);
    transform: translateY(-2px);
  }
}
```

### Paso 3: Reemplaza con @extend (1 min)

```scss
// ❌ ANTES
.mi-card {
  background: var(--bg-elevated);
  border-radius: var(--radius-lg);
  padding: 2rem;
  box-shadow: var(--shadow-md);
  border: 1px solid var(--border-color);
  transition: all 0.3s ease;
  
  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 40px rgba(0, 0, 0, 0.6);
  }
}

// ✅ DESPUÉS
.mi-card {
  @extend .card;
}
```

---

## 🔍 Mapping Rápido (Usa esto!)

| Si tienes... | USA este componente |
|---|---|
| `.card { background: white; border-radius; padding; box-shadow; }` | `@extend .card;` |
| `.stat-card { text-align: center; ... }` | `@extend .stat-card;` |
| `.btn { padding; border-radius; background; color; ... }` | `@extend .btn; @extend .btn-primary;` |
| `.input { label + input wrapper }` | `@extend .input-field;` |
| `.table { thead; tbody; border; }` | `@extend .table;` |
| `.badge { padding; border-radius; background; }` | `@extend .badge; @extend .badge-success;` |
| `.grid { display: grid; grid-template-columns; gap; }` | `@extend .grid-auto-fit;` |
| `.modal { position: fixed; background; ... }` | `@extend .modal-overlay;` |
| `.alert { background; border; padding; ... }` | `@extend .alert; @extend .alert-success;` |

---

## ❌ Cosas que NO debes hacer

### ❌ NO duplicar componentes
```scss
// ❌ MALO
.mi-nuevo-card {
  background: var(--bg-elevated);
  border-radius: var(--radius-lg);
  padding: 2rem;
  box-shadow: var(--shadow-md);
  border: 1px solid var(--border-color);
  // ... copiaste todo de .card
}

// ✅ BIEN
.mi-nuevo-card {
  @extend .card;
}
```

### ❌ NO hardcodear colores
```scss
// ❌ MALO
.header {
  background: white;
  color: #333;
  border: 1px solid #ddd;
}

// ✅ BIEN
.header {
  background: var(--bg-elevated);
  color: var(--text-primary);
  border: 1px solid var(--border-default);
}
```

### ❌ NO hardcodear espacios
```scss
// ❌ MALO
.container {
  padding: 16px;
  margin: 24px 0;
  gap: 8px;
}

// ✅ BIEN
.container {
  padding: var(--space-4);
  margin: var(--space-6) 0;
  gap: var(--space-2);
}
```

### ❌ NO hardcodear sombras
```scss
// ❌ MALO
.elevated {
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.5);
}

// ✅ BIEN
.elevated {
  box-shadow: var(--shadow-md);
}
```

### ❌ NO hardcodear border-radius
```scss
// ❌ MALO
.rounded {
  border-radius: 8px;
}

// ✅ BIEN
.rounded {
  border-radius: var(--radius-md);
}
```

---

## ✅ Checklist Final (Usa esto!)

Antes de hacer commit:

- [ ] Corrí `scss-auditor.js` y todo pasó ✅
- [ ] 0 hardcoded colors (no hex, no white/black)
- [ ] 0 hardcoded box-shadows (todo `var(--shadow-*)`)
- [ ] 0 hardcoded border-radius (todo `var(--radius-*)`)
- [ ] 0 hardcoded padding/margin (todo `var(--space-*)`)
- [ ] Usé `@extend .component` para componentes base
- [ ] Todos mis valores vienen de `design-tokens`
- [ ] No duplicué componentes que están en `_components.scss`
- [ ] Mi SCSS es 40%+ más pequeño que antes

---

## 📊 Resultados Esperados

### admin-dashboard.scss
```
ANTES: 180 líneas
DESPUÉS: 60 líneas
REDUCCIÓN: -67% ✅
```

---

## 🆘 Troubleshooting

### "¿Qué componente debo usar para X?"
→ Abre `COMPONENT_LIBRARY.md` y busca en la sección "Componentes Disponibles"

### "¿Cómo extiendo un componente?"
→ Usa `@extend .base-component;` y añade tus overrides

Ejemplo:
```scss
.mi-card-especial {
  @extend .card;
  border-top: 4px solid var(--tr-blue); // Solo lo específico
}
```

### "¿Qué variables tengo disponibles?"
→ Abre `design-tokens.scss` y busca `--` (todas las variables disponibles)

### "Mi SCSS tiene errores de compilación"
→ Probablemente usaste `var(--nombre-incorrecto)`
→ Verifica el nombre exacto en `design-tokens.scss`

---

## 📚 Archivos Importantes

| Archivo | Qué buscar | Cuándo |
|---------|-----------|--------|
| `design-tokens.scss` | Variables disponibles | Necesito saber qué tokens existen |
| `_components.scss` | Componentes base | Necesito ver cómo está implementado .card |
| `COMPONENT_LIBRARY.md` | Ejemplos de uso | Necesito saber cómo usar un componente |
| `REFACTORING_PLAN.md` | Prioridades | Necesito saber cuál archivo refactorizar |
| `scripts/scss-auditor.js` | Validación | Necesito auditar mi archivo |

---

## 🎯 Objetivo

**Resultado Final**: 
- ✅ Cero hardcoded values
- ✅ 100% design-token driven
- ✅ Máxima reutilización de componentes
- ✅ Cero duplicación
- ✅ SCSS 50%+ más pequeño

---

**¡Éxito! 🚀**

Si tienes dudas:
1. Revisa `COMPONENT_LIBRARY.md`
2. Corre `scss-auditor.js` para ver qué falta
3. Compara con `admin-dashboard.scss` (ejemplo refactorizado)
