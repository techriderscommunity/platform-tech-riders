import { readdirSync, readFileSync, statSync } from 'node:fs';
import { join } from 'node:path';

const root = process.cwd();
const featuresDir = join(root, 'src', 'app', 'features');

const nativeControlRegex = /<(input|textarea|select)\b|<button\b(?=[^>]*\bclass\s*=\s*["'][^"']*\bbtn-(primary|secondary|outline|text|small|sm|back|nav)\b)/gi;

const allowList = new Set();

function walk(dir) {
  const files = [];
  for (const entry of readdirSync(dir)) {
    const fullPath = join(dir, entry);
    const stats = statSync(fullPath);
    if (stats.isDirectory()) {
      files.push(...walk(fullPath));
      continue;
    }
    if (fullPath.endsWith('.html')) {
      files.push(fullPath);
    }
  }
  return files;
}

const htmlFiles = walk(featuresDir);
const violations = [];

for (const filePath of htmlFiles) {
  const relative = filePath.replace(/\\/g, '/').split('/techito/')[1];
  const source = readFileSync(filePath, 'utf-8');
  const hasNativeControls = nativeControlRegex.test(source);
  nativeControlRegex.lastIndex = 0;

  if (hasNativeControls && !allowList.has(relative)) {
    violations.push(relative);
  }
}

if (!violations.length) {
  console.log('Shared UI audit OK: no unmanaged native controls outside allowlist.');
  process.exit(0);
}

console.log('Shared UI audit: files pending migration (native controls found)');
for (const file of violations) {
  console.log(`- ${file}`);
}

if (process.argv.includes('--strict')) {
  process.exit(1);
}
