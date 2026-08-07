import { expect, test } from '@playwright/test';

test.describe('Public Smoke', () => {
  test('loads homepage and global navigation', async ({ page }) => {
    await page.goto('/');

    await expect(page).toHaveURL(/\/$/);
    await expect(page.locator('header .nav-logo')).toBeVisible();
    await expect(page.locator('header .nav-links').getByRole('link', { name: /^contacto$/i })).toBeVisible();
  });

  test('can navigate to login from direct route', async ({ page }) => {
    await page.goto('/login');

    await expect(page).toHaveURL(/\/?\?login=1/);
    await expect(page.getByLabel(/email/i)).toBeVisible();
    await expect(page.getByRole('button', { name: /entrar/i })).toBeVisible();
  });
});
