---
name: 'design-patterns-library'
description: 'Librería completa de design patterns basada en Material Design, patrones ARIA, WAI-ARIA y patrones UX probados. Proporciona implementaciones de patrones testeadas, accesibles y reutilizables.'
---

# Skill de Librería de Design Patterns

Este skill proporciona una base de **design patterns probados y testeados** de fuentes líderes de la industria.

## Fundamentos y Referencias

### Estándares Oficiales
- **ARIA Authoring Practices**: https://www.w3.org/WAI/ARIA/apg/patterns/
- **Material Design System**: https://material.io/design
- **Apple Human Interface Guidelines**: https://developer.apple.com/design/human-interface-guidelines
- **WCAG 2.1 Guidelines**: https://www.w3.org/WAI/WCAG21/quickref/
- **WAI-ARIA 1.2**: https://www.w3.org/TR/wai-aria-1.2/

### Fuentes de Patrones Probados
- Shopify Polaris: https://polaris.shopify.com/
- Atlassian Design System: https://atlassian.design/
- IBM Carbon Design System: https://www.carbondesignsystem.com/
- GitHub Primer: https://primer.style/
- Figma Design Tokens: https://www.figma.com/resource-library/design-tokens/

## Categorías de Patrones

### 1. Patrones de Navegación

#### Patrón Tabs (ARIA Tab)
```typescript
// From: https://www.w3.org/WAI/ARIA/apg/patterns/tabs/

interface TabProps {
  label: string;
  id: string;
  isSelected: boolean;
  onChange: () => void;
}

export const Tab: React.FC<TabProps> = ({ label, id, isSelected, onChange }) => (
  <button
    role="tab"
    aria-selected={isSelected}
    aria-controls={`tabpanel-${id}`}
    id={`tab-${id}`}
    onClick={onChange}
    className={isSelected ? 'tab--active' : 'tab'}
  >
    {label}
  </button>
);

export const TabPanel: React.FC<{ id: string; children: React.ReactNode }> = ({ id, children }) => (
  <div
    role="tabpanel"
    id={`tabpanel-${id}`}
    aria-labelledby={`tab-${id}`}
    className="tab-panel"
  >
    {children}
  </div>
);
```

**Checklist del Patrón:**
- [ ] Navegación por teclado: teclas de flecha entre tabs
- [ ] Primer tab enfocado por defecto (teclas Home/End)
- [ ] `role="tab"` en los botones de tab
- [ ] `role="tabpanel"` en los paneles
- [ ] `aria-selected` en el tab activo
- [ ] `aria-controls` enlazando tab al panel
- [ ] `aria-labelledby` enlazando panel al tab
- [ ] El contenido del tab sigue al tab (orden DOM)

#### Navegación Breadcrumb
```typescript
// From: Material Design Navigation

export const Breadcrumbs: React.FC<{ items: BreadcrumbItem[] }> = ({ items }) => (
  <nav aria-label="Breadcrumb">
    <ol className="breadcrumbs">
      {items.map((item, index) => (
        <li key={item.id}>
          <a href={item.href} aria-current={index === items.length - 1 ? 'page' : undefined}>
            {item.label}
          </a>
          {index < items.length - 1 && <span aria-hidden="true">/</span>}
        </li>
      ))}
    </ol>
  </nav>
);
```

### 2. Patrones de Formulario

#### Patrón de Input Validado (ARIA Live Regions)
```typescript
// From: ARIA APG - Form Patterns

interface InputProps {
  label: string;
  error?: string;
  required?: boolean;
}

export const Input: React.FC<InputProps> = ({ label, error, required }) => {
  const inputId = useId();
  const errorId = useId();

  return (
    <div className="form-group">
      <label htmlFor={inputId}>
        {label}
        {required && <span aria-label="required">*</span>}
      </label>
      <input
        id={inputId}
        type="text"
        aria-invalid={!!error}
        aria-describedby={error ? errorId : undefined}
        required={required}
        className={error ? 'input--error' : 'input'}
      />
      {error && (
        <div id={errorId} role="alert" className="error-message">
          {error}
        </div>
      )}
    </div>
  );
};
```

