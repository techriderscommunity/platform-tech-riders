---
description: 'Estilos de componentes Angular: domina ViewEncapsulation (Emulated/ShadowDom/None), variables CSS, design tokens, patrones responsivos, organización SCSS, accesibilidad en estilos y testing de componentes.'
applyTo: '**.component.scss, **.component.css, **.component.ts'
---

# Guía de Estilos de Componentes Angular

Angular ofrece potentes capacidades de estilos mediante ViewEncapsulation y encapsulación CSS. Domina estos patrones para componentes listos para producción.

Referencia: [Guía de Estilos Angular](https://angular.dev/guide/components/styling)

## Estrategias de ViewEncapsulation

### 1. ViewEncapsulation.Emulated (Por defecto)

Angular emula la encapsulación de estilos usando atributos. Los estilos se limitan al componente sin Shadow DOM nativo.

```typescript
import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-button',
  template: `<button class="btn">Click me</button>`,
  styles: [`
    .btn {
      background-color: var(--color-primary);
      color: var(--color-on-primary);
      padding: 8px 16px;
      border-radius: 4px;
    }
  `],
  encapsulation: ViewEncapsulation.Emulated  // Default
})
export class ButtonComponent {}
```

**Comportamiento:**
- Estilos limitados al componente (se añaden selectores de atributo)
- Estilos externos NO se aplican dentro del componente
- Estilos del componente NO se filtran fuera
- Ideal para la mayoría de componentes

**Limitaciones:**
- Características de Shadow DOM no disponibles
- Se necesita ::ng-deep para atravesar la encapsulación

### 2. ViewEncapsulation.ShadowDom

Usa Shadow DOM nativo para encapsulación real.

```typescript
@Component({
  selector: 'app-card',
  template: `<div class="card"><ng-content></ng-content></div>`,
  styles: [`
    .card {
      box-shadow: var(--shadow-md);
      padding: var(--spacing-md);
      border-radius: var(--radius-md);
    }
  `],
  encapsulation: ViewEncapsulation.ShadowDom
})
export class CardComponent {}
```

**Ventajas:**
- Encapsulación nativa real
- Mejor rendimiento
- Reset CSS dentro del Shadow DOM

**Desventajas:**
- Soporte de navegadores (IE 11 no soportado)
- Los estilos no pueden acceder a variables CSS del padre (a veces)
- El estilado de contenido slotted requiere ::slotted()

### 3. ViewEncapsulation.None

Sin encapsulación - alcance global.

```typescript
@Component({
  selector: 'app-layout',
  template: `<main class="layout"><ng-content></ng-content></main>`,
  styles: [`
    .layout {
      max-width: 1200px;
      margin: 0 auto;
    }
  `],
  encapsulation: ViewEncapsulation.None  // Global styles
})
export class LayoutComponent {}
```

**Casos de Uso:**
- Estilos globales, utilidades
- Proveedores de tema
- Componentes de layout
- Estilos base (resets)

**Riesgos:**
- Conflictos de estilos
- Difícil de mantener
- Usa namespacing para evitar colisiones

## Variables CSS en Angular

### Definir Tokens

```typescript
// styles.css (Global)
:root {
  // Colors
  --color-primary: #0066cc;
  --color-on-primary: #ffffff;
  --color-secondary: #f0f0f0;
  --color-on-secondary: #000000;
  --color-error: #dc3545;
  --color-success: #28a745;

  // Spacing (8px grid)
  --spacing-xs: 4px;
  --spacing-sm: 8px;
  --spacing-md: 16px;
  --spacing-lg: 24px;
  --spacing-xl: 32px;

  // Typography
  --font-size-sm: 14px;
  --font-size-base: 16px;
  --font-size-lg: 18px;
  --line-height-normal: 1.5;

  // Shadows
  --shadow-sm: 0 1px 2px rgba(0,0,0,0.05);
  --shadow-md: 0 4px 6px rgba(0,0,0,0.1);
  --shadow-lg: 0 10px 15px rgba(0,0,0,0.1);

  // Border Radius
  --radius-sm: 2px;
  --radius-md: 4px;
  --radius-lg: 8px;
}

// Dark Mode
[data-theme="dark"] {
  --color-primary: #3399ff;
  --color-on-primary: #000000;
  --color-secondary: #2a2a2a;
  --color-on-secondary: #ffffff;
}
```

### Uso en Componente

```typescript
@Component({
  selector: 'app-button',
  template: `<button [class.btn-disabled]="disabled">{{ label }}</button>`,
  styles: [`
    button {
      padding: var(--spacing-sm) var(--spacing-md);
      background-color: var(--color-primary);
      color: var(--color-on-primary);
      border-radius: var(--radius-md);
      font-size: var(--font-size-base);
      line-height: var(--line-height-normal);
      box-shadow: var(--shadow-sm);
      cursor: pointer;
      transition: all 200ms ease;
    }

    button:hover:not(:disabled) {
      box-shadow: var(--shadow-lg);
    }

    button:focus-visible {
      outline: 2px solid var(--color-focus, #0066cc);
      outline-offset: 2px;
    }

    button:disabled {
      opacity: 0.5;
      cursor: not-allowed;
    }

    .btn-disabled {
      background-color: var(--color-disabled);
    }
  `]
})
export class ButtonComponent {
  @Input() label = 'Click me';
  @Input() disabled = false;
}
```

## Patrones Responsivos

```typescript
@Component({
  selector: 'app-grid',
  template: `<div class="grid"><ng-content></ng-content></div>`,
  styles: [`
    .grid {
      display: grid;
      grid-template-columns: 1fr;  /* Mobile: 1 column */
      gap: var(--spacing-md);
      padding: var(--spacing-md);
    }

    /* Tablet: 2 columns */
    @media (min-width: 640px) {
      .grid {
        grid-template-columns: repeat(2, 1fr);
      }
    }

    /* Desktop: 3 columns */
    @media (min-width: 1024px) {
      .grid {
        grid-template-columns: repeat(3, 1fr);
        gap: var(--spacing-lg);
        padding: var(--spacing-lg);
      }
    }
  `]
})
export class GridComponent {}
```

## Organización SCSS en Angular

```scss
// button.component.scss

// Variables (local to component)
$button-height: 44px;
$button-min-width: 48px;

// Mixins
@mixin button-variant($bg-color, $text-color) {
  background-color: $bg-color;
  color: $text-color;

  &:hover:not(:disabled) {
    background-color: darken($bg-color, 10%);
  }
}

@mixin focus-visible {
  &:focus-visible {
    outline: 2px solid var(--color-focus);
    outline-offset: 2px;
  }
}

// Component Styles
.button {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-height: $button-height;
  min-width: $button-min-width;

  padding: var(--spacing-sm) var(--spacing-md);
  border-radius: var(--radius-md);
  border: none;

  font-weight: 500;
  font-size: var(--font-size-base);
  cursor: pointer;
  transition: all 200ms ease;

  @include focus-visible;

  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }

  @media (prefers-reduced-motion: reduce) {
    transition: none;
  }
}

// Variants
.button--primary {
  @include button-variant(var(--color-primary), var(--color-on-primary));
}

.button--secondary {
  @include button-variant(var(--color-secondary), var(--color-on-secondary));
}
```

## Accesibilidad en Componentes Angular

```typescript
@Component({
  selector: 'app-button',
  template: `
    <button
      [attr.aria-label]="ariaLabel"
      [attr.aria-disabled]="disabled"
      [disabled]="disabled"
      (click)="onClick()"
    >
      {{ label }}
    </button>
  `,
  styles: [`
    button {
      min-height: 44px;  /* Touch target */
      min-width: 48px;

      &:focus-visible {
        outline: 2px solid var(--color-focus);
        outline-offset: 2px;
      }
    }
  `]
})
export class ButtonComponent {
  @Input() label = 'Button';
  @Input() ariaLabel = '';
  @Input() disabled = false;
  @Output() click = new EventEmitter<void>();

  onClick() {
    if (!this.disabled) {
      this.click.emit();
    }
  }
}
```

## Theme Support with Angular

```typescript
// theme.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

type Theme = 'light' | 'dark';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private theme$ = new BehaviorSubject<Theme>('light');

  constructor() {
    this.initTheme();
  }

  private initTheme() {
    const stored = localStorage.getItem('app-theme') as Theme;
    if (stored) {
      this.setTheme(stored);
    } else if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
      this.setTheme('dark');
    }
  }

  getTheme() {
    return this.theme$.asObservable();
  }

  setTheme(theme: Theme) {
    this.theme$.next(theme);
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('app-theme', theme);
  }

  toggleTheme() {
    this.setTheme(this.theme$.value === 'light' ? 'dark' : 'light');
  }
}

// app.component.ts
export class AppComponent {
  constructor(public themeService: ThemeService) {}

  toggleTheme() {
    this.themeService.toggleTheme();
  }
}
```

## Testing Styles in Angular

```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ButtonComponent } from './button.component';
import { getComputedStyle } from '@angular/platform-browser';

describe('ButtonComponent Styles', () => {
  let component: ButtonComponent;
  let fixture: ComponentFixture<ButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ButtonComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should have focus visible outline on button element', () => {
    const button = fixture.nativeElement.querySelector('button');

    // Simulate focus
    button.focus();
    button.dispatchEvent(new Event('focus'));

    const styles = window.getComputedStyle(button, ':focus-visible');
    expect(styles.outline).toContain('2px');
  });

  it('should apply primary color variant', () => {
    component.variant = 'primary';
    fixture.detectChanges();

    const button = fixture.nativeElement.querySelector('button');
    expect(button.classList.contains('button--primary')).toBe(true);
  });
});
```

## Best Practices Summary

1. **Use ViewEncapsulation.Emulated** for most components
2. **Define tokens in global styles**, use in components
3. **Support dark mode** with CSS variables
4. **Test responsive design** on actual devices
5. **Include focus indicators** (2px outline, 2px offset)
6. **Document accessibility** features in components
7. **Use SCSS for organization** (variables, mixins)
8. **Test visual styles** in unit tests
9. **Validate color contrast** with WCAG standards
10. **Support prefers-reduced-motion** for animations
