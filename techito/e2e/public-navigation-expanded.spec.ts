import { expect, test } from '@playwright/test';

test.describe('Public navigation expanded', () => {
  test('navigates main public routes from header', async ({ page }) => {
    await page.route('**/api/events**', async (route) => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify([]),
      });
    });

    await page.route('**/api/tutorials/paginated**', async (route) => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ items: [], totalCount: 0, pageNumber: 1, pageSize: 12 }),
      });
    });

    await page.goto('/');
    const header = page.locator('header');

    await header.getByRole('link', { name: /^about us$/i }).click();
    await expect(page).toHaveURL(/\/about-us$/);
    await expect(page.getByRole('heading', { level: 2, name: /^staff$/i })).toBeVisible();

    await header.getByRole('link', { name: /^tutorials$/i }).click();
    await expect(page).toHaveURL(/\/tutorials$/);
    await expect(page.getByRole('heading', { level: 2, name: /^tutoriales$/i })).toBeVisible();

    await header.getByRole('link', { name: /^orientatech$/i }).click();
    await expect(page).toHaveURL(/\/orienta-tech$/);
    await expect(page.getByRole('heading', { name: /empresas y talent/i })).toBeVisible();

    await header.getByRole('link', { name: /^contact$/i }).click();
    await expect(page).toHaveURL(/\/contact$/);
    await expect(page.getByRole('heading', { level: 2, name: /^contacto$/i })).toBeVisible();
  });

  test('loads join page correctly', async ({ page }) => {
    await page.goto('/join');
    await expect(page).toHaveURL(/\/join$/);
    await expect(page.getByRole('heading', { level: 1, name: /únete al movimiento tech/i })).toBeVisible();
  });
});