**Checklist del Patrón:**
- [ ] Etiqueta asociada con `htmlFor`
- [ ] `aria-invalid` cuando hay error
- [ ] `aria-describedby` enlazando al mensaje de error
- [ ] El mensaje de error tiene `role="alert"`
- [ ] Indicador de requerido etiquetado
- [ ] Touch target >= 44x44px
- [ ] Mensajes de error claros
- [ ] Texto de ayuda con `aria-describedby`

#### Combobox / Autocomplete (ARIA Combobox)
```typescript
// From: https://www.w3.org/WAI/ARIA/apg/patterns/combobox/

// Complex pattern with:
- [ ] Role="combobox" on input
- [ ] Role="listbox" on dropdown
- [ ] Role="option" on items
- [ ] aria-expanded on trigger
- [ ] aria-haspopup="listbox"
- [ ] aria-owns linking combobox to listbox
- [ ] aria-activedescendant for focus management
- [ ] Keyboard support: ArrowDown, ArrowUp, Enter, Escape
- [ ] Filtering/search logic
- [ ] Touch accessible
```

### 3. Patrones de Disclosure/Accordion

#### Patrón Accordion
```typescript
// From: Material Design & ARIA APG

interface AccordionItemProps {
  title: string;
  children: React.ReactNode;
  defaultOpen?: boolean;
}

export const AccordionItem: React.FC<AccordionItemProps> = ({ title, children, defaultOpen }) => {
  const [isOpen, setIsOpen] = useState(defaultOpen ?? false);
  const headingId = useId();

  return (
    <div className="accordion-item">
      <h3 id={headingId} className="accordion-header">
        <button
          className="accordion-trigger"
          aria-expanded={isOpen}
          aria-controls={`panel-${headingId}`}
          onClick={() => setIsOpen(!isOpen)}
        >
          <span>{title}</span>
          <svg className="accordion-icon" aria-hidden="true">
            <use href="#icon-chevron" />
          </svg>
        </button>
      </h3>
      {isOpen && (
        <div id={`panel-${headingId}`} className="accordion-panel">
          {children}
        </div>
      )}
    </div>
  );
};
```

**Checklist del Patrón:**
- [ ] Botón con `aria-expanded`
- [ ] `aria-controls` en el botón
- [ ] El icono es `aria-hidden="true"`
- [ ] Soporte de teclado: Enter/Space para alternar
- [ ] Contenido oculto cuando está cerrado (DOM o display:none)
- [ ] Transiciones suaves
- [ ] Solo un panel abierto (opcional - depende del diseño)

### 4. Patrones de Diálogo/Modal

#### Diálogo Modal (ARIA Dialog)
```typescript
// From: ARIA APG - Dialog Pattern

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  children: React.ReactNode;
}

export const Modal: React.FC<ModalProps> = ({ isOpen, onClose, title, children }) => {
  const titleId = useId();

  useEffect(() => {
    if (isOpen) {
      document.body.style.overflow = 'hidden';
      // Trap focus
      return () => {
        document.body.style.overflow = '';
      };
    }
  }, [isOpen]);

  if (!isOpen) return null;

  return (
    <>
      {/* Backdrop */}
      <div
        className="modal-backdrop"
        onClick={onClose}
        aria-hidden="true"
      />
      {/* Dialog */}
      <div
        className="modal"
        role="dialog"
        aria-modal="true"
        aria-labelledby={titleId}
        aria-describedby="modal-description"
      >
        <div className="modal-header">
          <h2 id={titleId}>{title}</h2>
          <button
            onClick={onClose}
            aria-label="Close dialog"
            className="modal-close"
          >
            ×
          </button>
        </div>
        <div id="modal-description" className="modal-body">
          {children}
        </div>
      </div>
    </>
  );
};
```

**Checklist del Patrón:**
- [ ] `role="dialog"`
- [ ] `aria-modal="true"`
- [ ] `aria-labelledby` en el título
- [ ] `aria-describedby` en el contenido
- [ ] El backdrop evita interacción detrás del modal
- [ ] Trampa de foco (Tab dentro del modal)
- [ ] Tecla Escape para cerrar
- [ ] Previene scroll del body
- [ ] Devuelve el foco al cerrar
- [ ] El backdrop es `aria-hidden="true"`

