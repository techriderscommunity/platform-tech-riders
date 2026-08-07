/**
 * scss-tokenize.js
 * Batch-replaces hardcoded values with design token references in SCSS files.
 * Usage: node scripts/scss-tokenize.js
 */
const fs = require('fs');
const path = require('path');

// ─── Replacement rules (ordered: most specific first) ───────────────────────

const COLOR_RULES = [
  // Extended palette — indigo/gray (mis-cursos)
  { from: /#4f46e5\b/gi,   to: 'var(--palette-indigo-500)' },
  { from: /#2563eb\b/gi,   to: 'var(--palette-indigo-600)' },
  { from: /#1d4ed8\b/gi,   to: 'var(--palette-indigo-700)' },
  { from: /#f3f4f6\b/gi,   to: 'var(--palette-gray-100)' },
  { from: /#6b7280\b/gi,   to: 'var(--palette-gray-500)' },
  // Emerald badge (perfil-candidato)
  { from: /#d1fae5\b/gi,   to: 'var(--badge-light-emerald-bg)' },
  { from: /#065f46\b/gi,   to: 'var(--badge-light-emerald-text)' },
  // Error red (already a token)
  { from: /#ef4444\b/gi,   to: 'var(--accent-error)' },
  // Utility
  { from: /#fff\b/g,       to: 'var(--color-white)' },
  { from: /#ffffff\b/gi,   to: 'var(--color-white)' },
  { from: /\bwhite\b/g,    to: 'var(--color-white)', onlyInValuePosition: true },
  { from: /#000\b/g,       to: 'var(--color-black)' },
  { from: /#000000\b/gi,   to: 'var(--color-black)' },
  // Brand
  { from: /#ea4335\b/gi,   to: 'var(--brand-google)' },
  { from: /#4267b2\b/gi,   to: 'var(--brand-facebook)' },
  // Error light
  { from: /#ff7676\b/gi,   to: 'var(--accent-error-light)' },
  // Progress track
  { from: /#e5e7eb\b/gi,   to: 'var(--progress-track-bg)' },
  // Light-mode badge backgrounds
  { from: /#dcfce7\b/gi,   to: 'var(--badge-light-success-bg)' },
  { from: /#166534\b/gi,   to: 'var(--badge-light-success-text)' },
  { from: /#fef3c7\b/gi,   to: 'var(--badge-light-warning-bg)' },
  { from: /#92400e\b/gi,   to: 'var(--badge-light-warning-text)' },
  { from: /#dbeafe\b/gi,   to: 'var(--badge-light-info-bg)' },
  { from: /#1e40af\b/gi,   to: 'var(--badge-light-info-text)' },
  { from: /#fce7f3\b/gi,   to: 'var(--badge-light-accent-bg)' },
  { from: /#9d174d\b/gi,   to: 'var(--badge-light-accent-text)' },
  // Orienta-tech light palette
  { from: /#e3f2fd\b/gi,   to: 'var(--palette-light-blue-50)' },
  { from: /#b3e5fc\b/gi,   to: 'var(--palette-light-blue-100)' },
  { from: /#1a237e\b/gi,   to: 'var(--palette-navy-800)' },
  { from: /#1976d2\b/gi,   to: 'var(--palette-blue-700)' },
  { from: /#00bcd4\b/gi,   to: 'var(--palette-cyan-500)' },
  { from: /#fdf2f8\b/gi,   to: 'var(--palette-pink-100)' },
  { from: /#ec4899\b/gi,   to: 'var(--palette-pink-500)' },
  // Placeholder gradients (quienes-somos)
  { from: /#1e4a8a\b/gi,   to: 'var(--placeholder-blue-from)' },
  { from: /#0c6ea6\b/gi,   to: 'var(--placeholder-blue-to)' },
  { from: /#5b3a9e\b/gi,   to: 'var(--placeholder-purple-from)' },
  // Note: #8b2d6e is used as both purple-to and pink-from — purple-to wins
  { from: /#8b2d6e\b/gi,   to: 'var(--placeholder-purple-to)' },
  { from: /#047a8a\b/gi,   to: 'var(--placeholder-teal-from)' },
  { from: /#a12b42\b/gi,   to: 'var(--placeholder-pink-to)' },
  { from: /#a85210\b/gi,   to: 'var(--placeholder-orange-from)' },
  { from: /#b56a22\b/gi,   to: 'var(--placeholder-orange-to)' },
  { from: /#a88414\b/gi,   to: 'var(--placeholder-gold-from)' },
  { from: /#a8690b\b/gi,   to: 'var(--placeholder-gold-to)' },
  // Note: #0a7a5e appears as teal-to and green-from — use green-from
  { from: /#0a7a5e\b/gi,   to: 'var(--placeholder-green-from)' },
  { from: /#0d8a7a\b/gi,   to: 'var(--placeholder-green-to)' },
];

// Only replace in css value context (after a colon), avoid class names/selectors
const SPACING_RULES = [
  // Exact px values → numbered tokens
  { from: /\bgap:\s*8px\b/g,               to: 'gap: var(--space-2)' },
  { from: /\bgap:\s*12px\b/g,              to: 'gap: var(--space-3)' },
  { from: /\bgap:\s*16px\b/g,              to: 'gap: var(--space-4)' },
  { from: /\bgap:\s*24px\b/g,              to: 'gap: var(--space-6)' },
  { from: /\bgap:\s*32px\b/g,              to: 'gap: var(--space-8)' },
  { from: /\bgap:\s*40px\b/g,              to: 'gap: var(--space-10)' },
  // Rem gap values → fine-grained tokens
  { from: /\bgap:\s*0\.3rem\b/g,           to: 'gap: var(--space-0-3)' },
  { from: /\bgap:\s*0\.4rem\b/g,           to: 'gap: var(--space-0-4)' },
  { from: /\bgap:\s*0\.6rem\b/g,           to: 'gap: var(--space-0-6)' },
  { from: /\bgap:\s*0\.7rem\b/g,           to: 'gap: var(--space-0-7)' },
  { from: /\bgap:\s*0\.8rem\b/g,           to: 'gap: var(--space-0-8)' },
  { from: /\bgap:\s*1\.2rem\b/g,           to: 'gap: var(--space-1-2)' },
  { from: /\bgap:\s*2\.5rem\b/g,           to: 'gap: var(--space-10)' },
  // Padding exact px values
  { from: /\bpadding:\s*4px\s+12px\b/g,    to: 'padding: var(--space-1) var(--space-3)' },
  { from: /\bpadding:\s*4px\s+16px\b/g,    to: 'padding: var(--space-1) var(--space-4)' },
  { from: /\bpadding:\s*8px\s+16px\b/g,    to: 'padding: var(--space-2) var(--space-4)' },
  { from: /\bpadding:\s*12px\s+24px\b/g,   to: 'padding: var(--space-3) var(--space-6)' },
  // Padding rem values — use input-md-padding token where it exists
  { from: /\bpadding:\s*0\.7rem\s+0\.85rem\b/g, to: 'padding: var(--input-md-padding)' },
  { from: /\bpadding:\s*0\.7rem\s+0\.85rem\s+0\.7rem\s+0\.85rem\b/g, to: 'padding: var(--input-md-padding)' },
  { from: /\bpadding:\s*0\.7rem\s+1rem\b/g,     to: 'padding: var(--space-0-7) var(--space-md)' },
  { from: /\bpadding:\s*0\.7rem\s+1\.2rem\b/g,  to: 'padding: var(--space-0-7) var(--space-1-2)' },
  { from: /\bpadding:\s*0\.7rem\s+1\.5rem\b/g,  to: 'padding: var(--space-0-7) var(--space-lg)' },
  { from: /\bpadding:\s*0\.9rem\s+2\.5rem\b/g,  to: 'padding: var(--space-0-9) var(--space-10)' },
  { from: /\bpadding:\s*0\.875rem\s+1\.5rem\b/g,to: 'padding: var(--space-0-875) var(--space-lg)' },
  { from: /\bpadding:\s*0\.875rem\s+1\.75rem\b/g,to:'padding: var(--space-0-875) var(--space-1-75)' },
  { from: /\bpadding:\s*0\.625rem\s+1rem\b/g,   to: 'padding: var(--space-0-625) var(--space-md)' },
  { from: /\bpadding:\s*0\.375rem\s+0\.875rem\b/g, to: 'padding: var(--space-0-375) var(--space-0-875)' },
  { from: /\bpadding:\s*0\.375rem\s+0\.75rem\b/g,  to: 'padding: var(--space-0-375) var(--space-0-75)' },
  { from: /\bpadding:\s*0\.35rem\s+0\.75rem\b/g,   to: 'padding: var(--space-0-35) var(--space-0-75)' },
  { from: /\bpadding:\s*0\.2rem\s+0\.7rem\b/g,     to: 'padding: var(--space-0-2) var(--space-0-7)' },
  { from: /\bpadding:\s*0\.2rem\s+0\.55rem\b/g,    to: 'padding: var(--space-0-2) var(--space-0-55)' },
  { from: /\bpadding:\s*0\.2rem\s+0\.3rem\b/g,     to: 'padding: var(--space-0-2) var(--space-0-3)' },
  { from: /\bpadding:\s*0\.1rem\s+0\.2rem\b/g,     to: 'padding: var(--space-0-1) var(--space-0-2)' },
  { from: /\bpadding:\s*0\.4rem\s+0\.8rem\b/g,     to: 'padding: var(--space-0-4) var(--space-0-8)' },
  { from: /\bpadding:\s*0\.4rem\s+1rem\b/g,        to: 'padding: var(--space-0-4) var(--space-md)' },
  { from: /\bpadding:\s*1\.2rem\s+2rem\b/g,        to: 'padding: var(--space-1-2) var(--space-xl)' },
  { from: /\bpadding:\s*1\.2rem\b(?!\s+\S)/g,      to: 'padding: var(--space-1-2)' },
  { from: /\bpadding:\s*1\.2rem\s+1rem\s+2rem\s+1rem\b/g, to: 'padding: var(--space-1-2) var(--space-md) var(--space-xl) var(--space-md)' },
  // padding: Xrem 0 shorthand
  { from: /\bpadding:\s*0\.6rem\s+0\b/g,   to: 'padding: var(--space-0-6) 0' },
  { from: /\bpadding:\s*0\.7rem\s+0\b/g,   to: 'padding: var(--space-0-7) 0' },
  // Margin
  { from: /\bmargin:\s*0\.2rem\s+0\s+0\s+0\b/g,   to: 'margin: var(--space-0-2) 0 0 0' },
  { from: /\bmargin:\s*0\.4rem\s+auto\s+1rem\s+auto\b/g, to: 'margin: var(--space-0-4) auto var(--space-md) auto' },
];

const SHADOW_RULES = [
  { from: /box-shadow:\s*0 8px 18px rgba\(79,\s*70,\s*229,\s*0\.22\)/g,    to: 'box-shadow: var(--shadow-indigo)' },
  { from: /box-shadow:\s*0 6px 20px rgba\(0,\s*0,\s*0,\s*0\.15\)/g,        to: 'box-shadow: var(--shadow-card-hover)' },
  { from: /box-shadow:\s*0 4px 24px rgba\(33,\s*147,\s*176,\s*0\.08\)/g,   to: 'box-shadow: var(--shadow-cyan-sm)' },
  { from: /box-shadow:\s*0 2px 8px rgba\(33,\s*147,\s*176,\s*0\.1\)/g,     to: 'box-shadow: var(--shadow-cyan-xs)' },
  { from: /box-shadow:\s*0 4px 12px rgba\(33,\s*147,\s*176,\s*0\.15\)/g,   to: 'box-shadow: var(--shadow-cyan-md)' },
];

const RADIUS_RULES = [
  { from: /\bborder-radius:\s*2rem\b/g,    to: 'border-radius: var(--radius-full)' },
  { from: /\bborder-radius:\s*1rem\b/g,    to: 'border-radius: var(--radius-full)' },
  { from: /\bborder-radius:\s*1\.5rem\b/g, to: 'border-radius: var(--radius-2xl)' },
  { from: /\bborder-radius:\s*0\.9rem\b/g, to: 'border-radius: var(--radius-xl)' },
  { from: /\bborder-radius:\s*0\.75rem\b/g,to: 'border-radius: var(--radius-lg)' },
  { from: /\bborder-radius:\s*4px\b/g,     to: 'border-radius: var(--radius-sm)' },
  { from: /\bborder-radius:\s*2px\b/g,     to: 'border-radius: var(--radius-2)' },
  // 999px is effectively the same as radius-full
  { from: /\bborder-radius:\s*999px\b/g,   to: 'border-radius: var(--radius-full)' },
];

// ─── File processing ─────────────────────────────────────────────────────────

function applyRules(content, rules) {
  let result = content;
  for (const rule of rules) {
    if (rule.onlyInValuePosition) {
      // Only replace 'white' when it appears after ':' (CSS value context)
      // Use a negative lookbehind to avoid matching class names / selectors
      result = result.replace(/(?<=:\s*(?:[^{};]*[,\s])?)\bwhite\b(?!\s*-)/g, 'var(--color-white)');
    } else {
      result = result.replace(rule.from, rule.to);
    }
  }
  return result;
}

function processFile(filePath) {
  let content = fs.readFileSync(filePath, 'utf8');
  const original = content;

  content = applyRules(content, COLOR_RULES);
  content = applyRules(content, SPACING_RULES);
  content = applyRules(content, SHADOW_RULES);
  content = applyRules(content, RADIUS_RULES);

  if (content !== original) {
    fs.writeFileSync(filePath, content, 'utf8');
    return true;
  }
  return false;
}

// ─── Walk src/app for SCSS files ─────────────────────────────────────────────

function walk(dir) {
  const results = [];
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) results.push(...walk(full));
    else if (entry.name.endsWith('.scss')) results.push(full);
  }
  return results;
}

const appDir = path.join(__dirname, '..', 'src', 'app');
const files = walk(appDir);
let changed = 0;

for (const f of files) {
  if (processFile(f)) {
    changed++;
    console.log(`✅ ${path.relative(appDir, f)}`);
  }
}

console.log(`\nDone. ${changed}/${files.length} files updated.`);
