import { expect, test } from '@playwright/test';

type Role = 'admin' | 'empresa' | 'junior';

function setAuthSession(role: Role) {
  const user = {
    id: `e2e-${role}`,
    email: `${role}@techriders.es`,
    name: `E2E ${role}`,
    role
  };

  return {
    token: `fake-token-${role}`,
    user
  };
}

test.describe('Auth Guards', () => {
  test('redirects unauthenticated users to login modal', async ({ page }) => {
    await page.goto('/intranet/admin');

    await expect(page).toHaveURL(/\/?\?login=1&returnUrl=/);
    await expect(page.getByLabel(/email/i)).toBeVisible();
  });

  test('redirects junior user away from admin route', async ({ page }) => {
    const session = setAuthSession('junior');

    await page.addInitScript((payload) => {
      localStorage.setItem('token', payload.token);
      localStorage.setItem('user', JSON.stringify(payload.user));
    }, session);

    await page.goto('/intranet/admin');

    await expect(page).toHaveURL(/\/intranet\/junior$/);
    await expect(page.getByRole('heading', { name: /bienvenido/i })).toBeVisible();
  });

  test('allows admin user into admin dashboard', async ({ page }) => {
    const session = setAuthSession('admin');

    await page.addInitScript((payload) => {
      localStorage.setItem('token', payload.token);
      localStorage.setItem('user', JSON.stringify(payload.user));
    }, session);

    await page.goto('/intranet/admin');

    await expect(page).toHaveURL(/\/intranet\/admin$/);
    await expect(page.getByRole('heading', { name: /panel de administraci[oó]n/i })).toBeVisible();
  });

  test('allows empresa user into empresa dashboard', async ({ page }) => {
    const session = setAuthSession('empresa');

    await page.addInitScript((payload) => {
      localStorage.setItem('token', payload.token);
      localStorage.setItem('user', JSON.stringify(payload.user));
    }, session);

    await page.goto('/intranet/empresa');

    await expect(page).toHaveURL(/\/intranet\/empresa$/);
    await expect(page.getByRole('heading', { name: /panel de control/i })).toBeVisible();
  });
});