### 5. Patrones de Notificación/Alerta

#### Alerta de Live Region
```typescript
// From: WCAG 2.1 & ARIA APG

interface AlertProps {
  message: string;
  type: 'info' | 'success' | 'warning' | 'error';
  onDismiss?: () => void;
}

export const Alert: React.FC<AlertProps> = ({ message, type, onDismiss }) => (
  <div
    className={`alert alert--${type}`}
    role="alert"
    aria-live="polite"
    aria-atomic="true"
  >
    <div className="alert-content">{message}</div>
    {onDismiss && (
      <button onClick={onDismiss} aria-label="Dismiss alert" className="alert-close">
        ×
      </button>
    )}
  </div>
);
```

**Pattern Checklist:**
- [ ] `role="alert"` for urgent notifications
- [ ] `aria-live="polite"` for non-urgent
- [ ] `aria-atomic="true"` to announce entire message
- [ ] Auto-dismiss after delay (optional)
- [ ] Keyboard dismissible
- [ ] Appropriate color + icon (not color alone)
- [ ] Contrast meets WCAG AA
- [ ] Positioned where users will see it

### 6. Menu Patterns

#### Dropdown Menu
```typescript
// From: ARIA APG - Menu Button Pattern

interface MenuProps {
  trigger: React.ReactNode;
  items: MenuItem[];
}

export const Menu: React.FC<MenuProps> = ({ trigger, items }) => {
  const [isOpen, setIsOpen] = useState(false);
  const menuId = useId();

  return (
    <div className="menu-wrapper">
      <button
        aria-haspopup="menu"
        aria-expanded={isOpen}
        onClick={() => setIsOpen(!isOpen)}
      >
        {trigger}
      </button>
      {isOpen && (
        <ul
          id={menuId}
          role="menu"
          className="menu"
          onKeyDown={(e) => {
            if (e.key === 'Escape') setIsOpen(false);
          }}
        >
          {items.map((item) => (
            <li key={item.id} role="none">
              <a role="menuitem" href={item.href}>
                {item.label}
              </a>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};
```

**Pattern Checklist:**
- [ ] `aria-haspopup="menu"` on trigger
- [ ] `aria-expanded` on trigger
- [ ] `role="menu"` on container
- [ ] `role="menuitem"` on items
- [ ] Keyboard: Arrow keys to navigate
- [ ] Enter/Space to activate
- [ ] Escape to close
- [ ] Mouse click outside closes menu
- [ ] First item focused when opened

## Layout & Grid Patterns

### Responsive Layout Breakpoints (Industry Standard)
```typescript
// From: Material Design & Web Best Practices

const breakpoints = {
  xs: 320,   // Mobile phone
  sm: 480,   // Mobile landscape
  md: 768,   // Tablet
  lg: 1024,  // Desktop
  xl: 1280,  // Large desktop
  xxl: 1536, // Extra large
};

const mediaQueries = {
  mobile: '@media (max-width: 479px)',
  tablet: '@media (min-width: 480px) and (max-width: 1023px)',
  desktop: '@media (min-width: 1024px)',
};
```

## Touch Target Size Pattern

```typescript
// From: WCAG 2.5.5 - Target Size (Level AAA)

// ✅ Recommended: 48x48px (Material Design)
// ✅ Minimum: 44x44px (Apple, WCAG AAA)
// ⚠️ Minimum: 24x24px (WCAG Level AA - but larger is better)

const touchTargetStyles = {
  button: {
    minHeight: '44px', // Touch target size
    minWidth: '44px',
    padding: '12px 16px', // Comfortable clicking
  },
  link: {
    minHeight: '44px', // Clickable area
    padding: '8px 12px',
  },
};
```

## Color & Contrast Patterns

