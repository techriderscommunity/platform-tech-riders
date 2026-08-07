/**
 * ===================================================================
 * ARIA & ACCESSIBILITY PATTERNS — TechRiders Design System
 * ===================================================================
 * Guidelines for semantic HTML, ARIA attributes, and keyboard navigation.
 * All components must implement these patterns for WCAG AA compliance.
 * ===================================================================
 */

import { Component, Input } from '@angular/core';

// ===== FORM COMPONENTS ===== //

/**
 * Form Input with ARIA patterns
 * Usage: <app-form-input [(ngModel)]="email" ariaLabel="Email Address" required aria-required="true"></app-form-input>
 */
@Component({
  selector: 'app-form-input',
  template: `
    <div class="form-group">
      <label [for]="inputId" [attr.aria-required]="required">{{ label }}</label>
      <input
        [id]="inputId"
        type="{{ type }}"
        [value]="value"
        (change)="onChange($event)"
        [disabled]="disabled"
        [attr.aria-label]="ariaLabel"
        [attr.aria-describedby]="helpTextId"
        [attr.aria-invalid]="hasError"
        [attr.required]="required"
        [attr.aria-required]="required"
        class="form-input"
        [class.is-error]="hasError"
        [class.is-success]="isSuccess"
      />
      @if (helpText) {
        <p [id]="helpTextId" class="help-text">{{ helpText }}</p>
      }
      @if (hasError) {
        <p
          [id]="errorId"
          role="alert"
          class="error-message"
          aria-live="polite"
        >
          {{ errorMessage }}
        </p>
      }
    </div>
  `,
})
export class FormInputComponent {
  @Input() label: string = '';
  @Input() type: string = 'text';
  @Input() value: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() ariaLabel: string = '';
  @Input() helpText: string = '';
  @Input() hasError: boolean = false;
  @Input() isSuccess: boolean = false;
  @Input() errorMessage: string = '';

  inputId = `input-${Math.random().toString(36).substr(2, 9)}`;
  helpTextId = `help-${this.inputId}`;
  errorId = `error-${this.inputId}`;

  onChange(event: Event) {
    // Handle input change
  }
}

// ===== BUTTON COMPONENTS ===== //

/**
 * Accessible Button with ARIA patterns
 * Usage: <app-button aria-label="Submit form" (click)="submit()">Submit</app-button>
 */
@Component({
  selector: 'app-button',
  template: `
    <button
      [attr.aria-label]="ariaLabel"
      [attr.aria-pressed]="ariaPressed"
      [attr.aria-disabled]="disabled"
      [disabled]="disabled"
      [class.btn]="true"
      [class.is-loading]="isLoading"
    >
      @if (!isLoading) {
        <span>{{ label }}</span>
      }
      @if (isLoading) {
        <span aria-live="polite" aria-atomic="true">Loading...</span>
      }
    </button>
  `,
})
export class ButtonComponent {
  @Input() label: string = '';
  @Input() ariaLabel: string = '';
  @Input() ariaPressed: boolean = false;
  @Input() disabled: boolean = false;
  @Input() isLoading: boolean = false;
}

// ===== DIALOG / MODAL COMPONENTS ===== //

/**
 * Accessible Modal with ARIA patterns
 * Usage: <app-modal [open]="isOpen" ariaLabelledby="modalTitle" (close)="isOpen = false"></app-modal>
 */
@Component({
  selector: 'app-modal',
  template: `
    @if (open) {
      <div
        role="dialog"
        [attr.aria-labelledby]="ariaLabelledby"
        [attr.aria-modal]="true"
        class="modal-backdrop"
        (keydown.escape)="close()"
      >
        <div class="modal-content">
          <h2 [id]="ariaLabelledby">{{ title }}</h2>
          <ng-content></ng-content>
          <button aria-label="Close dialog" (click)="close()">×</button>
        </div>
      </div>
    }
  `,
})
export class ModalComponent {
  @Input() open: boolean = false;
  @Input() title: string = '';
  @Input() ariaLabelledby: string = '';

  close() {
    this.open = false;
  }
}

// ===== ALERT / NOTIFICATION COMPONENTS ===== //

/**
 * Accessible Alert with ARIA patterns
 * Usage: <app-alert role="alert" aria-live="polite">Form submitted successfully!</app-alert>
 */
@Component({
  selector: 'app-alert',
  template: `
    <div
      role="alert"
      [attr.aria-live]="liveRegion"
      [attr.aria-atomic]="true"
      class="alert"
      [class.alert-info]="type === 'info'"
      [class.alert-success]="type === 'success'"
      [class.alert-error]="type === 'error'"
      [class.alert-warning]="type === 'warning'"
    >
      @if (icon) {
        <span class="alert-icon">{{ icon }}</span>
      }
      <ng-content></ng-content>
    </div>
  `,
})
export class AlertComponent {
  @Input() type: 'info' | 'success' | 'error' | 'warning' = 'info';
  @Input() liveRegion: 'polite' | 'assertive' = 'polite';
  @Input() icon: string = '';
}

