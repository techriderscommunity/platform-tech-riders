# 🎨 TechRiders Frontend

**Angular 20 SPA** — Dark Navy Design System | Standalone Components | OnPush Change Detection

---

## 📦 Project Structure

```
techito/
├── src/
│   ├── app/                    ← Angular components & services
│   ├── assets/                 ← Images, fonts, static files
│   ├── environments/           ← Environment configuration
│   ├── design-tokens.scss      ← Single source of truth (60+ variables)
│   ├── _components.scss        ← Reusable component library (20+ components)
│   ├── styles.scss             ← Global styles
│   └── main.ts, server.ts
│
├── docs/                       ← Developer documentation
│   ├── QUICK_START.md          ← How to refactor SCSS (10 min)
│   ├── COMPONENTS.md           ← Component library reference
│   └── REFACTORING_PLAN.md     ← What files to refactor (priorities)
│
├── public/                     ← Public assets
├── content-source/             ← Content (tutorials, events)
├── angular.json                ← Angular config
├── tsconfig.json               ← TypeScript config
├── karma.conf.js               ← Unit test runner
└── README.md                   ← You are here
```

---

## 🎯 Design System

All styling uses **centralized design tokens** (zero hardcoded values):

- **`design-tokens.scss`** — 60+ CSS variables (colors, spacing, shadows, typography, z-index)
- **`_components.scss`** — 20+ reusable components (.card, .btn, .grid, .input, .table, .badge, etc.)
- **`styles.scss`** — Global styles importing both above

### Usage
```scss
// ✅ Correct
.my-component {
  @extend .card;
  background: var(--bg-elevated);
  padding: var(--space-4);
  border-radius: var(--radius-md);
}

// ❌ Wrong
.my-component {
  background: white;
  padding: 1rem;
  border-radius: 8px;
}
```

---

## 🚀 Getting Started

### Development
```bash
ng serve
# Open http://localhost:4200
```

### Build Production
```bash
ng build --configuration production
```

### Run Tests
```bash
ng test
```

### Run E2E (Playwright)
```bash
# install browser once
npm run e2e:install

# run headless e2e suite
npm run e2e
```

The E2E baseline validates:
- public smoke navigation
- auth guard redirect to login
- role guard redirects by role
- protected route access by valid role

---

## 📚 SCSS Development

### 1. Use Existing Components
```html
<div class="card">
  <div class="card-header"><h2>Title</h2></div>
  <div class="card-body">Content</div>
</div>

<button class="btn btn-primary">Click</button>

<div class="grid-auto-fit">
  <div class="card">Item 1</div>
  <div class="card">Item 2</div>
</div>
```

### 2. Refactor Pages
See: **[docs/QUICK_START.md](docs/QUICK_START.md)** (10 min guide)

### 3. Find Components
See: **[docs/COMPONENTS.md](docs/COMPONENTS.md)** (complete reference)

### 4. Check Priorities
See: **[docs/REFACTORING_PLAN.md](docs/REFACTORING_PLAN.md)** (what to refactor next)

### 5. Validate SCSS
```bash
node scripts/scss-auditor.js src/app/features/your-page/your-page.scss
# ✅ PASS: No issues found!
```

---

## 🎨 Available Components

| Category | Components |
|----------|-----------|
| **Cards** | `.card`, `.card-elevated`, `.card-secondary` |
| **Buttons** | `.btn`, `.btn-primary`, `.btn-secondary`, `.btn-outline`, `.btn-sm`, `.btn-lg` |
| **Forms** | `.input-field`, `.input-sm` |
| **Grids** | `.grid-auto-fit`, `.grid-auto-fill`, `.grid-2`, `.grid-3` |
| **Tables** | `.table`, `.table-responsive` |
| **Badges** | `.badge`, `.badge-info`, `.badge-success`, `.badge-warning`, `.badge-error` |
| **Layout** | `.section-header`, `.stat-card`, `.feature-card`, `.carousel` |
| **Other** | `.tag`, `.chip`, `.pagination`, `.progress`, `.modal-*`, `.alert-*` |

---

## 🔄 SCSS Refactoring Status

| Phase | Status | Files | Est. Time |
|-------|--------|-------|-----------|
| **Infrastructure** | ✅ Complete | design-tokens, _components, styles | — |
| **Example** | ✅ Complete | admin-dashboard | — |
| **High Priority** | ⏳ Queue | admin-staff, admin-colaboradores, admin-embajadores, dashboard-empresa, mis-ofertas | 1 hour |
| **Medium Priority** | 📅 Next | mis-cursos, perfil-usuario, editar-perfil, ver-candidatos, intranet-home | 1.5 hours |
| **Low Priority** | 📅 Future | conocimiento, orienta-tech, home, contacto, login, +6 more | 2 hours |

---

## 📖 Documentation

| File | Purpose | Read Time |
|------|---------|-----------|
| **[docs/QUICK_START.md](docs/QUICK_START.md)** | Step-by-step refactoring guide for developers | 10 min |
| **[docs/COMPONENTS.md](docs/COMPONENTS.md)** | Complete component library reference with examples | 15 min |
| **[docs/REFACTORING_PLAN.md](docs/REFACTORING_PLAN.md)** | Prioritized list of 20 files to refactor | 10 min |

---

## 🛠️ Key Tools

### SCSS Auditor
Validates SCSS compliance (no hardcoded colors, shadows, etc.):
```bash
node scripts/scss-auditor.js <file.scss>
```

---

## 🔗 Related

- **Backend**: [../backend](../backend) — .NET 10 API
- **Planning**: [../plan](../plan) — Architecture & ADRs
- **Content**: [content-source/](content-source/) — Tutorials & events

---

## ✅ Quick Checklist

Before committing SCSS changes:
- [ ] Ran `scss-auditor.js` and passed ✅
- [ ] No hardcoded colors (no hex, white, black)
- [ ] No hardcoded shadows
- [ ] No hardcoded border-radius
- [ ] No hardcoded spacing (using `var(--space-*)`)
- [ ] Used `@extend .component;` instead of duplicating
- [ ] All values come from design-tokens

---

**Last Updated**: May 24, 2026  
**Status**: ✅ Production Ready

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