```typescript
// From: WCAG 2.1 - Color Contrast

// Level AA (Minimum)
// - Normal text: 4.5:1 ratio
// - Large text (18pt+): 3:1 ratio

// Level AAA (Enhanced)
// - Normal text: 7:1 ratio
// - Large text: 4.5:1 ratio

const contrastValidator = {
  checkContrast: (foreground: string, background: string): boolean => {
    const l1 = getLuminance(foreground);
    const l2 = getLuminance(background);
    const lighter = Math.max(l1, l2);
    const darker = Math.min(l1, l2);
    const ratio = (lighter + 0.05) / (darker + 0.05);
    return ratio >= 4.5; // Level AA
  },
};
```

## Icon & Visual Indicators Pattern

```typescript
// From: WCAG 2.1 - Use of Color

// ✅ Do use multiple visual indicators
export const ErrorInput = () => (
  <div className="form-group">
    <input aria-invalid="true" className="input--error" />
    {/* Icon + Text + Color */}
    <svg className="error-icon" aria-hidden="true">
      <use href="#icon-error" />
    </svg>
    <span className="error-text">This field is required</span>
  </div>
);

// ❌ Don't rely on color alone
const BadBadge = ({ type }: { type: 'success' | 'error' }) => (
  <span style={{ color: type === 'success' ? 'green' : 'red' }}>
    Status
  </span>
);
```

## Animation & Motion Patterns

```typescript
// From: WCAG 2.3.3 - Animation from Interactions

// ✅ Respect prefers-reduced-motion
const transitionStyles = css`
  @media (prefers-reduced-motion: no-preference) {
    transition: all 300ms ease-in-out;
  }
  @media (prefers-reduced-motion: reduce) {
    transition: none;
  }
`;

// ✅ Avoid animations that flash
// - Frequency > 3 flashes per second = risk of seizures
// - Avoid photosensitive triggers
```

## Form Validation Pattern

```typescript
// From: ARIA APG - Form Validation

export const FormValidation = () => {
  const [errors, setErrors] = useState<Record<string, string>>({});

  const validate = (formData: FormData) => {
    const newErrors: Record<string, string> = {};

    if (!formData.email) newErrors.email = 'Email is required';
    else if (!isValidEmail(formData.email)) newErrors.email = 'Invalid email format';

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  return (
    <form
      onSubmit={(e) => {
        e.preventDefault();
        const formData = new FormData(e.currentTarget);
        if (validate(Object.fromEntries(formData))) {
          // Submit
        }
      }}
    >
      <Input
        name="email"
        aria-invalid={!!errors.email}
        aria-describedby={errors.email ? 'email-error' : undefined}
      />
      {errors.email && (
        <div id="email-error" role="alert" className="error">
          {errors.email}
        </div>
      )}
    </form>
  );
};
```

## Summary: Pattern Implementation Checklist

```markdown
For every component implementing a pattern:

### Accessibility
- [ ] Appropriate ARIA roles, states, properties
- [ ] Keyboard navigation fully supported
- [ ] Focus management correct
- [ ] Screen reader friendly
- [ ] Color contrast >= 4.5:1 (AA)
- [ ] Touch target >= 44x44px

### Usability
- [ ] Clear visual feedback for all states
- [ ] Consistent with platform conventions
- [ ] Error messages helpful and actionable
- [ ] No surprise focus changes
- [ ] Logical tab order

### Performance
- [ ] Minimal re-renders
- [ ] Efficient event handlers
- [ ] Optimized animations
- [ ] Responsive at all breakpoints

### Testing
- [ ] Unit tests for logic
- [ ] Integration tests for interactions
- [ ] a11y tests with axe
- [ ] Visual regression tests
- [ ] Manual keyboard testing
- [ ] Screen reader testing
```

## Resources

- **ARIA Authoring Practices**: https://www.w3.org/WAI/ARIA/apg/patterns/
- **Material Design Patterns**: https://material.io/design/patterns/
- **WCAG 2.1 Guidelines**: https://www.w3.org/WAI/WCAG21/quickref/
- **Shopify Polaris Patterns**: https://polaris.shopify.com/patterns
- **Inclusive Components**: https://inclusive-components.design/

## Related Skills

- `aria-accessibility-patterns` - Deep dive into ARIA
- `storybook-component-documentation` - Document patterns
- `design-system-generator` - Pattern-based systems
- `ux-audit` - Validate pattern compliance