// ===== NAVIGATION PATTERNS ===== //

/**
 * Accessible Navigation Menu
 * ARIA patterns for nested menus, active states, and keyboard navigation
 */
export class NavigationPatterns {
  /**
   * Main nav should use role="navigation" or semantic <nav>
   * Active link: aria-current="page"
   * Submenus: aria-expanded="true/false", aria-controls="submenu-id"
   */

  /* HTML Structure:
  <nav aria-label="Main navigation">
    <ul role="menubar">
      <li role="presentation">
        <a href="/home" aria-current="page">Home</a>
      </li>
      <li role="presentation">
        <button
          aria-expanded="false"
          aria-controls="submenu-features"
          aria-haspopup="true"
        >
          Features
        </button>
        <ul
          id="submenu-features"
          role="menu"
          hidden
          aria-label="Features submenu"
        >
          <li role="presentation"><a href="/features/1">Feature 1</a></li>
          <li role="presentation"><a href="/features/2">Feature 2</a></li>
        </ul>
      </li>
    </ul>
  </nav>
  */

  /**
   * Keyboard navigation:
   * - Tab: Move to next focusable element
   * - Shift+Tab: Move to previous focusable element
   * - Enter: Activate button/link
   * - Space: Toggle menu/checkbox
   * - Arrow keys: Navigate menu items (role="menubar")
   */
}

// ===== FORM VALIDATION PATTERNS ===== //

/**
 * Form Validation with ARIA
 * Pattern: aria-invalid="true" + aria-describedby for error messages
 */
export class FormValidationPattern {
  /*
  <div class="form-group">
    <label for="email">Email</label>
    <input
      id="email"
      type="email"
      aria-invalid="true"
      aria-describedby="email-error"
      class="is-error"
    />
    <p id="email-error" role="alert" class="error-message">
      Please enter a valid email address
    </p>
  </div>

  Success state:
  <input
    id="password"
    type="password"
    aria-invalid="false"
    class="is-success"
  />
  */
}

// ===== PROGRESSIVE DISCLOSURE (Show/Hide) ===== //

/**
 * Show/Hide Pattern with ARIA
 * Pattern: aria-expanded + aria-controls
 */
export class DisclosurePattern {
  /*
  <button
    aria-expanded="false"
    aria-controls="details-section"
  >
    Show more details
  </button>

  <div id="details-section" hidden>
    <!-- Hidden content -->
  </div>

  On toggle:
  - Set aria-expanded="true/false" on button
  - Toggle hidden attribute on content div
  */
}

// ===== LOADING STATES ===== //

/**
 * Loading indicator with ARIA live regions
 * Pattern: aria-live="polite" for non-intrusive updates
 *          aria-live="assertive" for urgent updates
 */
export class LoadingPattern {
  /*
  <div aria-live="polite" aria-busy="true" role="status">
    Loading data...
  </div>

  When done:
  <div aria-live="polite" aria-busy="false" role="status">
    Data loaded successfully
  </div>
  */
}

// ===== ACCESSIBILITY CHECKLIST ===== //

/**
 * Before shipping any component, verify:
 *
 * ✓ Semantic HTML: Use <button>, <input>, <nav>, <main>, etc.
 * ✓ ARIA roles: Only when semantic HTML cannot be used
 * ✓ ARIA labels: aria-label, aria-labelledby, aria-describedby
 * ✓ Form labels: <label for="id"> linked to input
 * ✓ Error messages: role="alert", aria-invalid="true"
 * ✓ Keyboard nav: Tab, Shift+Tab, Enter, Space, Arrow keys
 * ✓ Focus visible: :focus-visible style defined
 * ✓ Focus trap: Modal should trap focus inside
 * ✓ Skip links: Allow users to skip repetitive content
 * ✓ Color contrast: WCAG AA (4.5:1 for text, 3:1 for UI)
 * ✓ Font size: Minimum 14px for body text
 * ✓ Touch targets: Minimum 44px (WCAG 2.1 Level AAA)
 * ✓ Alt text: All images must have meaningful alt text
 * ✓ Live regions: Use aria-live for dynamic updates
 * ✓ Landmarks: Use <nav>, <main>, <aside>, <footer>
 * ✓ Screen reader testing: Test with NVDA, JAWS, or VoiceOver
 */

// ===== TESTING COMMANDS ===== //

/**
 * Accessibility testing:
 *
 * 1. Keyboard navigation:
 *    - Tab through entire page
 *    - Verify focus is visible and logical
 *    - Verify modals trap focus
 *
 * 2. Screen reader testing (NVDA on Windows):
 *    - Start NVDA
 *    - Navigate with arrow keys
 *    - Verify form labels, error messages, buttons are announced
 *
 * 3. Color contrast (axe DevTools extension):
 *    - Open Chrome DevTools → axe DevTools
 *    - Verify WCAG AA compliance
 *
 * 4. Automated testing:
 *    npm run a11y
 *    ngx-testing-library with axe-core for unit tests
 */
