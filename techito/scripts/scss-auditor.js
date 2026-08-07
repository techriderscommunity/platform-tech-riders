#!/usr/bin/env node

/**
 * SCSS Compliance Auditor
 *
 * Verifica que los archivos SCSS cumplan con los estándares de TechRiders:
 * - No hardcoded colors (hex, named colors, rgb)
 * - No hardcoded box-shadows
 * - No hardcoded border-radius
 * - No duplicación de componentes
 * - Uso de design-tokens
 */

const fs = require('fs');
const path = require('path');

class SCSSAuditor {
  constructor(filePath) {
    this.filePath = filePath;
    this.content = fs.readFileSync(filePath, 'utf-8');
    this.lines = this.content.split('\n');
    this.issues = [];
  }

  audit() {
    console.log(`\n📋 Auditing: ${path.relative(process.cwd(), this.filePath)}\n`);

    this.checkHardcodedColors();
    this.checkHardcodedShadows();
    this.checkHardcodedBorderRadius();
    this.checkComponentDuplication();
    this.checkDesignTokenUsage();

    this.report();
    return this.issues;
  }

  checkHardcodedColors() {
    const hexPattern = /#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})(?![0-9a-fA-F])/g;
    const namedColorsPattern = /\b(white|black|red|blue|green|yellow|gray|grey|orange|purple|pink|cyan|magenta|lime|navy|teal|olive|coral|salmon|gold|silver|maroon|crimson|indigo|turquoise|khaki|orchid|violet|tan|chocolate)\b/gi;
    const rgbPattern = /rgb\([^)]+\)/g;

    this.lines.forEach((line, idx) => {
      if (line.includes('//') && line.indexOf('//') < line.search(/[\s]?(#|rgb|white|black)/)) return; // Skip comments

      // Hex colors (but allow in comments)
      if (!line.trim().startsWith('//') && line.match(hexPattern)) {
        this.issues.push({
          line: idx + 1,
          severity: 'error',
          rule: 'no-hardcoded-hex',
          message: `Hardcoded hex color found: "${line.trim()}"`,
          suggestion: 'Use design-token variable (e.g., var(--tr-blue))'
        });
      }

      // Named colors (except in comments)
      if (!line.trim().startsWith('//') && line.match(namedColorsPattern)) {
        const match = line.match(namedColorsPattern);
        if (match && !line.includes('var(--')) {
          this.issues.push({
            line: idx + 1,
            severity: 'error',
            rule: 'no-hardcoded-named-color',
            message: `Hardcoded named color found: "${line.trim()}"`,
            suggestion: 'Use design-token variable (e.g., var(--text-primary))'
          });
        }
      }

      // RGB colors
      if (!line.trim().startsWith('//') && line.match(rgbPattern)) {
        this.issues.push({
          line: idx + 1,
          severity: 'error',
          rule: 'no-hardcoded-rgb',
          message: `Hardcoded rgb color found: "${line.trim()}"`,
          suggestion: 'Use design-token variable'
        });
      }
    });
  }

  checkHardcodedShadows() {
    const shadowPattern = /box-shadow:\s*0\s+\d+px/i;

    this.lines.forEach((line, idx) => {
      if (!line.trim().startsWith('//') && line.match(shadowPattern)) {
        this.issues.push({
          line: idx + 1,
          severity: 'error',
          rule: 'no-hardcoded-shadow',
          message: `Hardcoded box-shadow found: "${line.trim()}"`,
          suggestion: 'Use design-token variable (e.g., var(--shadow-md))'
        });
      }
    });
  }

  checkHardcodedBorderRadius() {
    const radiusPattern = /border-radius:\s*[\d.]+(?:px|rem)\b/i;

    this.lines.forEach((line, idx) => {
      if (!line.trim().startsWith('//') && line.match(radiusPattern)) {
        this.issues.push({
          line: idx + 1,
          severity: 'error',
          rule: 'no-hardcoded-radius',
          message: `Hardcoded border-radius found: "${line.trim()}"`,
          suggestion: 'Use design-token variable (e.g., var(--radius-md))'
        });
      }
    });
  }

  checkComponentDuplication() {
    const componentPatterns = [
      { name: '.card', pattern: /^\s*\.card\s*{/ },
      { name: '.btn', pattern: /^\s*\.btn[^-]/ },
      { name: '.stat-card', pattern: /^\s*\.stat-card\s*{/ },
      { name: '.input-field', pattern: /^\s*\.input-field\s*{/ },
      { name: '.table', pattern: /^\s*\.table\s*{/ },
      { name: '.badge', pattern: /^\s*\.badge(?!-)\s*{/ },
    ];

    componentPatterns.forEach(comp => {
      const matches = this.lines.filter((line, idx) => {
        return line.match(comp.pattern) && !line.trim().startsWith('//');
      });

      if (matches.length > 0) {
        this.issues.push({
          line: -1,
          severity: 'warning',
          rule: 'component-redefinition',
          message: `Component '${comp.name}' redefined (found ${matches.length} definition(s))`,
          suggestion: `Use '@extend ${comp.name}' instead of redefining. Check _components.scss for base definition.`
        });
      }
    });
  }

  checkDesignTokenUsage() {
    const hardcodedSpaces = /(?:padding|margin|gap):\s*[\d.]+(?:px|rem)(?!\s*\/\/)/i;

    this.lines.forEach((line, idx) => {
      if (!line.trim().startsWith('//') && line.match(hardcodedSpaces) && !line.includes('var(--space')) {
        this.issues.push({
          line: idx + 1,
          severity: 'warning',
          rule: 'hardcoded-spacing',
          message: `Hardcoded spacing value found: "${line.trim()}"`,
          suggestion: 'Consider using design-token variables (e.g., var(--space-4))'
        });
      }
    });
  }

  report() {
    if (this.issues.length === 0) {
      console.log('✅ PASS: No issues found!\n');
      return;
    }

    const errors = this.issues.filter(i => i.severity === 'error');
    const warnings = this.issues.filter(i => i.severity === 'warning');

    if (errors.length > 0) {
      console.log(`❌ ERRORS (${errors.length}):`);
      errors.forEach(issue => {
        console.log(`  Line ${issue.line}: [${issue.rule}]`);
        console.log(`    ${issue.message}`);
        console.log(`    💡 ${issue.suggestion}\n`);
      });
    }

    if (warnings.length > 0) {
      console.log(`⚠️  WARNINGS (${warnings.length}):`);
      warnings.forEach(issue => {
        console.log(`  [${issue.rule}]`);
        console.log(`    ${issue.message}`);
        console.log(`    💡 ${issue.suggestion}\n`);
      });
    }

    const totalIssues = errors.length + warnings.length;
    console.log(`\n📊 Summary: ${errors.length} errors, ${warnings.length} warnings\n`);

    if (errors.length > 0) {
      process.exit(1);
    }
  }
}

// CLI
const filePath = process.argv[2];

if (!filePath) {
  console.log('Usage: node scss-auditor.js <path-to-scss-file>');
  console.log('Example: node scss-auditor.js src/app/features/admin/admin-dashboard.scss');
  process.exit(1);
}

if (!fs.existsSync(filePath)) {
  console.log(`❌ File not found: ${filePath}`);
  process.exit(1);
}

const auditor = new SCSSAuditor(filePath);
auditor.audit();
