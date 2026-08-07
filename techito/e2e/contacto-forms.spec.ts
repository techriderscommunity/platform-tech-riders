import { expect, test } from '@playwright/test';

test.describe('Contacto forms', () => {
  test('submits contacto form and shows success toaster', async ({ page }) => {
    await page.route('**/api/contact', async (route) => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ ok: true }),
      });
    });

    await page.goto('/contact');

    const contacto = page.locator('#contacto');

    await contacto.locator('app-ui-text-field .ui-field-input').first().fill('E2E User');
    await contacto.locator('app-ui-text-field .ui-field-input').nth(1).fill('e2e@techriders.es');
    await contacto.locator('app-ui-textarea .ui-textarea-input').fill('Mensaje de prueba para contacto.');

    await contacto.getByRole('button', { name: /enviar mensaje/i }).click();

    await expect(page.getByText(/mensaje enviado/i)).toBeVisible();
  });
});
