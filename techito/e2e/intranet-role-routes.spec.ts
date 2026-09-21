import { expect, test } from '@playwright/test';

type Role = 'admin' | 'empresa' | 'junior';

function setAuthSession(role: Role) {
  return {
    token: `fake-token-${role}`,
    user: {
      id: `e2e-${role}`,
      email: `${role}@techriders.es`,
      name: `E2E ${role}`,
      role,
    },
  };
}

async function withSession(page: Parameters<typeof test>[0]['page'], role: Role) {
  const session = setAuthSession(role);
  await page.addInitScript((payload) => {
    localStorage.setItem('token', payload.token);
    localStorage.setItem('user', JSON.stringify(payload.user));
  }, session);
}

test.describe('Intranet role routes', () => {
  test('guest can preview the intranet home without login', async ({ page }) => {
    await page.goto('/intranet');
    await expect(page).toHaveURL(/\/intranet$/);
    await expect(page.getByRole('heading', { name: /bienvenido/i })).toBeVisible();
  });

  test('admin can access admin routes', async ({ page }) => {
    await withSession(page, 'admin');

    await page.goto('/intranet/admin');
    await expect(page).toHaveURL(/\/intranet\/admin$/);
    await expect(page.getByText(/panel de administraci[oó]n/i).first()).toBeVisible();

    await page.goto('/intranet/admin/staff');
    await expect(page).toHaveURL(/\/intranet\/admin$/);
    await expect(page.getByText(/panel de administraci[oó]n/i).first()).toBeVisible();
  });

  test('logged-in user lands on the intranet root', async ({ page }) => {
    await withSession(page, 'admin');

    await page.goto('/');
    await expect(page).toHaveURL(/\/intranet$/);
  });

  test('empresa can access empresa routes', async ({ page }) => {
    await withSession(page, 'empresa');

    await page.goto('/intranet/company');
    await expect(page).toHaveURL(/\/intranet\/company$/);
    await expect(page.getByRole('heading').first()).toBeVisible();

    await page.goto('/intranet/admin');
    await expect(page).toHaveURL(/\/intranet\/company$/);
    await expect(page.getByRole('heading').first()).toBeVisible();
  });

  test('junior can access junior routes', async ({ page }) => {
    await withSession(page, 'junior');

    await page.goto('/intranet/junior');
    await expect(page).toHaveURL(/\/intranet\/junior$/);
    await expect(page.getByRole('heading', { name: /bienvenido/i })).toBeVisible();

    await page.goto('/intranet/junior/my-courses');
    await expect(page).toHaveURL(/\/intranet\/junior\/my-courses$/);
    await expect(page.getByRole('heading', { name: /mis cursos/i })).toBeVisible();
  });

  test('wrong role is redirected to its own intranet area', async ({ page }) => {
    await withSession(page, 'empresa');

    await page.goto('/intranet/admin');
    await expect(page).toHaveURL(/\/intranet\/company$/);
  });
});
